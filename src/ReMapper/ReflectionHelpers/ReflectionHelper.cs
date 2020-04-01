using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReMap.ReflectionHelpers
{
	public static class ReflectionHelper
	{
		public static PropertyInfo GetPropertyByName(Type type, string name)
		{
			return type.GetProperty(name);
		}

		public static List<PropertyInfo> GetAllProperties(Type type)
		{
			return type.GetProperties().ToList();
		}
	}
}
