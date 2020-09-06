using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBanHangWebApi2.DTO
{
    public class PagedInput
    {
        public int PageSize { get; set; } // item trng 1 page
        public int PageIndex { get; set; } // Trang so
    }
    // Kieu dai dien cho tung kieu Data chuyen vao : HangHoa , ChungLoai ....
    public class PagedOutPut<T>
    {
        public List<T> Items { get; set; }
        public int TotalItemCout { get; set; }
    }
}