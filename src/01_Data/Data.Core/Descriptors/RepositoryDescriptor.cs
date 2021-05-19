using System;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Descriptors
{
    public class RepositoryDescriptor : IRepositoryDescriptor
    {
        public Type InterfaceType { get; }

        public Type ImplementType { get; }

        public RepositoryDescriptor(Type interfaceType, Type implementType)
        {
            InterfaceType = interfaceType;
            ImplementType = implementType;
        }
    }
}
