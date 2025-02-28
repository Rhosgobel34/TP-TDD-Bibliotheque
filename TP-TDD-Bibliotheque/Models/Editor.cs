using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_TDD_Bibliotheque.Models
{
    [Table("editor")]
    public class Editor
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("Editor_name")]
        public string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();

        public Editor() {}
    }
}
