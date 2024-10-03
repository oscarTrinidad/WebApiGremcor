using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGremcor.Model
{
    public class ResponseModel
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public dynamic? data { get; set; }
    }
}
