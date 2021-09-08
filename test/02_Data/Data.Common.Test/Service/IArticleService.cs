using System.Threading.Tasks;

namespace Data.Common.Test.Service
{
    public interface IArticleService
    {
        Task<bool> Add();
    }
}
