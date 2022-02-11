using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BibliotecaUser class
public class BibliotecaUser : IdentityUser
{
    [PersonalData]
    [DisplayName("Nome")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Campo Nome deve ter entre 3 e 30 caracteres")]
    [Required(ErrorMessage = "Nome do usuário deve ser preenchido")]
    [Column(TypeName="nvarchar(100)")]
    public string? FirstName { get; set; }

    [PersonalData]
    [DisplayName("Sobrenome")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Campo Nome deve ter entre 3 e 30 caracteres")]
    [Required(ErrorMessage = "Nome do usuário deve ser preenchido")]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [NotMapped]    
    [DataType(DataType.Text)]
    [Display(Name = "Perfis de usuário : ")]
    [UIHint("List")]
    public List<SelectListItem> Roles { get; set; }


    [NotMapped]
    public string Role { get; set; }

    public BibliotecaUser()
    {
        Roles = new List<SelectListItem>();
        //Roles.Add(new SelectListItem() { Value = Role, Text = Role });
        //Roles.Add(new SelectListItem() { Value = "8c17b769-70a7-4d14-910b-5568b5b10cb7", Text = "Admin" });
        //Roles.Add(new SelectListItem() { Value = "21556d8b-8f76-4866-a87d-0b0c1e8e41e6", Text = "Operator" });
        //Roles.Add(new SelectListItem() { Value = "1fa337b3-093e-42a7-8bbe-13688ea33ae4", Text = "User" });
        //Roles.Add(new SelectListItem() { Value = "eebf1033-12a0-4961-b801-020e947c36b2", Text = "Biblian" });
    }

}

