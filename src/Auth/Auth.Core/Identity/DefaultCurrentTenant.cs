using Mkh.Identity.Abstractions;
using System;

namespace Mkh.Auth.Core.Identity;

internal class DefaultCurrentTenant : ICurrentTenant
{
    public Guid Id => Guid.Empty;

    public string Name => string.Empty;
}