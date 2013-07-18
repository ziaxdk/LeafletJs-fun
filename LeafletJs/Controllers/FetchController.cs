using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using RestSharp;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Globalization;

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

        public object Post()
        {
            var col = mongo.GetCollection<RestPostNummer>("Vejnavn");
            // http://oiorest.dk/danmark//postdistrikter/2640/adresser.json
            var client = new RestClient("http://oiorest.dk/");
            var req2 = new RestRequest("danmark/postdistrikter/{postNummer}/adresser.json", Method.GET);
            req2.AddUrlSegment("postNummer", "2640");
            var res2 = client.Execute<List<RootObject>>(req2).Data.Select(x => new OpslagVejnavn()
                                                                                   {
                                                                                       Vejnavn = x.vej.navn,
                                                                                       Nummer = x.husnr,
                                                                                       PostNummer = x.postdistrikt.nr,
                                                                                       By = x.postdistrikt.navn,
                                                                                       //Position2 = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(Convert.ToDouble(x.wgs84koor.latitude, CultureInfo.InvariantCulture), Convert.ToDouble(x.wgs84koor.longitude, CultureInfo.InvariantCulture)))
                                                                                   }).ToList();

            foreach (var opslagVejnavn in res2)
            {
                col.Save(opslagVejnavn);
            }
            //}
            return "OK";

            //new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates())
            //return new { count = res2.Count() };

        }

    }

    public class OpslagVejnavn
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Vejnavn { get; set; }
        public string VejnavnSearch { get; set; }
        public string Nummer { get; set; }
        public string PostNummer { get; set; }
        public string By { get; set; }
        //public GeoJsonPoint<GeoJson2DCoordinates> Position2 { get; set; }
        public GeoPoint Position2 { get; set; }

    }

    public class GeoPoint
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }
    }

    public class Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }




    public class Vej
    {
        public string navn { get; set; }
    }

    public class Postdistrikt
    {
        public string nr { get; set; }
        public string navn { get; set; }
    }

    public class Wgs84koor
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class RootObject
    {
        public Vej vej { get; set; }
        public string husnr { get; set; }
        public Postdistrikt postdistrikt { get; set; }
        public Wgs84koor wgs84koor { get; set; }
    }
}
