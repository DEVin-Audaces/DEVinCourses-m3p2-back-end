using Xunit.Abstractions;
using Xunit.Sdk;

namespace DEVCoursesAPI.Tests;

public class AlphabeticalOrder: ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
        testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}