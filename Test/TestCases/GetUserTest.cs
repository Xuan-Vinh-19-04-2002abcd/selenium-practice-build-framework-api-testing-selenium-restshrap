using FluentAssertions;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Core.ShareData;
using Practice.Services.Helper;
using Practice.Services.Model.Request;
using Practice.Services.Model.Response;
using Practice.Services.Service;
using Practice.Test.Constants;
using Practice.Test.DataObject;

namespace Practice.Test.TestCases;

public class GetUserTest : BaseTest
{
    private UserService _userService;
    private BookService _bookService;

    public GetUserTest() : base()
    {
        _userService = new UserService(ApiClient);
        _bookService = new BookService(ApiClient);
    }

    [Test]
    [TestCase("user_1")]
    public async Task GetUserNotHaveBookSuccessAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Create token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        ReportLog.Info("2. Create userId");
        var idUser = account.UserId;
        var getUserRes = await _userService.GetUserSuccess(idUser, token);
        var result = getUserRes.Data;
        
        ReportLog.Info("3. Verify status code body and scheama");
        getUserRes.VerifyStatusCodeOk();
        await RestExtensions.VerrifySchema(getUserRes.Content, FileSchemaConstant.GetUserSchemaFilePath);
        result.UserId.Should().Be(idUser);
        result.Username.Should().Be(account.Username);  
    }
    [Test]
    [TestCase("user_1")]
    public async Task GetUserHaveBookSuccessAsyncTest(string accountKey)
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
        
      
        ReportLog.Info("3. Get user successfully");
        var idUser = account.UserId;
        var getUserRes = await _userService.GetUserSuccess(idUser, token);
        var result = getUserRes.Data;
        
        ReportLog.Info("4. Verify status code and schema");
        getUserRes.VerifyStatusCodeOk();
        await RestExtensions.VerrifySchema(getUserRes.Content, FileSchemaConstant.GetUserSchemaFilePath);
        
        result.UserId.Should().Be(idUser);
        result.Username.Should().Be(account.Username);

        
        ReportLog.Info("5. Verify the book in the user's collection matches the book in the collection store");
        List<BookingColectionDtoRes> duplicateBooks = _bookService.FindDuplicates(listBook, result.Books);
        foreach (var book in duplicateBooks)
        {
            var book1 = listBook.First(b => b.Isbn == book.Isbn);
            var book2 = result.Books.First(b => b.Isbn == book.Isbn);
            book1.Should().BeEquivalentTo(book2);
        }
        
    }
    [Test]
    [TestCase("user_1")]
    public async Task GetUserUnAuthorizationAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Create userId");
        AccountDto account = AccountData[accountKey];
        var getUserRes = await _userService.GetUserWithUnAuthorization(account.UserId);
        var result = getUserRes.Data;
        
        ReportLog.Info("2. Verify satus code and body");
        getUserRes.VerifyStatusCodeUnauthorized();
        result.Message.Should().Be(UserMessageConstant.Unauthorization);
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