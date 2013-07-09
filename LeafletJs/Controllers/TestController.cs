using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;

namespace LeafletJs.Controllers
{
    public class TestController : ApiController
    {
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");

        public object Get()
        {
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");

            //var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPolygon<Temp>(new GeoJsonPolygonCoordinates<Temp>(new GeoJsonLinearRingCoordinates<Temp>(new[] { new Temp() }))));

            var t = new GeoJson2DCoordinates(12.231356, 55.647011);
            var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPoint<GeoJson2DCoordinates>(t));

            return col.Find(q);

            //return q.ToString();
        }
    }
}
