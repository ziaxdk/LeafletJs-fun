using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ziax.dk.Domain
{
    public class Gas : IPlaceable
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Units { get; set; }
        public decimal Price { get; set; }
        public int Odometer { get; set; }
        public LatLon Position { get; set; }
        public string[] Tags { get; set; }

        //public string VechicleId { get; set; }
        public VehicleInner Vehicle { get; set; }
        //public string StationId { get; set; }
        public StationInner Station { get; set; }

        public string[] VisibleToId { get; set; }

        public string CreatedById { get; set; }

        public Gas()
        {
            Position = new LatLon();
        }

        public class VehicleInner
        {
            public string Id { get; set; }
            public string Vendor { get; set; }
            public string Model { get; set; }
        }

        public class StationInner : IPlaceable
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Zipcode { get; set; }
            public string City { get; set; }


            public LatLon Position { get; set; }

            public StationInner()
            {
                Position = new LatLon();
            }
        }
    }


}