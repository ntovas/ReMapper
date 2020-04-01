using System;
using System.Linq.Expressions;
using ReMap.Classes;
using ReMap.ExpressionHelpers;

namespace ReMap.Extensions
{
	public static class MapBuilderExtensions
	{
		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
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
			return new MapBuilderChainHelper<TSource, TResult>
			{
				Builder = builder,
				LastProperty = prop
			};
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
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
			return new MapBuilderChainHelper<TSource, TResult>
			{
				Builder = builder,
				LastProperty = prop
			};
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult,TSourceProp, TResultProp>(
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
			return new MapBuilderChainHelper<TSource, TResult>
			{
				Builder = builder,
				LastProperty = prop
			};
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
			this MapBuilder<TSource, TResult> builder,
			string source)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = source,
				TargetPropertyName = source
			};

			builder.AddProperty(prop);
			return new MapBuilderChainHelper<TSource, TResult>
			{
				Builder = builder,
				LastProperty = prop
			};
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
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
			return new MapBuilderChainHelper<TSource, TResult>
			{
				Builder = builder,
				LastProperty = prop
			};
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
			this MapBuilderChainHelper<TSource, TResult> builderHelper,
			Expression<Func<TSource, object>> source)
		{
			var property = ExpressionHelper.GetPropertyFromExpression(source);

			var propertyName = property.Name;

			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = source,
				TargetPropertyName = propertyName
			};

			builderHelper.Builder.AddProperty(prop);
			builderHelper.LastProperty = prop;
			return builderHelper;
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
			this MapBuilderChainHelper<TSource, TResult> builderHelper,
			Expression<Func<TSource, object>> source,
			Expression<Func<TResult, object>> target)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourceExpression = source,
				TargetExpression = target
			};

			builderHelper.Builder.AddProperty(prop);
			builderHelper.LastProperty = prop;
			return builderHelper;
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult, TSourceProp, TResultProp>(
			this MapBuilderChainHelper<TSource, TResult> builderHelper,
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

			builderHelper.Builder.AddProperty(prop);
			builderHelper.LastProperty = prop;
			return builderHelper;
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
			this MapBuilderChainHelper<TSource, TResult> builderHelper,
			string source)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = source,
				TargetPropertyName = source
			};

			builderHelper.Builder.AddProperty(prop);
			builderHelper.LastProperty = prop;
			return builderHelper;
		}

		public static MapBuilderChainHelper<TSource, TResult> Add<TSource, TResult>(
			this MapBuilderChainHelper<TSource, TResult> builderHelper,
			string source,
			string target)
		{
			var prop = new PropertyConfiguration<TSource, TResult>
			{
				SourcePropertyName = source,
				TargetPropertyName = target
			};

			builderHelper.Builder.AddProperty(prop);
			builderHelper.LastProperty = prop;
			return builderHelper;
		}


		public static ReMapper Build<TSource, TResult>(this MapBuilder<TSource, TResult> builder)
		{
			return builder.BuildMappedList();
		}

		public static ReMapper Build<TSource, TResult>(this MapBuilderChainHelper<TSource, TResult> builderHelper)
		{
			return builderHelper.Builder.BuildMappedList();
		}


		public static MapBuilderChainHelper<T, R> Test<T, R, Q,S>(this MapBuilderChainHelper<T, R> builder, Func<T,Q> func, Func<R,S> func2, Func<Q,S> func3)
		{
			return builder;
		}
	}
}
