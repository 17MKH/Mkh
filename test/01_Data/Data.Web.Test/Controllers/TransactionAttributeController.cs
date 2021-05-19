using System.Threading.Tasks;
using Data.Common.Test.Service;
using Microsoft.AspNetCore.Mvc;

namespace Data.Web.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionAttributeController : ControllerBase
    {

        private readonly IArticleService _service;

        public TransactionAttributeController(IArticleService service)
        {
            _service = service;
        }

        public Task<bool> Index()
        {
            return _service.Add();
        }
    }
}
