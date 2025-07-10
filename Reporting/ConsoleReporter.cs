using System;
using System.IO;
using TestRunner.Core;


namespace TestRunner.Reporting
{
    // Console reporter class
    public class ConsoleReporter
    {
        public void ReportResults(TestSummary summary)
        {
            Console.WriteLine("=== Test Results ===");
            Console.WriteLine();

            foreach (var result in summary.Results)
            {
                var status = result.Passed ? "PASS" : "FAIL";
                var statusColor = result.Passed ? ConsoleColor.Green : ConsoleColor.Red;

                Console.Write($"[");
                Console.ForegroundColor = statusColor;
                Console.Write(status);
                Console.ResetColor();
                Console.WriteLine($"] {result.TestName} ({result.Duration.TotalMilliseconds:F2}ms)");

                if (!result.Passed)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"    Error: {result.ErrorMessage}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Summary ===");
            Console.WriteLine($"Total Tests: {summary.TotalTests}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Passed: {summary.PassedTests}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed: {summary.FailedTests}");
            Console.ResetColor();
        }

        public void SaveResultsToFile(TestSummary summary)
        {
            // Create "Test Results" folder at project root
            var resultsDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Test Results");
            Directory.CreateDirectory(resultsDir); // Creates if not exists

            // Generate timestamped filename
            var timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            var filePath = Path.Combine(resultsDir, $"test_results_{timestamp}.txt");

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Test Results Report");
                writer.WriteLine($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
                writer.WriteLine();

                foreach (var result in summary.Results)
                {
                    writer.WriteLine($"{(result.Passed ? "PASS" : "FAIL")} - {result.TestName}");
                    if (!result.Passed)
                    {
                        writer.WriteLine($"    Error: {result.ErrorMessage}");
                    }
                    writer.WriteLine($"    Duration: {result.Duration.TotalMilliseconds:F2}ms");
                    writer.WriteLine();
                }

                writer.WriteLine("Summary:");
                writer.WriteLine($"Total: {summary.TotalTests}");
                writer.WriteLine($"Passed: {summary.PassedTests}");
                writer.WriteLine($"Failed: {summary.FailedTests}");
            }

            Console.WriteLine($"\nResults saved to: {Path.GetFullPath(filePath)}");
        }

    }
}