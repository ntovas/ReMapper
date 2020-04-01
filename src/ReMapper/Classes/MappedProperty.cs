using System.Linq.Expressions;
using System.Reflection;

namespace ReMap.Classes
{
	public class MappedProperty<TSourceProperty, TTargetProperty> 
	{
		public PropertyInfo SourceProperty { get; set; }

		public PropertyInfo TargetProperty { get; set; }

		public Expression MappingFunc{ get; set; }
	}
}
