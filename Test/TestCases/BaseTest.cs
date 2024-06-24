using NUnit.Framework.Interfaces;
using Practice.Core;
using Practice.Core.Configurations;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Core.ShareData;
using Practice.Core.Utilities;
using Practice.Services.Service;
using Practice.Test.Constants;
using Practice.Test.DataObject;

namespace Practice.Test;

public class BaseTest
{
    protected Dictionary<string, AccountDto> AccountData;
    protected  APIClient ApiClient;

    public BaseTest()
    {
        AccountData = JsonFileUtility.ReadAndParse<AccountDto>(FileConstant.AccountFilePath.GetAbsolutePath());
        ApiClient = new APIClient(ConfigurationManager.GetConfiguration()["application:url"]);
        ExtentTestManager.CreateParentTest(TestContext.CurrentContext.Test.ClassName);
    }

    [SetUp]
    public void SetUp()
    {
        ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);
        DataStorage.InitData();
    }
    [TearDown]
    public void AfterTest()
    {
        DataStorage.ClearData();
        ExtentTestManager.UpdateTestReport(); 
    }
    
}