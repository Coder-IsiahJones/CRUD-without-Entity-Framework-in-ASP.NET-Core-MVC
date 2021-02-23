using System.ComponentModel.DataAnnotations;

namespace CRUD_without_Entity_Framework_in_ASP.NET_Core_MVC.Models
{
    public class BookVM
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}