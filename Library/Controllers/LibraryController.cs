
using Library.Models;
using Library.Servises;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<LibraryController> _logger;
        public LibraryController(
           ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        private static List<Rental> rentals = DataStorage.LoadFromFile(); // Завантаження даних із файлу під час ініціалізації

        // 1. Пошук аренди за статусом
        [HttpGet("rentals")]
        public IActionResult GetRentalsByStatus([FromQuery] string status)
        {
            var result = rentals.Where(r => r.Status == status).ToList();
            return Ok(result);
        }

        // 2. Пошук книги за ID
        [HttpGet("books/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = rentals.Select(r => r.Book).FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                return Ok(book);
            }
            return NotFound();

        }

        // 3. Створення аренди
        [HttpPost("rentals")]
        public IActionResult CreateRental([FromBody] Rental rental)
        {
            rentals.Add(rental);
            DataStorage.SaveToFile(rentals); // Збереження змін у файл
            return CreatedAtAction(nameof(GetRentalsByStatus), new { status = rental.Status }, rental);
        }

        // 4. Оновлення статусу аренди
        [HttpPut("rentals/{id}")]
        public IActionResult UpdateRentalStatus(int id, [FromBody] string status)
        {
            // Допустимі статуси аренди
            var validStatuses = new List<string> { "active", "returned", "overdue" };
          
            // Перевірка, чи статус є допустимим
            if (!validStatuses.Contains(status.ToLower()))
            {
                return BadRequest("Invalid rental status. Status must be 'active', 'returned', or 'overdue'.");
            }

            // Знаходимо аренду за ID
            var rental = rentals.FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
               
                return NotFound(); // Повертаємо 404, якщо аренда не знайдена
            }
            // Оновлюємо статус аренди
            rental.Status = status;

            // Оновлення статусу книги в залежності від нового статусу аренди
            if (status.ToLower() == "returned")
            {
                rental.Book.Status = "available"; // Книга доступна
            }
            else
            {
                rental.Book.Status = "not available"; // Книга недоступна
            }

            // Зберігаємо зміни у файл
            DataStorage.SaveToFile(rentals);


            // Повертаємо оновлену аренду
            return Ok(rental); 
        }


        // 5. Видалення аренди
        [HttpDelete("rentals/{id}")]
        public IActionResult DeleteRental(int id)
        {
            var rental = rentals.FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }
            rentals.Remove(rental);
            DataStorage.SaveToFile(rentals); // Збереження змін у файл
            return NoContent();
        }

        

    }
}
