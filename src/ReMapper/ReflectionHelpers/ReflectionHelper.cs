using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ReMap.ReflectionHelpers
{
	public static class ReflectionHelper
	{
		public static PropertyInfo GetPropertyByName(Type type, string name)
		{
			return type.GetProperty(name);
		}
	}
}
