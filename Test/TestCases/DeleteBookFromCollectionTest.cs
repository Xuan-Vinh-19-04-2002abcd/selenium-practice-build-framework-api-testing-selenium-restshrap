using FluentAssertions;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Services.Helper;
using Practice.Services.Model.Request;
using Practice.Services.Service;
using Practice.Test.Constants;
using Practice.Test.DataObject;

namespace Practice.Test.TestCases;

public class DeleteBookFromCollectionTest : BaseTest
{
    private BookService _bookService;
    private UserService _userService;

    public DeleteBookFromCollectionTest() : base()
    {
        _bookService = new BookService(ApiClient);
        _userService = new UserService(ApiClient);
    }
    [Test]
    [TestCase("user_1")]
    public async Task DeleteBookFromCollectionsSuccessAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Create token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        ReportLog.Info("2. Create isbn");
        var getAllBookRes = await _bookService.GetAllBookSuccess();
        var listBook = getAllBookRes.Data.Books;
        string isbn = DataProvider.IsbnProviderValid(listBook);
       
        BookDataHelper.StoreDataToDeleteBook(account.UserId,isbn,token);
        BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);
        
        ReportLog.Info("3. Delete book with no have token");
        var deleteBookRes =  _bookService.DeleteBookSuccess(account.UserId,isbn, token);
        
        ReportLog.Info("4. Verify status code and body");
        var result = deleteBookRes.Data;
        result.UserId.Should().Be(account.UserId);
        result.Isbn.Should().Be(isbn);
    }
    [Test]
    [TestCase("user_1")]
    public async Task DeleteBookFromCollectionsAuthorizationAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Create token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        var getAllBookRes = await _bookService.GetAllBookSuccess();
        var listBook = getAllBookRes.Data.Books;
        
        ReportLog.Info("2. Create isbn");
        string isbn = DataProvider.IsbnProviderValid(listBook);
        
        await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);
        
        ReportLog.Info("3. Delete book with no have token");
        var deleteBookRes =  _bookService.DeleteBookAuthorization(account.UserId,isbn);
        
        ReportLog.Info("4. Verify status code and body");
        var result = deleteBookRes.Data;
        deleteBookRes.VerifyStatusCodeUnauthorized();
        result.Code.Should().Be(1200);
        result.Message.Should().Be(UserMessageConstant.Unauthorization);
    }
    [Test]
    [TestCase("user_1")]
    public async Task DeleteBookFromCollectionsBadRequestAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Create token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        ReportLog.Info("2. Create isbn");
        var isbn = DataProvider.IsbnProviderInValid();
        
        ReportLog.Info("3. Delete book with isbn invalid");
        var deleteBookRes =  _bookService.DeleteBookBadRequest(account.UserId,isbn,token);
        
        ReportLog.Info("4. Verify status code and body");
        var result = deleteBookRes.Data;
        deleteBookRes.VerifyStatusCodeBadRequest();
        result.Code.Should().Be(1206);
        result.Message.Should().Be(UserMessageConstant.DeleteOfBadRequest);
    }
}