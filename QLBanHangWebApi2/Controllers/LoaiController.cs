using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLBanHangWebApi2.Controllers
{
    public class LoaiController : ApiController
    {
        // GET: api/Loai
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Loai/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Loai
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Loai/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Loai/5
        public void Delete(int id)
        {
        }
    }
}
