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
                       NgayCapNhat =p.NgayCapNhat,
                       NgayTao = p.NgayTao
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
    }
}
