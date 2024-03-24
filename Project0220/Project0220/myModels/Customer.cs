using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project0220.myModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    //下方設定日期: 年/月/日 沒有幾點幾分
    [DataType(DataType.Date)]
    //只能挑選填寫日期以前的日期 
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? MobilePhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? AddressCity { get; set; }

    public string? AddressDist { get; set; }

    public string? Address { get; set; }

    [Required(ErrorMessage = "帳號是必填的。")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "帳號長度必須大於5個字。")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "密碼是必填的。")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度必須大於6個字。")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool Admin { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public bool Subscribe { get; internal set; }
    
    public DateTime? ResetPasswordTokenExpiration { get; set; }
    public string ResetPasswordToken { get; set; }

}
