using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ziax.dk.Domain
{
    public static class Ext
    {
        public static T[] Set<T>(this T[] array, T TModel, Func<T, bool> equals)
        {
            if (array == null || array.Length == 0)
            {
                return new T[] { TModel };
                ;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (equals(array[i]))
                {
                    array[i] = TModel;
                    return array;
                }
                else
                {
                    return array.Concat(new T[] { TModel }).ToArray();
                }
            }
            return null;
        }
    }

    public class User : IIdentity
    {
        [BsonId(IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string DefaultVehicleId { get; set; }

        public string[] UserGroupsId { get; set; }
        public ExternalAuthInner[] ExternalAuths { get; set; }

        public void AddExternalAuth(ExternalAuthInner auth)
        {
            ExternalAuths = ExternalAuths.Set(auth, x => x.Origin == auth.Origin);
        }

        public class ExternalAuthInner
        {
            public enum OriginEnum
            {
                Unknown,
                Facebook,
                Google
            }

            public OriginEnum Origin { get; set; }
            public string Id { get; set; }
            public string AccessToken { get; set; }
            public DateTime ExpireUtc { get; set; }
        }

        public string AuthenticationType
        {
            get;
            set;
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Id); }
        }
    }
}