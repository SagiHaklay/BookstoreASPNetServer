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
        public async Task<BookProductModel?> CreateBook(BookDataModel bookData)
        {
            var book = new BookModel()
            {
                Name = bookData.Name,
                Author = bookData.Author,
                Publisher = bookData.Publisher,
                Price = bookData.Price,
                Discount = bookData.Discount,
                ImageUrl = bookData.ImageUrl
            };
            _context.Add(book);
            await _context.SaveChangesAsync();
            return new BookProductModel()
            {
                Id = book.Id.ToString(),
                Name = book.Name,
                Publisher = book.Publisher,
                Price = book.Price,
                Discount = book.Discount,
                ImageUrl = book.ImageUrl
            };
        }

        public async Task<BookProductModel?> DeleteBook(int id)
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
            return new BookProductModel()
            {
                Id = book.Id.ToString(),
                Name = book.Name,
                Publisher = book.Publisher,
                Price = book.Price,
                Discount = book.Discount,
                ImageUrl = book.ImageUrl
            };
        }

        public async Task<List<BookProductModel>> GetAllBooks()
        {
            var books = await _context.Books.Select(b => new BookProductModel() {
                Id = b.Id.ToString(),
                Name = b.Name,
                Publisher = b.Publisher,
                Price = b.Price,
                Discount = b.Discount,
                ImageUrl = b.ImageUrl
            }).ToListAsync();
            return books;
        }

        public async Task<BookProductModel?> GetBookById(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return null;
            }
            return new BookProductModel()
            {
                Id = book.Id.ToString(),
                Name = book.Name,
                Publisher = book.Publisher,
                Price = book.Price,
                Discount = book.Discount,
                ImageUrl = book.ImageUrl
            };
        }

        public async Task<BookProductModel?> UpdateBook(int id, BookUpdateModel bookData)
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
            if (bookData.ImageUrl != null)
            {
                book.ImageUrl = bookData.ImageUrl;
            }
            if (book.CartItems != null)
            {
                foreach (var item in book.CartItems)
                {
                    item.Product = book;
                }
            }
            await _context.SaveChangesAsync();
            return new BookProductModel()
            {
                Id = book.Id.ToString(),
                Name = book.Name,
                Publisher = book.Publisher,
                Price = book.Price,
                Discount = book.Discount,
                ImageUrl = book.ImageUrl
            };
        }
    }
}
