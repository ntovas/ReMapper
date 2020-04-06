using ReMap.Classes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ReMap.ExpressionHelpers
{
	public static class ExpressionHelper
	{
		public static PropertyInfo GetPropertyFromExpression(Expression getPropertyLambda)
		{
			MemberExpression exp;
			if (getPropertyLambda is LambdaExpression exprFunc)
			{
				switch (exprFunc.Body)
				{
					case UnaryExpression unExp when unExp.Operand is MemberExpression operand:
						exp = operand;
						break;
					case UnaryExpression unExp:
						throw new ArgumentException();
					case MemberExpression body:
						exp = body;
						break;
					default:
						throw new ArgumentException();
				}

				return (PropertyInfo)exp.Member;
			}

			throw new ArgumentException();
		}

		public static Func<TSource, TTarget, TTarget> BuildMapAction<TSource, TTarget>(IEnumerable<MappedProperty<TSource, TTarget>> properties)
		{
			var source = Expression.Parameter(typeof(TSource), "source");
			var target = Expression.Parameter(typeof(TTarget), "target");

			var result = Expression.Variable(typeof(TTarget));
			
			var statements = new List<Expression>();

			foreach (var propertyInfo in properties)
			{
				
				var targetProperty = Expression.Property(target, propertyInfo.TargetProperty);
				
				Expression value;

				Expression statement;
				if (propertyInfo.SourceProperty == null && propertyInfo.MappingFunc != null)
				{
					var expr = Expression.Invoke(propertyInfo.MappingFunc, source);
					value = expr;

					statement = Expression.Assign(targetProperty, value);
				}
				else
				{
					var sourceProperty = Expression.Property(source, propertyInfo.SourceProperty);
					value = sourceProperty;

					if (propertyInfo.MappingFunc != null)
					{
						var expr = Expression.Invoke(propertyInfo.MappingFunc, value);
						value = expr;

						statement = Expression.Assign(targetProperty, value);
					}
					else
					{
						if (value.Type != targetProperty.Type)
							value = Expression.Convert(value, targetProperty.Type);
						statement = Expression.Assign(targetProperty, value);

						if (!sourceProperty.Type.IsValueType || Nullable.GetUnderlyingType(sourceProperty.Type) != null)
						{
							var valueNotNull = Expression.NotEqual(sourceProperty, Expression.Constant(null, sourceProperty.Type));
							statement = Expression.IfThen(valueNotNull, statement);
						}
					}
				}

				statements.Add(statement);
			}

			var body = statements.Count == 1 ? statements[0] : Expression.Block(statements);
			
			if (!source.Type.IsValueType)
			{
				var sourceNotNull = Expression.NotEqual(source, Expression.Constant(null, source.Type));
				body = Expression.IfThen(sourceNotNull, body);
			}

			var assign = Expression.Assign(result, target);
			body =  Expression.Block(new[] {result}, new List<Expression>()
			{
				body, assign
			});

			if (body.CanReduce)
				body = body.ReduceAndCheck();
			body = body.ReduceExtensions();

			var lambda = Expression.Lambda<Func<TSource, TTarget, TTarget>>(body, source, target);

			return lambda.Compile();
		}

		public static Func<T> GetInstanceFunc<T>()
		{
			return Expression.Lambda<Func<T>>(
				Expression.New(typeof(T))
			).Compile();
		}
	}
}
