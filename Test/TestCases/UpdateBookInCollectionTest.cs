using System.Xml.Schema;
using FluentAssertions;
using Practice.Core.Extensions;
using Practice.Core.Report;
using Practice.Core.ShareData;
using Practice.Services.Helper;
using Practice.Services.Model.Request;
using Practice.Services.Service;
using Practice.Test.Constants;
using Practice.Test.DataObject;

namespace Practice.Test.TestCases;

public class UpdateBookInCollectionTest : BaseTest
{
    private BookService _bookService;
    private UserService _userService;

    public UpdateBookInCollectionTest() : base()
    {
        _bookService = new BookService(ApiClient);
        _userService = new UserService(ApiClient);
    }
    
    [Test]
    [TestCase("user_1")]
    public async Task UpdateBookFromCollectionSuccessAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get Token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        var responseGetAllBooks = await _bookService.GetAllBookSuccess();
        var listBook = responseGetAllBooks.Data.Books;
        
        ReportLog.Info("2. Create isbnUpdate and isbn");
        string isbn = DataProvider.IsbnProviderValid(listBook);
        string isbnUpdate = DataProvider.IsbnProviderValid(listBook, isbn);
       
        BookDataHelper.StoreDataToDeleteBook(account.UserId,isbn,token);
        BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        
        ReportLog.Info("3. Add book into collection");
        await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);
        
        ReportLog.Info("4. Update book");
        var response =  await _bookService.UpdateBookSuccess(account.UserId,isbnUpdate, isbn,token);

        BookDataHelper.StoreDataToDeleteBook(account.UserId, isbnUpdate, token);
        
        ReportLog.Info("5. Verify status code body and schema");
        var result = response.Data;
        response.VerifyStatusCodeOk();
        
        await RestExtensions.VerrifySchema(response.Content, FileSchemaConstant.UpdateBookInCollectionSchemaFilePath);
        result.UserId.Should().Be(account.UserId);
        result.Username.Should().Be(account.Username);
        
        var responseGetDetailBook = await _bookService.GetDetailBookSuccess(isbnUpdate);
        var bookUpdated = responseGetDetailBook.Data;
        foreach (var book in result.Books)
        {
            book.Should().BeEquivalentTo(bookUpdated);
            break;
        }

    }
    [Test]
    [TestCase("user_1")]
    public async Task UpdateBookFromCollectionUnauthorizetionAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get Token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        var responseGetAllBooks = await _bookService.GetAllBookSuccess();
        var listBook = responseGetAllBooks.Data.Books;
        
        ReportLog.Info("2. Create isbnUpdate and isbn");
        string isbn = DataProvider.IsbnProviderValid(listBook);
        string isbnUpdate = DataProvider.IsbnProviderValid(listBook, isbn);
      
        BookDataHelper.StoreDataToDeleteBook(account.UserId,isbn,token);
        BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        
        ReportLog.Info("3. Add book into book collection");
        await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);
        
        
        ReportLog.Info("4. Update book");
        var response =  await _bookService.UpdateBookAuthorization(account.UserId,isbnUpdate, isbn);
        
        ReportLog.Info("5. Verify status code, body");
        var result = response.Data;
        response.VerifyStatusCodeUnauthorized();
        result.Code.Should().Be(1200);
        result.Message.Should().Be(UserMessageConstant.Unauthorization);
        
    }
    [Test]
    [TestCase("user_1")]
    public async Task UpdateBookFromCollectionBadRequestAsyncTest(string accountKey)
    {
        ReportLog.Info("1. Get Token");
        AccountDto account = AccountData[accountKey];
        await _userService.StoreTokenAsync(accountKey, account);
        var token = _userService.GetToken(accountKey);
        
        var responseGetAllBooks = await _bookService.GetAllBookSuccess();
        var listBook = responseGetAllBooks.Data.Books;
        
        ReportLog.Info("2. Create isbnUpdate invalid and isbn");
        string isbn = DataProvider.IsbnProviderValid(listBook);
        string isbnUpdate = DataProvider.IsbnProviderInValid();
        
        
        BookDataHelper.StoreDataToDeleteBook(account.UserId,isbn,token);
        BookDataHelper.DeleteCreateBookFromStorage(_bookService);
        
        ReportLog.Info("3. Add book into book collection");
        await _bookService.AddBookToCollectionSuccess(account.UserId,isbn, token);

        ReportLog.Info("4. Update book");
        var response =  await _bookService.UpdateBookBadRequest(account.UserId,isbnUpdate,isbn, token);
        
        ReportLog.Info("5. Verify status code, body");
        var result = response.Data;
        response.VerifyStatusCodeBadRequest();
        result.Code.Should().Be(1205);
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