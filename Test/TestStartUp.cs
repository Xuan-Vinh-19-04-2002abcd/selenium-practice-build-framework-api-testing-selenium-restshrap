using Microsoft.Extensions.Configuration;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Test.Constants;
using ConfigurationManager = Practice.Core.Configurations.ConfigurationManager;

namespace Practice.Test;
[SetUpFixture]
public class TestStartUp
{
    [OneTimeSetUp]
    public void MySetup()
    { 
        ConfigurationManager.ReadConfiguration(FileConstant.SettingFilePath.GetAbsolutePath());
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("One Time Tear Down");
        ExtentReportManager.GenerateReport();
    }

}