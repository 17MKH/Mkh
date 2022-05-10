using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Core;

internal class DefaultPlatformProvider : IPlatformProvider
{
    public string ToDescription(int platform)
    {
        switch (platform)
        {
            case 0:
                return "Web";
            default:
                return "Other";
        }
    }
}