using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApplication1.Utility.Release;
using Xunit;



namespace WebApplication1Tests
{
	public class TestCase
	{
		public string input;
		public string desiredValue;
	}
	public class TestAdapter
	{
		public Queue<TestCase> LoadTestFromFile(string fileName)
		{
			using StreamReader file = new StreamReader("LoginTestCases1.tc");
			string line = string.Empty;
			Queue<TestCase> testCases = new Queue<TestCase>();
			while ((line = file.ReadLine()) != null)
			{
				string[] parts = line.Split(';');
				testCases.Enqueue(new TestCase { input = parts[0], desiredValue = parts[1] });
			}
			return testCases;
		}
		public void Test(Queue<TestCase> testCases, Func<string, bool> testMethod)
		{
			int lineIndex = 0;
			while (testCases.Count > 0)
			{
				++lineIndex;
				TestCase testCase = testCases.Dequeue();
				Assert.True(testMethod(testCase.input) == Convert.ToBoolean(testCase.desiredValue), $"Error in case #{lineIndex}");
			}
		}
	}
	public class UtilityTests
	{
		public class ValidatorsTests
		{
			[Fact]
			public void LoginValidatorTest()
			{
				TestAdapter testAdapter = new TestAdapter();
				testAdapter.Test(testAdapter.LoadTestFromFile("LoginTestCases1.tc"), new LoginValidator().IsValid);
			}
			[Fact]
			public void PasswordValidatorTest()
			{
				TestAdapter testAdapter = new TestAdapter();
				testAdapter.Test(testAdapter.LoadTestFromFile("PasswordTestCases1.tc"), new LoginValidator().IsValid);
			}
		}

		public class HashersTests
		{
			[Fact]
			public void DefaultPasswordHasherTest()
			{

			}
		}
	}
}
