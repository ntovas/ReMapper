using System;
using System.Linq;
using System.Linq.Expressions;
using ReMap.Classes;
using ReMap.ExpressionHelpers;
using ReMap.ReflectionHelpers;

namespace ReMap.Extensions
{
	public static class MapBuilderExtensions
	{
		public static MapBuilder<TSource, TResult> Add<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, object>> source)
		{
			var property = ExpressionHelper.GetPropertyFromExpression(source);

			var propertyName = property.Name;

			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = source,
				TargetPropertyName = propertyName
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> Add<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, object>> source,
			Expression<Func<TResult, object>> target)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = source,
				TargetExpression = target
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> Add<TSource, TResult,TSourceProp, TResultProp>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, TSourceProp>> source,
			Expression<Func<TResult, TResultProp>> target,
			Expression<Func<TSourceProp, TResultProp>> convert)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = source,
				TargetExpression = target,
				MappingFunc = convert
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> Add<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string source)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = source,
				TargetPropertyName = source
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> Add<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string source,
			string target)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = source,
				TargetPropertyName = target
			};

			builder.AddProperty(prop);
			return builder;
		}
		
		public static MapBuilder<TSource, TResult> AddAll<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string[] exclude = null)
		{
			var sourceProperties = ReflectionHelper.GetAllProperties(typeof(TSource));

			if (exclude != null && exclude.Any())
			{
				sourceProperties = sourceProperties.Where(c =>
					!exclude.Contains(c.Name)).ToList();
			}

			foreach (var sourceProperty in sourceProperties)
			{
				builder.Add(sourceProperty.Name);
			}

			return builder;
		}


		public static ReMapper Build<TSource, TResult>(this MapBuilder<TSource, TResult> builder)
		{
			return builder.BuildMapping();
		}

	}
}
