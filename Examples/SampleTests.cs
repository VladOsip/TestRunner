using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestRunner.Core;
using TestRunner.Reporting;

namespace TestRunner.Examples
{
    // Example test class to demonstrate the runner
    public class SampleTests
    {
        private int _testValue;

        [Setup]
        public void Setup()
        {
            _testValue = 42;
            Console.WriteLine("Setup called");
        }

        [Teardown]
        public void Teardown()
        {
            Console.WriteLine("Teardown called");
        }

        [Test]
        public void TestThatPasses()
        {
            if (_testValue != 42)
                throw new Exception("Test value should be 42");
        }

        [Test]
        public void TestThatFails()
        {
            throw new Exception("This test always fails");
        }

        [Test]
        public void TestBasicMath()
        {
            var result = 2 + 2;
            if (result != 4)
                throw new Exception($"Expected 4, got {result}");
        }

        [Test]
        public void TestStringOperations()
        {
            var text = "Hello World";
            if (!text.Contains("World"))
                throw new Exception("String should contain 'World'");
        }

        // This method should not be executed as it lacks [Test] attribute
        public void NotATest()
        {
            Console.WriteLine("This should not run");
        }
    }   
}
