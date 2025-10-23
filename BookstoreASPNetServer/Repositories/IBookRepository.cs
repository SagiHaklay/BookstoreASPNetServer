using BookstoreASPNetServer.Models;

namespace BookstoreASPNetServer.Repositories
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel?> GetBookById(string id);
        Task<BookModel?> CreateBook(BookDataModel bookData);
        Task<BookModel?> UpdateBook(string id, BookUpdateModel bookData);
        Task<BookModel?> DeleteBook(string id);
    }
}
