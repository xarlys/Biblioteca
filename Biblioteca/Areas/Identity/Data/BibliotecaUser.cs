using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BibliotecaUser class
public class BibliotecaUser : IdentityUser
{
    [PersonalData]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Campo Nome deve ter entre 3 e 30 caracteres")]
    [Required(ErrorMessage = "Nome do usuário deve ser preenchido")]
    [Column(TypeName="nvarchar(100)")]
    public string? FirstName { get; set; }

    [PersonalData]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Campo Nome deve ter entre 3 e 30 caracteres")]
    [Required(ErrorMessage = "Nome do usuário deve ser preenchido")]
    [Column(TypeName = "nvarchar(100)")]

    public string? LastName { get; set; }

}

