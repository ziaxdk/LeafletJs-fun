using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;
using RestSharp;
using ziax.dk.Domain;

namespace LeafletJs.Controllers
{
    public class ClickController : ApiController
    {
        private MongoDatabase mongoZiax = new MongoClient("mongodb://localhost:27017/ziax").GetServer().GetDatabase("ziax");
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


        public object Put(dd dd)
        {
            var col = mongo.GetCollection<RestPostNummer2>("Kommuner");
            var t = new GeoJson2DCoordinates(dd.lng, dd.lat);
            var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPoint<GeoJson2DCoordinates>(t));
            var zone = col.Find(q).SingleOrDefault();
            if (zone == null) return new object();
            zone.GasStations = GetStations(zone.polygon.coordinates);
            return zone;
        }


        private IEnumerable<GasStation> GetStations(List<List<List<List<double>>>> doc)
        {
            var colStations = mongoZiax.GetCollection<GasStation>("GasStations");
            var coords = doc[0][0];
            var allCoords = coords.Select(x => new GeoJson2DCoordinates(x[0], x[1]));


            
            
            var polyCoords = new GeoJsonPolygonCoordinates<GeoJson2DCoordinates>(new GeoJsonLinearRingCoordinates<GeoJson2DCoordinates>(allCoords));
            //var n = new GeoJsonMultiPolygonCoordinates<GeoJson2DCoordinates>(new [] { polyCoords });
            //var q = Query<GasStation>.GeoIntersects(x => x.Position, new GeoJsonMultiPolygon<GeoJson2DCoordinates>(n));
            //var stations1 = colStations.Find(q).ToList();

            var q2 = Query<GasStation>.Within(x => x.Position, new GeoJsonPolygon<GeoJson2DCoordinates>(polyCoords));
            var stations2 = colStations.Find(q2).ToList();

            //var polyCoords3 = new GeoJsonPolygonCoordinates<GeoJson2DCoordinates>(new GeoJsonLinearRingCoordinates<GeoJson2DCoordinates>(allCoords));
            //var q3 = Query<GasStation>.GeoIntersects(x => x.Position, new GeoJsonPolygon<GeoJson2DCoordinates>(polyCoords3));
            //var stations3 = colStations.Find(q3).ToList();

            //return null;

            return stations2;
        }

        public object Post(dd dd)
        {
            var colStations = mongoZiax.GetCollection<GasStation>("GasStations");
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");
            var t = new GeoJson2DCoordinates(dd.lng, dd.lat);
            var q = Query<RestPostNummer>.GeoIntersects(x => x.polygon, new GeoJsonPoint<GeoJson2DCoordinates>(t));
            var zone = col.Find(q).SingleOrDefault();


            //var allCoords = zone.polygon.coordinates[0][0].Select(x => new GeoJson2DCoordinates(x[0], x[1]));
            //var polyCoords = new GeoJsonPolygonCoordinates<GeoJson2DCoordinates>(new GeoJsonLinearRingCoordinates<GeoJson2DCoordinates>(allCoords));
            //var colStations = mongoZiax.GetCollection<GasStation>("GasStations");
            //var q2 = Query<GasStation>.Within(x => x.Position, new GeoJsonPolygon<GeoJson2DCoordinates>(polyCoords));

            BsonDocument bsonDocument = new BsonDocument("$within", new BsonDocument("$geometry", zone.polygon.ToJson()));
            //return (IMongoQuery)new QueryDocument(name, (BsonValue)bsonDocument);
            var qDoc = new QueryDocument("Position", bsonDocument);
            var q2 = colStations.Find(qDoc).ToList();

            return bsonDocument.ToString();
        }


        public class dd
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }

}
