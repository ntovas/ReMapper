using System;
using System.Linq.Expressions;
using ReMap.AbstractClasses;

namespace ReMap.Classes
{
	public class PropertyConfiguration<TSource,TResult>
	{
		public Expression SourceExpression { get; set; }

		public Expression TargetExpression { get; set; }

		public string SourcePropertyName { get; set; }

		public string TargetPropertyName { get; set; }

		public Expression MappingFunc { get; set; }
	}

}
