using System;
using System.Collections.Generic;
using ReMap.AbstractClasses;

namespace ReMap
{
	public class ReMapper
	{
		private static Dictionary<(Type, Type), MapBuilder> _converters;

		public ReMapper()
		{
			_converters = new Dictionary<(Type, Type), MapBuilder>();
		}

		internal void AddToConverters((Type, Type) types, MapBuilder mapBuilder)
		{
			_converters.Add(types, mapBuilder);
		}

		public void Convert<TSource, TResult>(TSource source, TResult result)
		{
			var key = (typeof(TSource), typeof(TResult));

			if (!_converters.ContainsKey(key)) throw new ArgumentException();

			var converter = _converters[key];

			converter.Convert(source, result);
		}
	}
}
