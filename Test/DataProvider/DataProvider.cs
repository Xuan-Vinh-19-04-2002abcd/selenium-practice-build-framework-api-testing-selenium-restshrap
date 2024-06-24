using Practice.Services.Model.Response;

namespace Practice.Test;

public class DataProvider
{
    public static string IsbnProviderValid(List<BookingColectionDtoRes> listBook, string previousIsbn = null)
    {
        if (listBook == null || listBook.Count == 0)
        {
            throw new ArgumentException("The list of books is empty or null.");
        }

        Random random = new Random();
        string randomIsbn;
        int randomIndex;
        do
        {
            randomIndex = random.Next(listBook.Count);
            randomIsbn = listBook[randomIndex].Isbn;
        } while (previousIsbn != null && randomIsbn == previousIsbn);

        return randomIsbn;
    }
    public static string IsbnProviderInValid()
    {
        Random rand = new Random();
        int[] digits = new int[13];
        
        for (int i = 0; i < 13; i++)
        {
            digits[i] = rand.Next(0, 10);
        }
        
        string isbn = string.Join("", digits);
        
        return isbn;
    }
}