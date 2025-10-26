using BookstoreASPNetServer.Models;
using BookstoreASPNetServer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreASPNetServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public ProductController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _bookRepository.GetAllBooks();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var result = await _bookRepository.GetBookById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] BookDataModel bookData)
        {
            var result = await _bookRepository.CreateBook(bookData);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPatch("{id}/update")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] BookUpdateModel bookData)
        {
            var result = await _bookRepository.UpdateBook(id, bookData);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var result = await _bookRepository.DeleteBook(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
