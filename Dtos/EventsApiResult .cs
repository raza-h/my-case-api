
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Dtos
{
    public class EventsApiResult<T>
    {
        public string ErrorMessage { get; set; }

        public Exception Exception { get; set; }

        public string Path { get; set; }

        public string RequestId { get; set; }

        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }
    }
}
