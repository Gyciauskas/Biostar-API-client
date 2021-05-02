using Newtonsoft.Json;
using System.Collections.Generic;

namespace SupremaMiddleware.Server
{
    public static class JsonSerializer
    {
        public static string Serialize(object model)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                { model.GetType().Name, model }
            });
        }

        public static T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
