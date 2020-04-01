using System;
using System.Reflection;

namespace ReMap.Classes
{
	public class MappedProperty
	{
		public PropertyInfo SourceProperty { get; set; }

		public PropertyInfo TargetProperty { get; set; }

		public Func<object, object> MappingFunc { get; set; }
	}
}
