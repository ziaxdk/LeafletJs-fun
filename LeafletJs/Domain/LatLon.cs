using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ziax.dk.Domain
{
    public interface IPlaceable
    {
        LatLon Position { get; }
    }

    public class LatLon : IBsonSerializable
    {

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        //public string AsWKT { get { return string.Format(Utils.InvariantCulture, "POINT ({0} {1})", Longitude, Latitude); } }


        public object Deserialize(MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            try
            {
                bsonReader.ReadStartArray();
                Longitude = bsonReader.ReadDouble();
                Latitude = bsonReader.ReadDouble();
                bsonReader.ReadEndArray();
            }
            catch
            {
                bsonReader.SkipValue();
            }

            return this;
        }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            throw new NotImplementedException();
        }

        public void Serialize(MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, IBsonSerializationOptions options)
        {
            bsonWriter.WriteStartArray();
            bsonWriter.WriteDouble(Longitude);
            bsonWriter.WriteDouble(Latitude);
            bsonWriter.WriteEndArray();
        }

        public void SetDocumentId(object id)
        {
            throw new NotImplementedException();
        }
    }
}