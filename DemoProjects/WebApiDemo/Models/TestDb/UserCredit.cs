using System;
using System.Collections.Generic;

namespace WebApiDemo.Models.TestDb;

public partial class UserCredit
{
    public int Id { get; set; }

    public int HostId { get; set; }

    public int UserId { get; set; }

    public string? SmsName { get; set; }

    public string? SmsPassword { get; set; }

    public double Amount { get; set; }

    public bool Deleted { get; set; }

    public DateTime CreateTime { get; set; }

    public int CreateUser { get; set; }

    public DateTime? ModifyTime { get; set; }

    public int? ModifyUser { get; set; }
}
