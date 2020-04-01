﻿using ReMap.Classes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ReMap.ExpressionHelpers
{
	public static class ExpressionHelper
	{
		public static PropertyInfo GetPropertyFromExpression<T>(Expression<Func<T, object>> getPropertyLambda)
		{
			MemberExpression exp;

			switch (getPropertyLambda.Body)
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

		public static Action<TSource, TTarget> BuildMapAction<TSource, TTarget>(IEnumerable<MappedProperty> properties)
		{
			var source = Expression.Parameter(typeof(TSource), "source");
			var target = Expression.Parameter(typeof(TTarget), "target");

			var statements = new List<Expression>();
			foreach (var propertyInfo in properties)
			{
				var sourceProperty = Expression.Property(source, propertyInfo.SourceProperty);
				var targetProperty = Expression.Property(target, propertyInfo.TargetProperty);
				Expression value = sourceProperty;

				if (value.Type != targetProperty.Type)
					value = Expression.Convert(value, targetProperty.Type);
				Expression statement = Expression.Assign(targetProperty, value);
				
				if (!sourceProperty.Type.IsValueType || Nullable.GetUnderlyingType(sourceProperty.Type) != null)
				{
					var valueNotNull = Expression.NotEqual(sourceProperty, Expression.Constant(null, sourceProperty.Type));
					statement = Expression.IfThen(valueNotNull, statement);
				}
				statements.Add(statement);
			}

			var body = statements.Count == 1 ? statements[0] : Expression.Block(statements);
			if (!source.Type.IsValueType)
			{
				var sourceNotNull = Expression.NotEqual(source, Expression.Constant(null, source.Type));
				body = Expression.IfThen(sourceNotNull, body);
			}

			if (body.CanReduce)
				body = body.ReduceAndCheck();
			body = body.ReduceExtensions();

			var lambda = Expression.Lambda<Action<TSource, TTarget>>(body, source, target);

			return lambda.Compile();
		}
    }
}
