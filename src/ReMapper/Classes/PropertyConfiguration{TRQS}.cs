using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ReMap.AbstractClasses;

namespace ReMap.Classes
{
	public class PropertyConfiguration<TSource, TResult,TSourceProp,TResultProp>
	{
		public Expression<Func<TSource, TSourceProp>> SourceExpression { get; set; }

		public Expression<Func<TResult, TResultProp>> TargetExpression { get; set; }

		public string SourcePropertyName { get; set; }

		public string TargetPropertyName { get; set; }

		public Expression<Func<TSourceProp, TResultProp>> MappingFunc { get; set; }
	}
}
