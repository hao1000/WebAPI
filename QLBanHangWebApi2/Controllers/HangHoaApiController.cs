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
                    .OrderByDescending(p=>p.ID)
                    .Include(p=>p.ChungLoai)
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
        #region Su dung kieu dynamic | object de giao tiep voi client 
        [Route("doc-theo-ten/{value}")]
        [HttpGet]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> DocTheoTen(string value)
        {
            try
            {
                var items = await db.HangHoas
                    .Where(p=>p.Ten.Contains(value))
                    .OrderByDescending(p => p.ID)
                    .Include(p => p.ChungLoai)
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

        #region Su dung kieu ChungLoaiDTO de giao tiep voi client
        [Route("doc-theo-ten/{id}")]
        [HttpGet]
        [ResponseType(typeof(HangHoaOutput))]
        public async Task<IHttpActionResult> DocChiTietThongTin(int id) // <-- [FromUri]
        {
            try
            {
                var items = await db.HangHoas
                    .Where(p => p.ID == id)
                    .Include(p => p.ChungLoai)
                    .Select(p => new HangHoaOutPut1
                    {
                       ID=p.ID,
                        MaSo = p.MaSo,
                        Ten = p.Ten,
                        GiaBan = p.GiaBan,
                        DonViTinh = p.DonViTinh,
                        MoTa = p.MoTa,
                        ThongSoKyThuat = p.ThongSoKyThuat,
                        NgayTao = p.NgayTao,
                        NgayCapNhat = p.NgayCapNhat,
                        TenHinh = p.TenHinh,
                        ChungLoaiID = p.ChungLoaiID,
                        ChungLoai = new ChungLoaiDTO
                        {
                            ID = p.ChungLoai.ID,
                            MaSo = p.ChungLoai.MaSo,
                            Ten = p.ChungLoai.Ten
                        }
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

        #region  Doc mot trang
        [Route("doc-mot-trang")]
        [HttpPost]
        [ResponseType(typeof(PagedOutPut<HangHoaOutput>))]
        public async Task<IHttpActionResult> DocMotTrang([FromBody]PagedInput input)
        {
            try
            {
                int n = (input.PageIndex - 1) * input.PageSize;
                int totalItems = await db.HangHoas.CountAsync();
                var hangHoaItems = await db.HangHoas
                    .OrderByDescending(p => p.ID)
                    .Skip(n)
                    .Take(input.PageSize)
                    .Include(p => p.ChungLoai)
                    .Select(p => new HangHoaOutput

                    {
                        hangHoaEntity = p,
                        chungLoaiEntity = p.ChungLoai
                    })
                    .ToListAsync();
                var onePageOfData = new PagedOutPut<HangHoaOutput>
                {
                    Items = hangHoaItems,
                    TotalItemCout = totalItems
                };

                return Ok(onePageOfData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Loi khong truy cap duoc du lieu. Ly do : {ex.Message}");
            }
        }
        #endregion

        #region  
        [Route("doc-theo-ten/{id}")]
        [HttpGet]
        [ResponseType(typeof(HangHoaOutput))]
        public async Task<IHttpActionResult> DocTheoID(int id) // <-- [FromUri]
        {
            try
            {
                var items = await db.HangHoas
                    .Where(p => p.ID == id)                
                    .Select(p => new HangHoaInput
                    {
                        ID = p.ID,
                        MaSo = p.MaSo,
                        Ten = p.Ten,
                        GiaBan = p.GiaBan,
                        DonViTinh = p.DonViTinh,
                        MoTa = p.MoTa,
                        ThongSoKyThuat = p.ThongSoKyThuat,
                        ChungLoaiID = p.ChungLoaiID.HasValue?p.ChungLoaiID.Value:0,
                       //NgayCapNhat =p.NgayCapNhat,
                       //NgayTao = p.NgayTao
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

        #region  
        [Route("them-moi")]
        [HttpPost]
        [ResponseType(typeof(HangHoaInput))]
        public async Task<IHttpActionResult> Them(HangHoaInput input) // <-- [FromBody]
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
               //int d1 = await db.HangHoas.CountAsync(p => p.MaSo == input.MaSo);
                int d1 = await db.HangHoas.CountAsync(p => p.MaSo.StartsWith(input.MaSo));
                if (d1> 0) return BadRequest($"Ma so = '{input.MaSo}' da ton tai");
                bool ktFK = await db.ChungLoais.AnyAsync(p => p.ID == input.ChungLoaiID);
                if (!ktFK) return BadRequest($"Chung loai ID  = '{input.ChungLoaiID}' khong to tai");
                // Khoi tao mot hang hoa moi  (entity type = kieu du lieu giao tiep voi nguon du lieu)
                var entity = new HangHoa();
                ConvertDTOToEntity(input, entity,true);
                // Them vao dataset va luu bao DB
                db.HangHoas.Add(entity);
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
        public async Task<IHttpActionResult> HieuChinh(HangHoaInput input) // <-- [FromBody mac dinh]
        {
            try
            {
                // Tham chieu den thuc the thoa theo ID
                HangHoa entity = await db.HangHoas.FindAsync(input.ID);
                // Kiem tra du lieu
                if (entity == null) return BadRequest($"Mat hang = {input.ID} khong ton tai");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                int d1 = await db.ChungLoais.CountAsync(p => p.ID != input.ID && p.MaSo == input.MaSo);
                if (d1 > 0) return BadRequest($"Ma so = '{input.MaSo}' da ton tai !");
                ConvertDTOToEntity(input, entity,false);

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
        [Route("xoa")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Xoa(int id) // <-- [FromBody mac dinh]
        {
            try
            {
                // Tham chieu den thuc the thoa theo ID
                var entity = await db.HangHoas.FindAsync(id);
        
                // Kiem tra du lieu
                if (entity == null) return BadRequest($"Mat hang = {id} khong ton tai");

                db.HangHoas.Remove(entity);
                await db.SaveChangesAsync();
                return Ok($"DA xoa thong tin thanh cong");

            }
            catch (Exception ex)
            {
                int d = await db.HoaDonChiTiets.CountAsync(p => p.HangHoaID == id);
                if (d>0)
                {
                    return BadRequest($"khong xoa duoc");
                }
                return BadRequest($"Hieu chinh khong thanh cong. Ly do : {ex.Message}");
            }
        }
        #endregion

        #region MyRegion
        private void ConvertDTOToEntity(HangHoaInput input,  HangHoa entity, bool them = true)
        {
            entity.MaSo = input.MaSo;
            entity.Ten = input.Ten;
            entity.ThongSoKyThuat = input.ThongSoKyThuat;
            entity.GiaBan = input.GiaBan;
            if (them)
            {
                entity.NgayTao = DateTime.Today;
                entity.NgayCapNhat = DateTime.Today;
            }
            entity.MoTa = input.MoTa;
       
        }
        #endregion
    }
}
