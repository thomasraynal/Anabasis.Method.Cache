﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public interface ICachingBackend
	{
		void SetValueAdapter<TAdapter>(TAdapter value) where TAdapter : IValueAdapter;
		Task Clear(CancellationToken cancellationToken = default);
		bool TryGetValue<TItem>(string key, out TItem value);
		Task<TItem> GetValue<TItem>(string key);
		Task<string[]> GetKeys();
		void SetValue<TItem>(string key, TItem value, 
			long absoluteExpirationRelativeToNowInMilliseconds = 0, 
			long slidingExpirationInMilliseconds = 0);
		Task Invalidate(string key);
		Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true);
		Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true);
		Task InvalidateWhenStartWith(string predicate, bool isCaseSensitive = true);
		Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true);
	}
}
