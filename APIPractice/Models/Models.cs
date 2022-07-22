using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models
{
    public class Movie
    {
        public int Id { get; set; }
        //[DisplayName("名前")]
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Text { get; set; }

        [DataType(DataType.Date)]
        public DateTime AddDate { get; set; }
    }
}
