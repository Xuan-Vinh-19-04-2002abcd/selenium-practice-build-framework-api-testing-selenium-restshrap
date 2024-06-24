using FluentAssertions;
using Practice.Core;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Core.ShareData;
using Practice.Services.Helper;
using Practice.Services.Model.Request;
using Practice.Services.Service;
using Practice.Test.Constants;
using Practice.Test.DataObject;

namespace Practice.Test.TestCases;

public class AddBookToCollectionTest : BaseTest
{
    private BookService _bookService;
    private UserService _userService;

    public AddBookToCollectionTest() : base()
    {
        _bookService = new BookService(ApiClient);
        _userService = new UserService(ApiClient);
    }

    [Test]
    [TestCase("user_1")]
    public async Task AddBookToCollectionSuccessAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        ReportLog.Info("2. Get listbook ");
        var getAllBookRes = await _bookService.GetAllBookSuccess();
        var listBook = getAllBookRes.Data.Books;
        string isbn = DataProvider.IsbnProviderValid(listBook);
        ReportLog.Info("3. Save Data  ");
        BookDataHelper.StoreDataToDeleteBook(account.UserId,isbn,token);
        BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        ReportLog.Info("4. Add book  ");
       var addBookRes =  await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);
       var result = addBookRes.Data;
       ReportLog.Info("5. Verify status and body");
       addBookRes.VerifyStatusCodeCreated();
       foreach (var book in result.Books)
       {
           book.Isbn.Should().Be(isbn);
           break;
       }
    }
    [Test]
    [TestCase("user_1")]
    public async Task AddBookToCollectionUnauthorizedAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get all book");
        AccountDto account = AccountData[accountKey];
        var getAllBookRes = await _bookService.GetAllBookSuccess();
        var listBook = getAllBookRes.Data.Books;
        
        ReportLog.Info("2. Create isbn");
        string isbn = DataProvider.IsbnProviderValid(listBook);
        
        ReportLog.Info("3. Add book to collection but not have token");
        var addBookRes =  await _bookService.AddBookToCollectionUnauthorized(account.UserId,isbn);
        var result = addBookRes.Data;
        
        ReportLog.Info("4. Verify status and body");
        addBookRes.VerifyStatusCodeUnauthorized();
        result.Code.Should().Be(1200);
        result.Message.Should().Be(UserMessageConstant.Unauthorization);
    }
    [Test]
    [TestCase("user_1")]
    public async Task AddBookToCollectionBadRequestAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        ReportLog.Info("2. Create isbn");
        var isbn = DataProvider.IsbnProviderInValid();
        
        ReportLog.Info("3. Add book with isbn invalid");
        var addBookRes =  await _bookService.AddBookToCollectionBadRequest(account.UserId,isbn,token);
        
        ReportLog.Info("4. Verify status code and body");
        var result = addBookRes.Data;
        addBookRes.VerifyStatusCodeBadRequest();
        result.Message.Should().Be(UserMessageConstant.BadRequest);
    }

    [TearDown]
    public void TearDown()
    {
        if (_bookService != null && DataStorage.GetData("hasCreatedBook") is bool hasCreatedBook && hasCreatedBook)
        {
            BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        }
    }
    
}   