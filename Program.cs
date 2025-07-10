using System;
using System.Reflection;
using TestRunner.Core;
using TestRunner.Reporting;

namespace TestRunner
{
    // Program entry point
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var runner = new CustomTestRunner();
                var reporter = new ConsoleReporter();

                // Get the current assembly (contains our sample tests)
                var assembly = Assembly.GetExecutingAssembly();

                Console.WriteLine("Running custom test runner...");
                Console.WriteLine();

                // Run tests
                var summary = runner.RunTests(assembly);

                // Report results to console
                reporter.ReportResults(summary);

                // Saving results to log file
                reporter.SaveResultsToFile(summary);

                // Exit with appropriate code
                Environment.Exit(summary.FailedTests > 0 ? 1 : 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running tests: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}