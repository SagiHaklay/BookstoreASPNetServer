using BookstoreASPNetServer.Models;

namespace BookstoreASPNetServer.Repositories
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel?> GetBookById(int id);
        Task<BookModel?> CreateBook(BookDataModel bookData);
        Task<BookModel?> UpdateBook(int id, BookUpdateModel bookData);
        Task<BookModel?> DeleteBook(int id);
    }
}
