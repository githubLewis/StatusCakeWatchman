using StatusCakeWatchman.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Engine.Generation
{
    public class StatusCakeResponse
    {
        public bool Success { get; set; }
        public StatusCakeAlert Data { get; set; }
        public string Message { get; set; }
        public int InsertID { get; set; }
    }
}
