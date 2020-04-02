using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReMap;
using ReMap.Classes;
using ReMap.ExpressionHelpers;
using ReMap.Extensions;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var mapper = new ReMapper();
			mapper.AddBuilder<Test, Test2>()
			.MapField("Id")
			.MapField(c => c.Number,
				c => c.Number,
				c => c.ToString())
			.MapField("FirstName", "Name")
			.Reverse()
			
			.Build();

			mapper.AddBuilder<TestList, TestList2>()
			.MapField("Name")
			.MapField(to:c=> c.Count, convert:c=> c.List.Count)
			.MapField(c => c.List,
				c => c.List,
				 c => 
					c.Select(i=> 
						mapper.Convert<Test,Test2>(i)).ToList())
			.Reverse()
			.MapAll(new []{"List"})
			.MapField(to:c=> c.List, list2 => 
				list2.List.Select(c=> mapper.Convert<Test2, Test>(c)).ToList())
			.Build();

			var test = new Test
			{
				Id = "Test",
				Number = 2,
				FirstName = "Name"
			};

			var list = new TestList
			{
				Name = "Test",
				List = new List<Test> { test }
			};

			var test2 = new Test2();

			mapper.Convert(test, test2);

			var testlist2 = mapper.Convert<TestList, TestList2>(list);

			var testlist = mapper.Convert<TestList2, TestList>(testlist2);

			Console.ReadLine();
		}
	}

	public class Test
	{
		public string Id { get; set; }
		public int Number { get; set; }
		public string FirstName { get; set; }

	}

	public class Test2
	{
		public string Id { get; set; }
		public string Number { get; set; }
		public string Name { get; set; }
	}

	public class TestList
	{
		public string Name { get; set; }
		public List<Test> List { get; set; }
	}

	public class TestList2
	{
		public string Name { get; set; }
		public int Count { get; set; }
		public List<Test2> List { get; set; }
	}
}
