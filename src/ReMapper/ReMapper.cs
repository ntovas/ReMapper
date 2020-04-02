using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReMap
{
	public class ReMapper
	{
		private static Dictionary<(Type, Type), Delegate> _converters;
		private static Dictionary<Type, Delegate> _generators;

		private static bool _initialized = false;
		public ReMapper()
		{
			_converters = new Dictionary<(Type, Type), Delegate>();
			_generators = new Dictionary<Type, Delegate>();
		}

		internal void AddToConverters((Type, Type) types, Delegate converter)
		{
			if (!_converters.ContainsKey(types))
			{
				_converters.Add(types, converter);
			}
		}

		internal void AddToGenerators(Type type, Delegate func)
		{
			if (!_generators.ContainsKey(type))
			{
				_generators.Add(type, func);
			}
		}

		public void Convert<TSource, TResult>(TSource source, TResult result)
		{
			var key = (typeof(TSource), typeof(TResult));

			if (!_converters.ContainsKey(key))
			{
				throw new KeyNotFoundException($"{key.Item1.Name}->{key.Item2.Name}");
			}

			((Action<TSource, TResult>)_converters[key]).Invoke(source, result);

		}

		public TResult Convert<TSource, TResult>(TSource source)
		{
			var key = (typeof(TSource), typeof(TResult));

			if (!_converters.ContainsKey(key))
			{
				throw new KeyNotFoundException($"{key.Item1.Name}->{key.Item2.Name}");
			}

			var result = GetInstance<TResult>();

			Convert(source, result);

			return result;
		}


		private T GetInstance<T>()
		{
			var type = typeof(T);
			if (!_generators.ContainsKey(type))
			{
				throw new KeyNotFoundException(type.Name);
			}

			return ((Func<T>)_generators[type]).Invoke();
		}
	}
}
