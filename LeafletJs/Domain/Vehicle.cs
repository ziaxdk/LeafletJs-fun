using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ziax.dk.Domain
{
    public class Vehicle
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string Model { get; set; }
        public string FuelType { get; set; }

        public string[] UserById { get; set; }

    }
}