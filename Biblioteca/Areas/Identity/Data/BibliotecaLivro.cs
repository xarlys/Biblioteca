using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Areas.Identity.Data
{
    public class BibliotecaLivro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("#")]

        public int Id { get; set; }

        [DisplayName("NOME")]
        [Required(ErrorMessage = "Nome do livro deve ser preenchido")]
        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [DisplayName("DESCRIÇÃO")]
        [Required(ErrorMessage = "Descrição do livro deve ser preenchido")]
        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        //public virtual int LivroID { get; set; } = 0;
        //public virtual string? UserID { get; set; } = null;
        //public IList<BibliotecaReservaLivro> reservas { get; set; } = new List<BibliotecaReservaLivro>();
        //public ICollection<BibliotecaReservaLivro>? reservalivro { get; set; }
    }
}
