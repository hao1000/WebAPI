using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace QLBanHangWebApi2.DTO
{
    //Data Tranfer Object (Complex type)
    public class ChungLoaiDTO
    {
        // Giong class trong DAL
        public int ID { get; set; }

        [Display(Name = "Ma So")]
        [Required(ErrorMessage = "{0} khong duoc trong")]
        [MaxLength(10, ErrorMessage ="{(0) Phai duoi 10 ky tu}")]
        public string MaSo { get; set; }

        [Display(Name = "Ten")]
        [Required(ErrorMessage = "{0} khong duoc trong")]
        public string Ten { get; set; }
    }
}