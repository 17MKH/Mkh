using Mkh.Data.Abstractions.Query;

namespace Mkh.Mod.Admin.Core.Application.Account.Dto;

public class AccountQueryDto : QueryDto
{
    public string Username { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }
}