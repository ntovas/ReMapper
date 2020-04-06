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

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var mapper = new ReMapper();

			mapper.AddBuilder<Test, Test2>()
			.MapField("Id")
			.MapField(from => from.Number,
				to => to.Number, 
				i => i.ToString())
			.MapField("FirstName", "Name")
			.Reverse()
			.MapField(nameof(Test2.Id))
			.MapField(c=> c.Name, to=> to.FirstName)
			.MapField(from=> from.Number,
				to=> to.Number,
				s => int.Parse(s))
			.Build();

			mapper.AddBuilder<TestList, TestList2>()
			.MapField("Name")
			.MapField(to=> to.Count, from=> from.List.Count)
			.MapField(from => from.List,
				to => to.List)
			
			.Reverse()
			.MapAll()
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

			var test3 = mapper.Convert(test, test2);

			var testlist2 = mapper.Convert<TestList, TestList2>(list);

			var testlist = mapper.Convert<TestList2, TestList>(testlist2);

			Console.ReadLine();
		}
	}
}
