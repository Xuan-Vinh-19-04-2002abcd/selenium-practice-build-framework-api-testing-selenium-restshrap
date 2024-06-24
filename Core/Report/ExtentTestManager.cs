using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;

namespace Practice.Core.Report;

public class ExtentTestManager
{
    private static AsyncLocal<ExtentTest> _parentTest = new AsyncLocal<ExtentTest>();
    private static AsyncLocal<ExtentTest> _childTest = new AsyncLocal<ExtentTest>();

    public static ExtentTest CreateParentTest(string testName, string description = null)
    {
        _parentTest.Value = ExtentReportManager.Instance.CreateTest(testName, description);
        return _parentTest.Value;
    }

    public static ExtentTest CreateTest(string testName, string description = null)
    {
        if (_parentTest.Value == null)
        {
            throw new InvalidOperationException("Parent test is not set. Ensure CreateParentTest is called before CreateTest.");
        }
        _childTest.Value = _parentTest.Value.CreateNode(testName, description);
        return _childTest.Value;
    }

    public static ExtentTest GetTest()
    {
        if (_childTest.Value == null)
        {
            throw new InvalidOperationException("Child test is not set. Ensure CreateTest is called before GetTest.");
        }
        return _childTest.Value;
    }
    public static void UpdateTestReport()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : TestContext.CurrentContext.Result.StackTrace;
        var message = TestContext.CurrentContext.Result.Message;

        switch (status)
        {
            case TestStatus.Failed:
                ReportLog.Fail($"Test failed with message: {message}");
                ReportLog.Fail($"Stacktrace: {stacktrace}");
                break;
            case TestStatus.Inconclusive:
                ReportLog.Skip($"Test inconclusive with message: {message}");
                ReportLog.Skip($"Stacktrace: {stacktrace}");
                break;
            case TestStatus.Skipped:
                ReportLog.Skip($"Test skipped with message: {message}");
                break;
            default:
                ReportLog.Pass("Test passed");
                break;
        }
    }

}