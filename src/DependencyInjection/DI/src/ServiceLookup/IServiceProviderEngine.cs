// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 继承 IServiceProvider, 并且增加 获取 IServcieScope 的方法，该方法是返回一个Root的 Scoped IServiceProvider
    /// </summary>
    internal interface IServiceProviderEngine : IServiceProvider, IDisposable, IAsyncDisposable
    {
        IServiceScope RootScope { get; }
        void ValidateService(ServiceDescriptor descriptor);
    }
}
