using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReMap
{
	public class ReMapper
	{
		private static Dictionary<(Type, Type), Delegate> _mappers;
		private static Dictionary<(Type, Type), Expression> _converters;
		private static Dictionary<Type, Delegate> _generators;

		private static bool _initialized = false;
		public ReMapper()
		{
			_mappers = new Dictionary<(Type, Type), Delegate>();
			_converters = new Dictionary<(Type, Type), Expression>();
			_generators = new Dictionary<Type, Delegate>();
		}

		internal void AddToMappers((Type, Type) types, Delegate mapper)
		{
			if (!_mappers.ContainsKey(types))
			{
				_mappers.Add(types, mapper);
			}
		}

		internal void AddToConverters((Type, Type) types, Expression converter)
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

		internal bool CanConvert(Type source, Type result)
		{
			var key = (source, result);

			return _mappers.ContainsKey(key) || _converters.ContainsKey(key);
		}

		internal Func<TSource, TResult> GetConverter<TSource, TResult>()
		{
			Func<TSource, TResult> func = source => Convert<TSource, TResult>(source);
			return func;
		}

		internal Expression GetConverterExpression(Type source, Type target)
		{
			var key = (source,target);
			if (!_converters.ContainsKey(key))
			{
				throw new KeyNotFoundException($"{key.Item1.Name}->{key.Item2.Name}");
			}

			return _converters[key];
		}

		public R Convert<T, R>(T source, R result)
		{
			var key = (typeof(T), typeof(R));

			if (!_mappers.ContainsKey(key))
			{
				throw new KeyNotFoundException($"{key.Item1.Name}->{key.Item2.Name}");
			}

			return ((Func<T, R, R>)_mappers[key]).Invoke(source, result);
		}

		public R Convert<T, R>(T source)
		{
			var key = (typeof(T), typeof(R));

			if (!_mappers.ContainsKey(key) && !_converters.ContainsKey(key))
			{
				throw new KeyNotFoundException($"{key.Item1.Name}->{key.Item2.Name}");
			}

			if (_converters.ContainsKey(key))
			{
				return ((Expression<Func<T, R>>)_converters[key]).Compile().Invoke(source);
			}

			var result = GetInstance<R>();

			return ((Func<T, R, R>)_mappers[key]).Invoke(source, result); ;
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
