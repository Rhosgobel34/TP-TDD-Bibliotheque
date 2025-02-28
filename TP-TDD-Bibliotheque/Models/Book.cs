using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_TDD_Bibliotheque.Models
{
    [Table("book")]
    public class Book
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        [ForeignKey("Editor")]
        [Column("Editor_id")]
        public int EditorId { get; set; }

        public Book() {}

        public Book(string isbn, string title, bool available, int editorId)
        {
            ISBN = isbn;
            Title = title;
            Available = available;
            EditorId = editorId;
        }
    }
}
