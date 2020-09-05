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
    [RoutePrefix("api/chung-loai")]
    public class ChungLoaiApi_EmptyController : ApiController
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        #region Su dung truc tiep entity type de giao tiep voi client
        [Route("doc-tat-ca-1")]
        [HttpGet]
        [ResponseType(typeof(List<ChungLoai>))]
        public async Task<IHttpActionResult> DocTatCa1()
        {
            try
            {
                var items = await db.ChungLoais.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Loi khong truy cap duoc du lieu. Ly do : {ex.Message}" );
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
                var items = await db.ChungLoais
                    .Select(p=>new
                    {
                        p.ID,
                        p.MaSo,
                        p.Ten
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

        #region Su dung kieu dynamic | object de giao tiep voi client  --> Kieu du lieu xuat ra tuy y
        [Route("doc-tat-ca-bao-gom-hang-hoa")]
        [HttpGet]
        [ResponseType(typeof(List<dynamic>))]
       // [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> DocTatCaBaoGomHangHoa()
        {
            try
            {
                var items = await db.ChungLoais
                    .Select(p => new
                    {
                        p.ID,
                        p.MaSo,
                        p.Ten,
                        TongSoMatHang=p.HangHoas.Count(),
                        HangHoas =p.HangHoas.Select(h=>new {
                            h.ID,
                            h.MaSo,
                            h.Ten,
                            h.GiaBan
                        })
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

        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("doc-chi-tiet/{id}")]
        [HttpGet]      
        [ResponseType(typeof(ChungLoaiDTO))]
        public async Task<IHttpActionResult> DocChiTiet(int id) // <-- [FromUri]
        {
            try
            {
                var items = await db.ChungLoais
                    .Select(p => new
                    {
                        p.ID,
                        p.MaSo,
                        p.Ten
                    })
                    .SingleOrDefaultAsync(p=>p.ID ==id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest($"Loi khong truy cap duoc du lieu. Ly do : {ex.Message}");
            }
        }
        #endregion

        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("them-moi")]
        [HttpPost]
        [ResponseType(typeof(ChungLoaiDTO))]
        public async Task<IHttpActionResult> Them(ChungLoaiDTO input) // <-- [FromBody]
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                int d1 = await db.ChungLoais.CountAsync(p => p.MaSo == input.MaSo);
                if (d1 > 0) return BadRequest($"Ma so = '{input.MaSo}' da ton tai !");
                // KHoin tao mot ChungLoai moi (entity type - kieu du lieu giao tiep voi nguon du lieu)
                var entity = new ChungLoai();
                //Gan gia tri :
                entity.MaSo = input.MaSo;
                entity.Ten = input.Ten;
                db.ChungLoais.Add(entity);
                await db.SaveChangesAsync();
                input.ID = entity.ID;
                return Ok(input);
  
            }
            catch (Exception ex)
            {
                return BadRequest($"Them khong thanh cong. Ly do : {ex.Message}");
            }
        }
        #endregion


        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("hieu-chinh")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> HieuChinh(ChungLoaiDTO input) // <-- [FromBody]
        {
            try
            {
                // Tham chieu den thuc the thoa theo ID
                ChungLoai entity = await db.ChungLoais.FindAsync(input.ID);
                // Kiem tra du lieu
                if (entity==null) return BadRequest($"Chung loai  = {input.ID} khong ton tai");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                int d1 = await db.ChungLoais.CountAsync(p => p.ID!= input.ID && p.MaSo==input.MaSo);
                if (d1 > 0) return BadRequest($"Ma so = '{input.MaSo}' da ton tai !");
            
                //Data hop le, Gan gia tri :
                entity.MaSo = input.MaSo;
                entity.Ten = input.Ten;
                //db.ChungLoais.Add(entity);
                await db.SaveChangesAsync();
                input.ID = entity.ID;
                return Ok("Hieu chinh thanh cong");
                // Hoac c2 :
                //return StatusCode(HttpStatusCode.NoContent);
                //return 

            }
            catch (Exception ex)
            {
                return BadRequest($"Hieu chinh khong thanh cong. Ly do : {ex.Message}");
            }
        }
        #endregion

        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("xoa/{id}")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Xoa(int id) // <-- [FromBody]
        {
            try
            {
                // Tham chieu den thuc the thoa theo ID
                ChungLoai entity = await db.ChungLoais.FindAsync(id);
                // Kiem tra du lieu
                if (entity == null) return BadRequest($"Chung loai  = {id} khong ton tai");
                db.ChungLoais.Remove(entity);
                await db.SaveChangesAsync();
                return Ok($"Da xoa chung loai ID = {id} thanh cong"); // Status : 200
                // Hoac c2 :
                //return StatusCode(HttpStatusCode.NoContent);
                //return 

            }
            catch (Exception ex)
            {
                int d = await db.HangHoas.CountAsync(p => p.ChungLoaiID == id);
                if (d > 0) return BadRequest($"Khong xoa duoc vi da co {d} mat hang phu thuoc");
                return BadRequest($"Loi : {ex.Message}");
            }
        }
        #endregion
    }
}
