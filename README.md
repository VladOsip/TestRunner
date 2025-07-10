# Custom C# Test Runner
## Overview ##
This is a simple test runner built from scratch in C#. It uses custom attributes to discover and execute test methods within the same assembly, and reports the results to both the console and a text file. The project doesn't use any existing testing frameworks.

## How It Works
### Attributes
Three custom attributes are defined in `Core/Attributes.cs`:
- `[Test]` – Marks a method as a test.
- `[Setup]` – Optional. Runs before each test method.
- `[Teardown]` – Optional. Runs after each test method.
### Test Discovery
The runner (`Core/TestRunner.cs`) uses reflection to scan the current assembly for any classes containing methods marked with `[Test]`.
### Test Execution
Each test is run in isolation:
- A new instance of the test class is created.
- `[Setup]` is called before each test (if present).
- The `[Test]` method is executed.
- `[Teardown]` is called afterward (if present).
- Exceptions are caught and reported per test.
- Execution time is measured and included in the report.
---
## How to Run
1. Compile and run the project (e.g., via `dotnet run`).
2. Test results will be printed to the console.
3. A detailed results file will be saved in the Test Results/ directory, with a timestamped filename.
---
## Design Decisions
- Reflection-based discovery keeps the runner simple and framework-independent.
- Only parameterless methods are supported for tests, setup, and teardown.
- Setup and teardown are limited to one method each per class for simplicity.
- Code is organized into logical namespaces (`Core`, `Reporting`, `Examples`) for clarity while maintaining simplicity.
- Output is written to both the console and a text file for basic CI/test log support.
---
## Example
Sample test class included in `Examples/SampleTests.cs`:
```csharp
[Test]
public void TestThatPasses() { /* Passes */ }
[Test]
public void TestThatFails() { throw new Exception("Fails intentionally"); }
[Setup]
public void Setup() { /* Called before each test */ }
[Teardown]
public void Teardown() { /* Called after each test */ }
```
