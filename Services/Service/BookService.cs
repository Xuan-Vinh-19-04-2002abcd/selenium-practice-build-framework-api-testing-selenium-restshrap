using System.Net;
using Practice.Core;
using Practice.Services.Model.Request;
using Practice.Services.Model.Response;
using Practice.Test.Constants;
using Practice.Test.DataObject;
using Practice.Test.TestCases;
using RestSharp;

namespace Practice.Services.Service;

public class BookService
{
    private readonly APIClient _client;

    public BookService(APIClient apiClient)
    {
        _client = apiClient;
    }
    public async Task<RestResponse<AddBookToCollectionDt0Res>> AddBookToCollectionSuccess(AddBookToCollectionDtoReq bookDtoReq,string token)
    {
        var response = await _client.CreateRequest(EndPointConstant.AddBookToCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(bookDtoReq)
            .ExecutePostAsync<AddBookToCollectionDt0Res>();
        return response;
    }
    public async Task<RestResponse<UnAuthorizationDtoRes>> AddBookToCollectionUnauthorized(AddBookToCollectionDtoReq bookDtoReq)
    {
        var response = await _client.CreateRequest(EndPointConstant.AddBookToCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddJsonBody(bookDtoReq)
            .ExecutePostAsync<UnAuthorizationDtoRes>();
        return response;
    }
    public async Task<RestResponse<BadRequestDtoRes>> AddBookToCollectionBadRequest(AddBookToCollectionDtoReq bookDtoReq,string token)
    {
        var response = await _client.CreateRequest(EndPointConstant.AddBookToCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(bookDtoReq)
            .ExecutePostAsync<BadRequestDtoRes>();
        return response;
    }
    public async Task<RestResponse<UpdateBookDtoRes>> UpdateBookSuccess(UpdateBookInCollectionDtoReq updateBookDtoReq,string Isbn,string token)
    {
        var response = await _client.CreateRequest(string.Format(EndPointConstant.ReplaceBookInCollectionEnpoint, Isbn))
            .AddHeader("accept", "application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(updateBookDtoReq)
            .ExecutePutAsync<UpdateBookDtoRes>();
        return response;
    }
    public async Task<RestResponse<UnAuthorizationDtoRes>> UpdateBookAuthorization(UpdateBookInCollectionDtoReq updateBookDtoReq,string Isbn)
    {
        var response = await _client.CreateRequest(string.Format(EndPointConstant.ReplaceBookInCollectionEnpoint, Isbn))
            .AddHeader("accept", "application/json")
            .AddJsonBody(updateBookDtoReq)
            .ExecutePutAsync<UnAuthorizationDtoRes>();
        return response;
    }
    public async Task<RestResponse<BadRequestDtoRes>> UpdateBookBadRequest(UpdateBookInCollectionDtoReq updateBookDtoReq,string Isbn,string token)
    {
        var response = await _client.CreateRequest(string.Format(EndPointConstant.ReplaceBookInCollectionEnpoint, Isbn))
            .AddHeader("accept", "application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(updateBookDtoReq)
            .ExecutePutAsync<BadRequestDtoRes>();
        return response;
    }
    public RestResponse<DeleteBookDtoRes> DeleteBookSuccess(DeleteBookDtoReq deleteBookDtoReq,string token)
    {
        var response = _client.CreateRequest(EndPointConstant.DeleteBookFromCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddContentTypeHeader("application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(deleteBookDtoReq)
            .ExecuteDelete<DeleteBookDtoRes>();
        return response;
    }
    public RestResponse<UnAuthorizationDtoRes> DeleteBookAuthorization(DeleteBookDtoReq deleteBookDtoReq)
    {
        var response = _client.CreateRequest(EndPointConstant.DeleteBookFromCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddContentTypeHeader("application/json")
            .AddJsonBody(deleteBookDtoReq)
            .ExecuteDelete<UnAuthorizationDtoRes>();
        return response;
    }
    public RestResponse<BadRequestDtoRes> DeleteBookBadRequest(DeleteBookDtoReq deleteBookDtoReq,string token)
    {
        var response = _client.CreateRequest(EndPointConstant.DeleteBookFromCollectionEndPoint)
            .AddHeader("accept", "application/json")
            .AddContentTypeHeader("application/json")
            .AddAuthorizationHeader(token)
            .AddJsonBody(deleteBookDtoReq)
            .ExecuteDelete<BadRequestDtoRes>();
        return response;
    }
    public async Task<RestResponse<GetAllBookDtoRes>> GetAllBookSuccess()
    {
        var response = await _client.CreateRequest(EndPointConstant.GetAllBooks)
            .AddHeader("accept", "application/json")
            .AddContentTypeHeader("application/json")
            .ExecuteyGetAsync<GetAllBookDtoRes>();
        return response;
    }
    public  List<BookingColectionDtoRes> FindDuplicates(List<BookingColectionDtoRes> list1, List<BookingColectionDtoRes> list2)
    {
        HashSet<string> isbnSet = new HashSet<string>();
        List<BookingColectionDtoRes> duplicates = new List<BookingColectionDtoRes>();

        foreach (var book in list1)
        {
            isbnSet.Add(book.Isbn);
        }

        foreach (var book in list2)
        {
            if (isbnSet.Contains(book.Isbn))
            {
                duplicates.Add(book);
            }
        }

        return duplicates;
    }
    public async Task<RestResponse<AddBookToCollectionDt0Res>> AddBookToCollectionSuccess(string userId,string isbn,string token)
    {
        var bookReq = new AddBookToCollectionDtoReq()
        {
            UserId = userId,
            CollectionOfIsbns = new List<CollectionOfIsbn>()
            {
                new CollectionOfIsbn { Isbn = isbn  }
            }
        };
        return await AddBookToCollectionSuccess(bookReq, token);
    }
    public async Task<RestResponse<UnAuthorizationDtoRes>> AddBookToCollectionUnauthorized(string userId, string isbn)
    {
        var bookReq = new AddBookToCollectionDtoReq()
        {
            UserId = userId,
            CollectionOfIsbns = new List<CollectionOfIsbn>()
            {
                new CollectionOfIsbn { Isbn = isbn  }
            }
        };
        return await AddBookToCollectionUnauthorized(bookReq);
    }
    public async Task<RestResponse<BadRequestDtoRes>> AddBookToCollectionBadRequest(string userId,string isbn,string token)
    {
        var bookReq = new AddBookToCollectionDtoReq()
        {
            UserId = userId,
            CollectionOfIsbns = new List<CollectionOfIsbn>()
            {
                new CollectionOfIsbn { Isbn = isbn  }
            }
        };
        return await AddBookToCollectionBadRequest(bookReq, token);
    }
    public async Task<RestResponse<UpdateBookDtoRes>> UpdateBookSuccess( string userId,string isbnUpdate,string isbn,string token)
    {
        var updateBookReq = new UpdateBookInCollectionDtoReq()
        {
            Isbn = isbnUpdate,
            UserId = userId
        };
        return await UpdateBookSuccess(updateBookReq, isbn, token);
    }
    
    public async Task<RestResponse<UnAuthorizationDtoRes>> UpdateBookAuthorization( string userId,string isbnUpdate,string isbn)
    {
        var bookReq = new UpdateBookInCollectionDtoReq()
        {
            Isbn = isbnUpdate,
            UserId = userId
        };
        return await UpdateBookAuthorization(bookReq, isbn);
    }
    public async Task<RestResponse<BadRequestDtoRes>> UpdateBookBadRequest(string userId, string isbnUpdate,string isbn,string token)
    {
        var bookReq = new UpdateBookInCollectionDtoReq()
        {
            Isbn = isbnUpdate,
            UserId = userId
        };
        return await UpdateBookBadRequest(bookReq, isbn,token);
    }
    public RestResponse<DeleteBookDtoRes> DeleteBookSuccess(string userId,string isbn,string token)
    {
        var deleteBookReq = new DeleteBookDtoReq()
        {
            Isbn = isbn,
            UserId = userId
        };
        return DeleteBookSuccess(deleteBookReq, token);
    }
    public RestResponse<UnAuthorizationDtoRes> DeleteBookAuthorization(string userId, string isbn)
    {
        var deleteBookDtoRed = new DeleteBookDtoReq()
        {
            Isbn = isbn,
            UserId = userId
        };
        return DeleteBookAuthorization(deleteBookDtoRed);
    }
    public RestResponse<BadRequestDtoRes> DeleteBookBadRequest(string userId, string isbn,string token)
    {
        var deleteBookDtoRed = new DeleteBookDtoReq()
        {
            Isbn = isbn,
            UserId = userId
        };
        return DeleteBookBadRequest(deleteBookDtoRed,token);
    }
    public async Task<RestResponse<GetDetailBookDtoRes>> GetDetailBookSuccess(string isbn)
    {
        var response = await _client.CreateRequest(EndPointConstant.GetDetailBook)
            .AddParamater("ISBN", isbn)
            .ExecuteyGetAsync<GetDetailBookDtoRes>();
        return response;
    }
    
}