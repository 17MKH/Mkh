using System;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Menu
{
    public class MenuService : IMenuService
    {
        public Task<IResultModel> Query()
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Add(MenuAddDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Edit(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Update(MenuUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
