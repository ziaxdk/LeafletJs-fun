using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using ziax.dk.Domain;

namespace LeafletJs.Controllers
{
    public class ZipController : ApiController
    {
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");


        public object Get(int zip)
        {
            var col = mongo.GetCollection<RestPostNummer>("Kommuner");
            var stations = mongo.GetCollection("GasStation");

            var q1 = Query<RestPostNummer>.LTE(x => x.fra, zip);
            var q2 = Query<RestPostNummer>.GTE(x => x.til, zip);

            var mb = new QueryBuilder<RestPostNummer>().And(q1, q2);

            var data = col.Find(mb).ToList();
            return data;
        }
    }

    public class RestPostNummer
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string id { get; set; }
        public string href { get; set; }
        public string nr { get; set; }
        public int fra { get; set; }
        public int til { get; set; }
        public string navn { get; set; }
        public long areal { get; set; }
        public string grænse { get; set; }
        public string naboer { get; set; }

        public Polygon polygon { get; set; }

        public RestPostNummer()
        {
            polygon = new Polygon();
        }

        public class Polygon
        {
            //public string type { get; set; }
            //public string coordinates { get; set; }

            public string type { get; set; }
            public List<List<List<List<double>>>> coordinates { get; set; }
        }

    }

    public class RestPostNummer2 : RestPostNummer
    {
        public IEnumerable<GasStation> GasStations { get; set; }
    }

    public class RestPostNummer3
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string id { get; set; }
        public string href { get; set; }
        public string nr { get; set; }
        public int fra { get; set; }
        public int til { get; set; }
        public string navn { get; set; }
        public long areal { get; set; }
        public string grænse { get; set; }
        public string naboer { get; set; }

        public Polygon polygon { get; set; }

        public RestPostNummer3()
        {
            polygon = new Polygon();
        }

        public class Polygon
        {
            public string type { get; set; }
            public string coordinates { get; set; }

        }

    }


}
