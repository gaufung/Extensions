// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 同样包含两个属性
    /// - LifeTime 对应于 Singlton, Scoped 和 Transient
    /// - SeriviceCacheKey: 表明这是实现的这个服务的类型
    /// </summary>
    internal struct ResultCache
    {
        public static ResultCache None { get; } = new ResultCache(CallSiteResultCacheLocation.None, ServiceCacheKey.Empty);

        internal ResultCache(CallSiteResultCacheLocation lifetime, ServiceCacheKey cacheKey)
        {
            Location = lifetime;
            Key = cacheKey;
        }

        public ResultCache(ServiceLifetime lifetime, Type type, int slot)
        {
            Debug.Assert(lifetime == ServiceLifetime.Transient || type != null);

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    Location = CallSiteResultCacheLocation.Root;
                    break;
                case ServiceLifetime.Scoped:
                    Location = CallSiteResultCacheLocation.Scope;
                    break;
                case ServiceLifetime.Transient:
                    Location = CallSiteResultCacheLocation.Dispose;
                    break;
                default:
                    Location = CallSiteResultCacheLocation.None;
                    break;
            }
            Key = new ServiceCacheKey(type, slot);
        }

        public CallSiteResultCacheLocation Location { get; set; }

        public ServiceCacheKey Key { get; set; }
    }
}