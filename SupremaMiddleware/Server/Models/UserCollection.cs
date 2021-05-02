using System.Collections.Generic;

namespace SupremaMiddleware.Server.Models
{
    public class UserCollection
    {
        public string total { get; set; }
        public List<Row> rows { get; set; }
    }
}
