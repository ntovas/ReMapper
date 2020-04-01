using System;
using System.Collections.Generic;
using System.Text;
using ReMap.AbstractClasses;

namespace ReMap.Classes
{
	public class MapBuilderChainHelper<TSource, TResult>
	{
		public MapBuilder<TSource, TResult> Builder { get; set; }

		public PropertyConfiguration<TSource,TResult> LastProperty { get; set; }
	}
}
