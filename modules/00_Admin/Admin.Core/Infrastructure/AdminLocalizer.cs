using Microsoft.Extensions.Localization;
using Mkh.Module.Core;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// Admin 多语言
    /// </summary>
    [SingletonInject(true)]
    public class AdminLocalizer : ModuleLocalizerAbstract
    {
        public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
        {
        }
    }
}
