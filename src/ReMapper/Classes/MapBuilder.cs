using ReMap.ExpressionHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReMap.Classes
{
	public class MapBuilder<TSource, TResult>
	{
		private readonly ReMapper _reMapper;

		private readonly List<PropertyConfiguration<TSource, TResult>> _propertyList = new List<PropertyConfiguration<TSource, TResult>>();

		public MapBuilder(ReMapper reMapper)
		{
			_reMapper = reMapper;
		}

		internal void AddProperty(PropertyConfiguration<TSource, TResult> property)
		{
			_propertyList.Add(property);
		}

		internal ReMapper BuildMapping()
		{
			var mappedList = new List<MappedProperty<TSource, TResult>>();
			foreach (var property in _propertyList)
			{
				PropertyInfo sourceProperty = null;
				PropertyInfo targetProperty = null;

				if (property.SourceExpression != null)
				{
					sourceProperty = ExpressionHelper.GetPropertyFromExpression(property.SourceExpression);
				}
				else if (!string.IsNullOrEmpty(property.SourcePropertyName))
				{
					sourceProperty = typeof(TSource).GetProperty(property.SourcePropertyName);
				}

				if (sourceProperty == null && property.MappingFunc == null)
				{
					throw new ArgumentNullException(nameof(sourceProperty));
				}

				if (property.TargetExpression != null)
				{
					targetProperty = ExpressionHelper.GetPropertyFromExpression(property.TargetExpression);
				}
				else if (!string.IsNullOrEmpty(property.TargetPropertyName))
				{
					targetProperty = typeof(TResult).GetProperty(property.TargetPropertyName);
				}
				else
				{
					targetProperty = typeof(TResult).GetProperty(property.SourcePropertyName);
				}

				if (targetProperty == null)
				{
					continue;
				}

				if (property.MappingFunc == null && sourceProperty?.PropertyType != targetProperty.PropertyType)
				{
					var res = _reMapper.CanConvert(sourceProperty?.PropertyType, targetProperty.PropertyType);

					if (res)
					{
						property.MappingFunc = 
							_reMapper.GetConverterExpression(sourceProperty?.PropertyType, targetProperty.PropertyType);
					}
				}

				var mappedProperty = new MappedProperty<TSource,TResult>
				{
					SourceProperty = sourceProperty,
					TargetProperty = targetProperty,
					MappingFunc = property.MappingFunc
				};

				mappedList.Add(mappedProperty);
			}

			var converter = ExpressionHelper.BuildMapAction<TSource, TResult>(mappedList);

			_reMapper.AddToMappers((typeof(TSource), typeof(TResult)), converter);

			BuildGenerators<TSource>();
			BuildGenerators<TResult>();

			BuildConvertersForGenerics();

			return _reMapper;
		}

		internal MapBuilder<TResult, TSource> ReverseMapper()
		{
			return new MapBuilder<TResult, TSource>(_reMapper);
		}

		internal void BuildGenerators<T>()
		{
			_reMapper.AddToGenerators(typeof(T), ExpressionHelper.GetInstanceFunc<T>());
		}

		internal void BuildConvertersForGenerics()
		{
			var converter = _reMapper.GetConverter<TSource, TResult>();

			Expression<Func<List<TSource>, List<TResult>>> listExpression = list =>
				list.Select(c => converter(c)).ToList();

			Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> enumerableExpression = list =>
				list.Select(c => converter(c)).AsEnumerable();

			_reMapper.AddToConverters((typeof(List<TSource>), typeof(List<TResult>)), listExpression);
			
			_reMapper.AddToConverters((typeof(IEnumerable<TSource>), typeof(IEnumerable<TResult>)), enumerableExpression);
		}
	}
}
