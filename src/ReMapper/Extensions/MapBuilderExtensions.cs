using System;
using System.Linq.Expressions;
using ReMap.Classes;
using ReMap.ExpressionHelpers;

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

			builder.AddPropertyGetter(prop);
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

			builder.AddPropertyGetter(prop);
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

			builder.AddPropertyGetter(prop);
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

			builder.AddPropertyGetter(prop);
			return builder;
		}

		public static ReMapper Build<TSource, TResult>(this MapBuilder<TSource, TResult> builder)
		{
			return builder.BuildMappedList();
		}
	}
}
