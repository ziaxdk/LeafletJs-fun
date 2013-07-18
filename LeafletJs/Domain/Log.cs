using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ziax.dk.Domain
{

    public class Log
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }

        public Log()
        {
            CreatedUtc = DateTime.UtcNow;
            List = new Dictionary<DateTime, dynamic>();
        }

        public void AddToLog(dynamic item)
        {
            List.Add(DateTime.UtcNow, item);
        }

        public string Header { get; set; }
        public IDictionary<DateTime, dynamic> List { get; private set; }

        public DateTime CreatedUtc { get; private set; }
    }
}