using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;

namespace LeafletJs.Controllers
{
    public class ClickController : ApiController
    {
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");

        public object Get(double lat, double lng)
        {
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");

            //var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPolygon<Temp>(new GeoJsonPolygonCoordinates<Temp>(new GeoJsonLinearRingCoordinates<Temp>(new[] { new Temp() }))));

            var t = new GeoJson2DCoordinates(lng, lat);
            var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPoint<GeoJson2DCoordinates>(t));


            //var z1 =
            //    new GeoJsonPolygon<GeoJson2DCoordinates>(
            //        new GeoJsonPolygonCoordinates<GeoJson2DCoordinates>(
            //            new GeoJsonLinearRingCoordinates<GeoJson2DCoordinates>(new[] {t})));

            //var q2 = Query<RestPostNummer>.Within(x => x.polygon, z1);

            return col.Find(q);
        }
    }

}
