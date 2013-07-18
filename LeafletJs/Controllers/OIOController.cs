using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using RestSharp;

namespace LeafletJs.Controllers
{
    public class OIOController : ApiController
    {
        private const bool DoSave = false;
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");

        RestClient clientGeo = new RestClient("http://geo.oiorest.dk/");
        RestClient client = new RestClient("http://oiorest.dk/");





        public object Put()
        {
            var req = new RestRequest("postnumre.json", Method.GET);
            var res = clientGeo.Execute<List<RestPostNummer>>(req).Data;
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");

            foreach (var restPostNummer in res)
            {
                System.Diagnostics.Debug.WriteLine(restPostNummer.nr);
                //DoPostDistrikt(restPostNummer);
                DoVejnavne(restPostNummer);
            }


            return "OK";
        }

        private void DoVejnavne(RestPostNummer restPostNummer)
        {
            var col = mongo.GetCollection<RestPostNummer>("Vejnavn");
            var req2 = new RestRequest("danmark/postdistrikter/{postNummer}/adresser.json", Method.GET);
            req2.AddUrlSegment("postNummer", restPostNummer.nr);
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
        }

        private void DoPostDistrikt(RestPostNummer restPostNummer)
        {
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");
            var req2 = new RestRequest("postnumre/{nummer}/grænse.json", Method.GET);
            req2.AddUrlSegment("nummer", restPostNummer.nr);
            var res2 = clientGeo.Execute<RestPostNummer.Polygon>(req2).Data;
            restPostNummer.polygon = res2;

            //col.Save(restPostNummer);
        }
    }
}
