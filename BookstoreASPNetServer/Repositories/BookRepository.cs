using BookstoreASPNetServer.Data;
using BookstoreASPNetServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreASPNetServer.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreContext _context;
        public BookRepository(BookstoreContext context) 
        {
            _context = context;
        }
        public async Task<BookModel?> CreateBook(BookDataModel bookData)
        {
            var book = new BookModel()
            {
                Name = bookData.Name,
                Author = bookData.Author,
                Publisher = bookData.Publisher,
                Price = bookData.Price,
                Discount = bookData.Discount
            };
            _context.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel?> DeleteBook(int id)
        {
            var book = await _context.Books.Include(b => b.CartItems).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return null;
            }
            if (book.CartItems != null)
            {
                _context.RemoveRange(book.CartItems);
            }
            _context.Remove(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        public async Task<BookModel?> GetBookById(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            return book;
        }

        public async Task<BookModel?> UpdateBook(int id, BookUpdateModel bookData)
        {
            var book = await _context.Books.Include(b => b.CartItems).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return null;
            if (bookData.Name != null)
            {
                book.Name = bookData.Name;
            }
            if (bookData.Author != null)
            {
                book.Author = bookData.Author;
            }
            if (bookData.Publisher != null)
            {
                book.Publisher = bookData.Publisher;
            }
            if (bookData.Price != null)
            {
                book.Price = bookData.Price ?? 0;
            }
            if (bookData.Discount != null)
            {
                book.Discount = bookData.Discount;
            }
            if (book.CartItems != null)
            {
                foreach (var item in book.CartItems)
                {
                    item.Product = book;
                }
            }
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
