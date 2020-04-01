using System;
using System.Linq.Expressions;

namespace ReMap.Classes
{
	public class PropertyConfiguration<TSource,TResult>
	{
		public Expression<Func<TSource, object>> SourceExpression { get; set; }

		public Expression<Func<TResult, object>> TargetExpression { get; set; }

		public string SourcePropertyName { get; set; }

		public string TargetPropertyName { get; set; }

		public Func<object, object> MappingFunc { get; set; }
	}

}
