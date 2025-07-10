using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestRunner.Core
{
    /// <summary>
    /// The custom test runner that discovers and executes tests in an assembly.
    /// </summary>
    public class CustomTestRunner
    {
        /// <summary>
        /// Runs all the tests found in the provided assembly.
        /// </summary>
        public TestSummary RunTests(Assembly assembly)
        {
            var summary = new TestSummary();
            var testClasses = DiscoverTestClasses(assembly);

            foreach (var testClass in testClasses)
            {
                var results = RunTestsInClass(testClass);
                summary.Results.AddRange(results);
            }

            summary.TotalTests = summary.Results.Count;
            summary.PassedTests = summary.Results.Count(r => r.Passed);
            summary.FailedTests = summary.Results.Count(r => !r.Passed);

            return summary;
        }

        /// <summary>
        /// Discovers and returns a list of all test classes in the provided assembly.
        /// </summary>
        private List<Type> DiscoverTestClasses(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => type.GetMethods()
                    .Any(method => method.GetCustomAttribute<TestAttribute>() != null))
                .ToList();
        }

        /// <summary>
        /// finds the test, setup, and teardown methods in a given test class, and runs them.
        /// </summary>
        private List<TestResult> RunTestsInClass(Type testClass)
        {
            var results = new List<TestResult>();
            // Find all methods with [Test] attribute
            var testMethods = testClass.GetMethods()
                .Where(method => method.GetCustomAttribute<TestAttribute>() != null)
                .ToList();
            // Find method with [Setup] attribute
            var setupMethod = testClass.GetMethods()
                .FirstOrDefault(method => method.GetCustomAttribute<SetupAttribute>() != null);
            // Find method with [Teardown] attribute
            var teardownMethod = testClass.GetMethods()
                .FirstOrDefault(method => method.GetCustomAttribute<TeardownAttribute>() != null);

            // run each test method
            foreach (var testMethod in testMethods)
            {
                var result = RunSingleTest(testClass, testMethod, setupMethod, teardownMethod);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Runs a test method, including setup and teardown if they exist.
        /// </summary>
        private TestResult RunSingleTest(Type testClass, MethodInfo testMethod,
            MethodInfo setupMethod, MethodInfo teardownMethod)
        {
            var result = new TestResult
            {
                TestName = $"{testClass.Name}.{testMethod.Name}"
            };

            var startTime = DateTime.Now;
            object testInstance = null;

            try
            {
                // Create instance of test class
                testInstance = Activator.CreateInstance(testClass);

                // Run setup if exists
                setupMethod?.Invoke(testInstance, null);

                // Run the actual test
                testMethod.Invoke(testInstance, null);

                // If we get here, test passed
                result.Passed = true;
            }
            catch (Exception ex)
            {
                result.Passed = false;
                // Handle reflection exceptions
                var innerException = ex.InnerException ?? ex;
                result.ErrorMessage = innerException.Message;
            }
            finally
            {
                try
                {
                    // Run teardown if exists
                    teardownMethod?.Invoke(testInstance, null);
                }
                catch (Exception teardownEx)
                {
                    // Log teardown errors but don't fail the test
                    Console.WriteLine($"Teardown error in {result.TestName}: {teardownEx.Message}");
                }

                result.Duration = DateTime.Now - startTime;
            }

            return result;
        }
    }
}