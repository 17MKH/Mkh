using Mkh.Data.Core.Repository;
using Mkh.Mod.Admin.Core.Domain.Roles;
using Role = Mkh.Mod.Admin.Core.Domain.Roles.Role;

namespace Mkh.Mod.Admin.Core.Infrastructure.Repositories;

public class RoleRepository : RepositoryAbstract<Role>, IRoleRepository
{
}