// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 使用一个字典，用来保存每个服务和它对应的实现类型，同时还记录实现类型的 order
    /// </summary>
    internal class CallSiteChain
    {
        private readonly Dictionary<Type,ChainItemInfo> _callSiteChain;

        public CallSiteChain()
        {
            _callSiteChain = new Dictionary<Type, ChainItemInfo>();
        }

        public void CheckCircularDependency(Type serviceType)
        {
            if (_callSiteChain.ContainsKey(serviceType))
            {
                throw new InvalidOperationException(CreateCircularDependencyExceptionMessage(serviceType));
            }
        }

        public void Remove(Type serviceType)
        {
            _callSiteChain.Remove(serviceType);
        }

        public void Add(Type serviceType, Type implementationType = null)
        {
            _callSiteChain[serviceType] = new ChainItemInfo(_callSiteChain.Count, implementationType);
        }

        private string CreateCircularDependencyExceptionMessage(Type type)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat(Resources.CircularDependencyException, TypeNameHelper.GetTypeDisplayName(type));
            messageBuilder.AppendLine();

            AppendResolutionPath(messageBuilder, type);

            return messageBuilder.ToString();
        }

        private void AppendResolutionPath(StringBuilder builder, Type currentlyResolving = null)
        {
            foreach (var pair in _callSiteChain.OrderBy(p => p.Value.Order))
            {
                var serviceType = pair.Key;
                var implementationType = pair.Value.ImplementationType;
                if (implementationType == null || serviceType == implementationType)
                {
                    builder.Append(TypeNameHelper.GetTypeDisplayName(serviceType));
                }
                else
                {
                    builder.AppendFormat("{0}({1})",
                        TypeNameHelper.GetTypeDisplayName(serviceType),
                        TypeNameHelper.GetTypeDisplayName(implementationType));
                }

                builder.Append(" -> ");
            }

            builder.Append(TypeNameHelper.GetTypeDisplayName(currentlyResolving));
        }

        private struct ChainItemInfo
        {
            public int Order { get; }
            public Type ImplementationType { get; }

            public ChainItemInfo(int order, Type implementationType)
            {
                Order = order;
                ImplementationType = implementationType;
            }
        }
    }
}
