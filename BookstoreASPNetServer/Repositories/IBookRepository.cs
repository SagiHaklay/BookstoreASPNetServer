using BookstoreASPNetServer.Models;

namespace BookstoreASPNetServer.Repositories
{
    public interface IBookRepository
    {
        Task<List<BookProductModel>> GetAllBooks();
        Task<BookProductModel?> GetBookById(int id);
        Task<BookProductModel?> CreateBook(BookDataModel bookData);
        Task<BookProductModel?> UpdateBook(int id, BookUpdateModel bookData);
        Task<BookProductModel?> DeleteBook(int id);
    }
}
