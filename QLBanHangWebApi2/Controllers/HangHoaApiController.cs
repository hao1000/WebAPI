using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading.Tasks;
using System.Web.Http.Description;
using System.Data.Entity;
using QLBanHangWebApi2.DAL;
using QLBanHangWebApi2.DTO;


namespace QLBanHangWebApi2.Controllers
{
    // Dinh tuyen lai url
    [RoutePrefix("api/hang-hoa")]
    public class HangHoaApiController : ApiController
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("doc-chi-tiet/{id}")]
        [HttpGet]
        [ResponseType(typeof(HangHoaOutput))]
        public async Task<IHttpActionResult> DocChiTiet(int id) // <-- [FromUri]
        {
            try
            {
                var items = await db.HangHoas
                    .Where(p=>p.ID==id)
                    .Include(p=>p.ChungLoai)
                    .Select(p => new HangHoaOutput
                    {
                        hangHoaEntity =p,
                        chungLoaiEntity = p.ChungLoai
                    })
                    .SingleOrDefaultAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Loi khong truy cap duoc du lieu. Ly do : {ex.Message}");
            }
        }
        #endregion

        #region Su dung kieu dynamic | object de giao tiep voi client 
        [Route("doc-tat-ca")]
        [HttpGet]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> DocTatCa()
        {
            try
            {
                var items = await db.HangHoas
                    .Select(p => new HangHoaOutput
                    {
                        hangHoaEntity = p,
                        chungLoaiEntity = p.ChungLoai
                    })
                    .ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Loi khong truy cap duoc du lieu. Ly do : {ex.Message}");
            }
        }
        #endregion

    }
}
