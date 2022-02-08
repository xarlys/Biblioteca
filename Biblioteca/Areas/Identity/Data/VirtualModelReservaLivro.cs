namespace Biblioteca.Areas.Identity.Data
{
    public class VirtualModelReservaLivro
    {
        public virtual int Id { get; set; } = 0;
        public virtual string Name { get; set; } = null;
        public virtual string Description { get; set; } = null;
        public virtual int LivroID { get; set; } = 0;
        public virtual string? UserID { get; set; } = null;

        //public virtual IEnumerable<BibliotecaLivro> BookDataList { get; set; } = new List<BibliotecaLivro>();
        //public virtual IEnumerable<BibliotecaReservaLivro> BookReservationDataList { get; set; } = new List<BibliotecaReservaLivro>();


        //public class BookList
        //{
        //    public List<BibliotecaLivro> listLivro { get; set; } = null!;
        //    public List<BibliotecaReservaLivro> listReserva { get; set; } = null!;
        //}
        //public class BookReservaList
        //{
        //    public List<BibliotecaReservaLivro>? listReserva { get; set; }
        //}

    }
}
