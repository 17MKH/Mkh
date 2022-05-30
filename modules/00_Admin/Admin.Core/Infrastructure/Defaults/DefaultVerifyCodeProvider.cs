using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions.Options;
using Mkh.Cache.Abstractions;
using Mkh.Utils.Annotations;
using Mkh.Utils.Helpers;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults;

/// <summary>
/// 默认验证码提供器
/// </summary>
[SingletonInject]
internal class DefaultVerifyCodeProvider : IVerifyCodeProvider
{

    //颜色列表，用于验证码、噪线、噪点 
    private readonly Color[] _colors = new[] { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };

    private readonly StringHelper _stringHelper;
    private readonly ICacheProvider _cacheHandler;
    private readonly IOptionsMonitor<AuthOptions> _authOptions;
    private readonly AdminCacheKeys _cacheKeys;

    public DefaultVerifyCodeProvider(StringHelper stringHelper, ICacheProvider cacheHandler, IOptionsMonitor<AuthOptions> authOptions, AdminCacheKeys cacheKeys)
    {
        _stringHelper = stringHelper;
        _cacheHandler = cacheHandler;
        _authOptions = authOptions;
        _cacheKeys = cacheKeys;
    }

    public async Task<VerifyCodeModel> Create()
    {
        var code = _stringHelper.GenerateRandomNumber();

        var bytes = DrawVerifyCode(code);

        var id = Guid.NewGuid().ToString();

        await _cacheHandler.Set(_cacheKeys.VerifyCode(id), code, 5);

        return new VerifyCodeModel
        {
            Id = id,
            Base64 = "data:image/png;base64," + Convert.ToBase64String(bytes)
        };
    }

    public async Task<IResultModel> Verify(string id, string code)
    {
        //启用验证码
        if (_authOptions.CurrentValue.EnableVerifyCode)
        {
            if (code.IsNull())
                return ResultModel.Failed("请输入验证码");

            if (id.IsNull())
                return ResultModel.Failed("验证码不存在");

            var cacheCode = await _cacheHandler.Get(_cacheKeys.VerifyCode(id));
            if (cacheCode.IsNull())
                return ResultModel.Failed("验证码不存在");

            if (!cacheCode.Equals(code))
                return ResultModel.Failed("验证码有误");
        }

        return ResultModel.Success();
    }

    /// <summary>
    /// 绘制验证码图片，返回图片的字节数组
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private byte[] DrawVerifyCode(string code)
    {
        using var img = new Image<Rgba32>(4 + 16 * code.Length, 40);
        var font = new Font(SystemFonts.Families.First(), 16, FontStyle.Regular);
        var codeStr = code;
        img.Mutate(x =>
        {
            x.BackgroundColor(Color.WhiteSmoke);

            var r = new Random();

            //画噪线 
            for (var i = 0; i < 4; i++)
            {
                int x1 = r.Next(img.Width);
                int y1 = r.Next(img.Height);
                int x2 = r.Next(img.Width);
                int y2 = r.Next(img.Height);
                x.DrawLines(new Pen(_colors.RandomGet(), 1L), new PointF(x1, y1), new PointF(x2, y2));
            }

            //画验证码字符串 
            for (int i = 0; i < codeStr.Length; i++)
            {
                x.DrawText(codeStr[i].ToString(), font, _colors.RandomGet(), new PointF((float)i * 16 + 4, 8));
            }
        });

        using var stream = new MemoryStream();
        img.SaveAsPng(stream);
        return stream.GetBuffer();
    }

}