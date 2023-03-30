using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mkh.Utils.Annotations;
using Mkh.Utils.Encrypt;
using Mkh.Utils.File;

namespace Mkh.Utils.Web.FileUpload;

/// <summary>
/// 文件上传处理器
/// </summary>
[SingletonInject]
public class FileUploadProvider
{
    private readonly Md5Encrypt _md5Encrypt;

    public FileUploadProvider(Md5Encrypt md5Encrypt)
    {
        _md5Encrypt = md5Encrypt;
    }

    /// <summary>
    /// 上传
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IResultModel<FileDescriptor>> Upload(FileUploadModel model, CancellationToken cancellationToken = default)
    {
        Check.NotNull(model, nameof(model), "file upload model is null");

        Check.NotNull(model.RootDirectory, nameof(model.RootDirectory), "the file storage root directory is null");

        var result = new ResultModel<FileDescriptor>();

        if (model.FormFile == null)
            return result.Failed("请选择文件!");

        var size = model.FormFile.Length;

        //验证文件大小
        if (model.MaxSize > 0 && model.MaxSize < size)
            return result.Failed($"文件大小不能超过{new FileSize(model.MaxSize).ToString()}");

        var name = model.FileName.IsNull() ? model.FormFile.FileName : model.FileName;

        var descriptor = new FileDescriptor(name, size);

        //验证扩展名
        if (model.LimitExtensions != null && !model.LimitExtensions.Any(m => m.EqualsIgnoreCase(descriptor.Extension)))
            return result.Failed($"文件格式无效，请上传{model.LimitExtensions.Aggregate((x, y) => x + "," + y)}格式的文件");

        descriptor.RootDirectory = model.RootDirectory;

        //按照日期来保存文件
        var date = DateTime.Now;
        descriptor.RelativeDirectory = Path.Combine(date.ToString("yyyy"), date.ToString("MM"), date.ToString("dd"));

        //创建目录
        if (!Directory.Exists(descriptor.FullDirectory))
        {
            Directory.CreateDirectory(descriptor.FullDirectory);
        }

        //生成文件存储名称
        descriptor.StorageName = $"{Guid.NewGuid().ToString().Replace("-", "")}.{descriptor.Extension}";

        //写入
        await using var stream = new FileStream(descriptor.FullName, FileMode.Create);

        //计算MD5
        if (model.CalculateMd5)
        {
            descriptor.Md5 = _md5Encrypt.Encrypt(stream);
        }

        await model.FormFile.CopyToAsync(stream, cancellationToken);

        return result.Success(descriptor);
    }
}