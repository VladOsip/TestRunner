using System;
using System.Collections.Generic;

namespace TestRunner.Core
{
    /// <summary>
    /// A class representing the result of a single test.
    /// </summary>
    public class TestResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    /// A summary of all test results. 
    /// </summary>
    public class TestSummary
    {
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }
        public int FailedTests { get; set; }
        public List<TestResult> Results { get; set; } = new List<TestResult>();
    }
}