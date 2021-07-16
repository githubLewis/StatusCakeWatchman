using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman
{
    public class GreaterThanZeroContractResolverAttribute : Attribute
    {
    }

    public class GreaterThanZeroContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property != null && property.Writable )
            {
                var attributes = property.AttributeProvider.GetAttributes(typeof(GreaterThanZeroContractResolverAttribute), true);
                if (attributes != null && attributes.Count > 0)
                {
                    property.ShouldDeserialize = instance =>
                    {
                        try
                        {
                            PropertyInfo prop = (PropertyInfo)member;
                            if (prop.CanRead)
                            {
                                var value = prop.GetValue(instance, null);// getting default value(0) here instead of null for PropertyB
                                return (int)value > 0;
                            }
                        }
                        catch
                        {
                        }
                        return false;
                    };
                }

               // property.Writable = false;
            }
            return property;
        }
    }
}
