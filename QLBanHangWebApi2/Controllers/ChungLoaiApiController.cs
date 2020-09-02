using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using QLBanHangWebApi2.Models;

namespace QLBanHangWebApi2.Controllers
{
    public class ChungLoaiApiController : ApiController
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        // GET: api/ChungLoaisaApi
        public IQueryable<ChungLoai> GetChungLoais()
        {
            return db.ChungLoais;
        }

        // GET: api/ChungLoaisaApi/5
        [ResponseType(typeof(ChungLoai))]
        public async Task<IHttpActionResult> GetChungLoai(int id)
        {
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            if (chungLoai == null)
            {
                return NotFound();
            }

            return Ok(chungLoai);
        }

        // PUT: api/ChungLoaisaApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChungLoai(int id, ChungLoai chungLoai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chungLoai.ID)
            {
                return BadRequest();
            }

            db.Entry(chungLoai).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChungLoaiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ChungLoaisaApi
        [ResponseType(typeof(ChungLoai))]
        public async Task<IHttpActionResult> PostChungLoai(ChungLoai chungLoai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ChungLoais.Add(chungLoai);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = chungLoai.ID }, chungLoai);
        }

        // DELETE: api/ChungLoaisaApi/5
        [ResponseType(typeof(ChungLoai))]
        public async Task<IHttpActionResult> DeleteChungLoai(int id)
        {
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            if (chungLoai == null)
            {
                return NotFound();
            }

            db.ChungLoais.Remove(chungLoai);
            await db.SaveChangesAsync();

            return Ok(chungLoai);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChungLoaiExists(int id)
        {
            return db.ChungLoais.Count(e => e.ID == id) > 0;
        }
    }
}