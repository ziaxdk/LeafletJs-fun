using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using RestSharp;

namespace LeafletJs.Controllers
{
    public class FetchController : ApiController
    {
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");

        public object Put()
        {

            //{"href":"http:\/\/geo.oiorest.dk\/postnumre\/1000-1499.json","nr":"1000-1499","fra":"1000","til":"1499","navn":"København K","areal":"115090000","grænse":"http:\/\/geo.oiorest.dk\/postnumre\/1000-1499\/grænse.json","naboer":"http:\/\/geo.oiorest.dk\/postnumre\/1000-1499\/naboer.json"}
            //http://geo.oiorest.dk/postnumre.
            var client = new RestClient("http://geo.oiorest.dk/");
            var req = new RestRequest("postnumre.json", Method.GET);
            var res = client.Execute<List<RestPostNummer>>(req).Data;
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");

            foreach (var restPostNummer in res)
            {
                Console.WriteLine(restPostNummer.navn);
                var req2 = new RestRequest("postnumre/{nummer}/grænse.json", Method.GET);
                req2.AddUrlSegment("nummer", restPostNummer.nr);
                var res2 = client.Execute<RestPostNummer.Polygon>(req2).Data;
                restPostNummer.polygon = res2;

                //restPostNummer.polygon.coordinates = JsonConvert.DeserializeObject<List<List<List<List<double>>>>>(poly);
                //restPostNummer.polygon.type = "Polygon";
                //col.Save(restPostNummer);
            }
            return new { count = res.Count };

        }

    }
}
