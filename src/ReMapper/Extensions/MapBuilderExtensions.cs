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
		public static MapBuilder<TSource, TResult> MapField<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, object>> from)
		{
			var property = ExpressionHelper.GetPropertyFromExpression(from);

			var propertyName = property.Name;

			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = from,
				TargetPropertyName = propertyName
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> MapField<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, object>> from,
			Expression<Func<TResult, object>> to)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = from,
				TargetExpression = to
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> MapField<TSource, TResult,TSourceProp, TResultProp>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TSource, TSourceProp>> from,
			Expression<Func<TResult, TResultProp>> to,
			Expression<Func<TSourceProp, TResultProp>> convert)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = from,
				TargetExpression = to,
				MappingFunc = convert
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> MapField<TSource, TResult, TResultProp>(
			this MapBuilder<TSource, TResult> builder,
			Expression<Func<TResult, TResultProp>> to,
			Expression<Func<TSource, TResultProp>> convert)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				TargetExpression = to,
				MappingFunc = convert
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> MapField<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string from)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = from,
				TargetPropertyName = from
			};

			builder.AddProperty(prop);
			return builder;
		}

		public static MapBuilder<TSource, TResult> MapField<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string from,
			string to)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = from,
				TargetPropertyName = to
			};

			builder.AddProperty(prop);
			return builder;
		}
		
		public static MapBuilder<TSource, TResult> MapAll<TSource, TResult>(
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
				builder.MapField(sourceProperty.Name);
			}

			return builder;
		}


		public static MapBuilder<TResult, TSource> Reverse<TSource, TResult>(this MapBuilder<TSource, TResult> builder)
		{
			builder.BuildMapping();

			return builder.ReverseMapper();
		}

		public static ReMapper Build<TSource, TResult>(this MapBuilder<TSource, TResult> builder)
		{
			return builder.BuildMapping();
		}

	}
}
