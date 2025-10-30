using BookstoreASPNetServer.Models;
using BookstoreASPNetServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreASPNetServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IConfiguration _config;
        private readonly string[] _allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        public ProductController(IBookRepository bookRepository, IConfiguration config)
        {
            _bookRepository = bookRepository;
            _config = config;
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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] BookDataModel bookData, IFormFile? image)
        {
            if (image != null)
            {
                var imageUrl = await SaveFileAsync(image);
                if (imageUrl == null)
                {
                    return BadRequest("Could not upload file.");
                }
                bookData.ImageUrl = imageUrl;
            }
            else
            {
                bookData.ImageUrl = null;
            }
            var result = await _bookRepository.CreateBook(bookData);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPatch("{id}/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] BookUpdateModel bookData, IFormFile? image)
        {
            if (image != null)
            {
                var imageUrl = await SaveFileAsync(image);
                if (imageUrl == null)
                {
                    return BadRequest("Could not upload file.");
                }
                bookData.ImageUrl = imageUrl;
            }
            else
            {
                bookData.ImageUrl = null;
            }
            var result = await _bookRepository.UpdateBook(id, bookData);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var result = await _bookRepository.DeleteBook(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        private async Task<string?> SaveFileAsync(IFormFile file)
        { 
            if (file.Length > 0)
            {
                string ext = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Contains(ext)) return null;
                var fileName = Path.GetRandomFileName() + ext;
                var storedFilesPath = _config["StoredFilesPath"];
                if (storedFilesPath == null) return null;
                var filePath = Path.Combine(storedFilesPath, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }
    }
}
