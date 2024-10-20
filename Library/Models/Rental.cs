

namespace Library.Models
{
    public class Rental
    {
            public int Id { get; set; }
            public User User { get; set; }
            public Book Book { get; set; }
            public DateTime RentalDate { get; set; }
            public DateTime ReturnDate { get; set; }
            public string Status { get; set; }
    }
}
