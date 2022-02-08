
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;



namespace Biblioteca.Areas.Identity.Data
{
    public class BibliotecaReservaLivro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("#")]
        public int Id { get; set; }

        public string? UserID  { get; set; }
        [DisplayName("Usuario")]
        [ForeignKey("UserID")]
        public virtual BibliotecaUser? BibliotecaUser { get; set; }

        public int LivroID { get; set; } 
        [DisplayName("Livro")]
        [ForeignKey("LivroID")]
        public virtual BibliotecaLivro? BibliotecaLivro { get; set; }

        //public List<BibliotecaLivro>? livros { get; set; }
        //public IList<BibliotecaLivro>? modelListLivros { get; set; }
        //public IList<BibliotecaReservaLivro>? modelListReservaLivros { get; set; }
    }
}
