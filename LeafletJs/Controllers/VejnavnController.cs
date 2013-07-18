using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RestSharp.Contrib;

namespace LeafletJs.Controllers
{
    public class VejnavnController : ApiController
    {
        private MongoDatabase mongo = new MongoClient("mongodb://localhost:27017/tester").GetServer().GetDatabase("tester");

        public object Get(string q, string zip)
        {
            var col = mongo.GetCollection<OpslagVejnavn>("Vejnavn");

            var regex = new BsonRegularExpression(string.Format("^{0}", q));
            var mq = Query<OpslagVejnavn>.EQ(x => x.VejnavnSearch, regex);

            if (string.IsNullOrEmpty(zip))
            {
                return col.Find(mq).ToList();
            }
            //var mq = Query<OpslagVejnavn>.EQ(x => x.VejnavnSearch, q);
            var mq2 = Query<OpslagVejnavn>.EQ(x => x.PostNummer, zip);


            var build = new QueryBuilder<OpslagVejnavn>().And(mq2, mq);
            //var debug = build.ToBsonDocument();
            //return "";

            var res = col.FindAs<OpslagVejnavn>(build).ToList();
            return res;
        }
    }
}
