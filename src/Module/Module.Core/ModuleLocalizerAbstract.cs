using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Mkh.Module.Abstractions;

namespace Mkh.Module.Core
{
    public abstract class ModuleLocalizerAbstract : IModuleLocalizer
    {
        private readonly IStringLocalizer _localizer;

        protected ModuleLocalizerAbstract(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        public LocalizedString this[string name] => _localizer[name];

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];
    }
}
