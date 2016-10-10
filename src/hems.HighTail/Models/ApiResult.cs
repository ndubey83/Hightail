using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hems.HighTail.Models {
    public class ApiResult<T> : ApiResult {
        public T Response { get; set; }
    }

    public class ApiResult {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }

}
