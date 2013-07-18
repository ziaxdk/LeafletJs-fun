using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ziax.dk.Domain
{
    public class GasStation : IPlaceable
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }


        public LatLon Position { get; set; }

        public GasStation()
        {
            Position = new LatLon();
        }
    }


}