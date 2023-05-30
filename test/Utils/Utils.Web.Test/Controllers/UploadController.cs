using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mkh.Utils.Web.FileUpload;

namespace Utils.Web.Test.Controllers
{

    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly FileUploadProvider _provider;

        public UploadController(FileUploadProvider provider)
        {
            _provider = provider;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string name)
        {
            var uploadModel = new FileUploadModel
            {
                FormFile = file,
                FileName = name,
                RootDirectory = Path.Combine(AppContext.BaseDirectory, "Upload")
            };

            //文件大小不能超过10K
            //uploadModel.MaxSize = 10240;

            //限制文件扩展名
            //uploadModel.LimitExtensions = new List<string>
            //{
            //    FileExtensions.JPEG,
            //    FileExtensions.JPG
            //};

            //计算文件md5值
            //uploadModel.CalculateMd5 = true;

            var result = await _provider.Upload(uploadModel);

            return Ok(result);
        }
    }
}
