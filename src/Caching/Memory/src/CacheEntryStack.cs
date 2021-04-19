// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Extensions.Caching.Memory
{
    /// <summary>
    /// CacheEntry Stack 是一个链表，每个元素包含了一个 CacheEntry 对象
    /// 在调用 Push 方法的时候，就会生成一个新的 CacheEntryStack, 该方法的返回一个 CacheEntryStack， 而且 Previous 属性就是调用
    /// 的 CacheEntryStack 对象
    /// </summary>
    internal class CacheEntryStack
    {
        private readonly CacheEntryStack _previous;
        private readonly CacheEntry _entry;

        private CacheEntryStack()
        {
        }

        private CacheEntryStack(CacheEntryStack previous, CacheEntry entry)
        {
            if (previous == null)
            {
                throw new ArgumentNullException(nameof(previous));
            }

            _previous = previous;
            _entry = entry;
        }

        public static CacheEntryStack Empty { get; } = new CacheEntryStack();

        public CacheEntryStack Push(CacheEntry c)
        {
            return new CacheEntryStack(this, c);
        }

        public CacheEntry Peek()
        {
            return _entry;
        }
    }
}
