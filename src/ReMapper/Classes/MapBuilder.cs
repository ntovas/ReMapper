using ReMap.ExpressionHelpers;
using System;
using System.Collections.Generic;
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

				if (sourceProperty == null)
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

				var mappedProperty = new MappedProperty<TSource,TResult>
				{
					SourceProperty = sourceProperty,
					TargetProperty = targetProperty,
					MappingFunc = property.MappingFunc
				};

				mappedList.Add(mappedProperty);
			}

			var converter = ExpressionHelper.BuildMapAction<TSource, TResult>(mappedList);

			_reMapper.AddToConverters((typeof(TSource), typeof(TResult)), converter);

			BuildGenerators<TSource>();
			BuildGenerators<TResult>();

			return _reMapper;
		}

		internal void BuildGenerators<T>()
		{
			var func = ExpressionHelper.GetInstanceFunc<T>();
			_reMapper.AddToGenerators(typeof(T), func);
		}


	}
}
