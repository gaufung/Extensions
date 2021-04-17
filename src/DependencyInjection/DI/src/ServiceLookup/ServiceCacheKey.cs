// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 包含了两个属性
    /// - Type（用来记录实现的 service 的类型）
    /// - Slot (用来实现的序号，后实现的拥有最高的优先级）
    /// </summary>
    internal struct ServiceCacheKey: IEquatable<ServiceCacheKey>
    {
        public static ServiceCacheKey Empty { get; } = new ServiceCacheKey(null, 0);

        /// <summary>
        /// Type of service being cached
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Reverse index of the service when resolved in <code>IEnumerable&lt;Type&gt;</code> where default instance gets slot 0.
        /// For example for service collection
        ///  IService Impl1
        ///  IService Impl2
        ///  IService Impl3
        /// We would get the following cache keys:
        ///  Impl1 2
        ///  Impl2 1
        ///  Impl3 0
        /// </summary>
        public int Slot { get; }

        public ServiceCacheKey(Type type, int slot)
        {
            Type = type;
            Slot = slot;
        }

        public bool Equals(ServiceCacheKey other)
        {
            return Type == other.Type && Slot == other.Slot;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode() * 397) ^ Slot;
            }
        }
    }
}
