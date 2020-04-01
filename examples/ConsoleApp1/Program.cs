using System;
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

			var mapper = new ReMapper()
				.AddBuilder<Test, Test2>()
				.Add(t => t.Id,
					t => t.Id)
				.Add("Number")
				.Build();

			var test = new Test
			{
				Id = "Test",
				Number = 2
			};

			var test2 = new Test2();
			mapper.Convert(test, test2);

			Console.ReadLine();
		}
	}

	public class Test
	{
		public string Id { get; set; }
		public int Number { get; set; }
	}

	public class Test2
	{
		public string Id { get; set; }
		public int Number { get; set; }
	}
}
