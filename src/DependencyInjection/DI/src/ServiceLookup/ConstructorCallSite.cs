// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 构造函数形式
    /// </summary>
    internal class ConstructorCallSite : ServiceCallSite
    {
        internal ConstructorInfo ConstructorInfo { get; }
        internal ServiceCallSite[] ParameterCallSites { get; }

        /// <summary>
        /// 构造函数无参数
        /// </summary>
        public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo) : this(cache, serviceType, constructorInfo, Array.Empty<ServiceCallSite>())
        {
        }

        /// <summary>
        /// 构造函数和参数形式
        /// </summary>
        public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo, ServiceCallSite[] parameterCallSites) : base(cache)
        {
            if (!serviceType.IsAssignableFrom(constructorInfo.DeclaringType))
            {
                throw new ArgumentException(Resources.FormatImplementationTypeCantBeConvertedToServiceType(constructorInfo.DeclaringType, serviceType));
            }

            ServiceType = serviceType;
            ConstructorInfo = constructorInfo;
            ParameterCallSites = parameterCallSites;
        }

        public override Type ServiceType { get; }

        public override Type ImplementationType => ConstructorInfo.DeclaringType;
        public override CallSiteKind Kind { get; } = CallSiteKind.Constructor;
    }
}
