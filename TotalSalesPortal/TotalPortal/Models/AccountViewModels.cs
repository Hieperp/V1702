using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            this.OrganizationalUnitSelectList = new List<SelectListItem>();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // New Fields added to extend Application User class:
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Division")]
        [Range(1, 999999, ErrorMessage = "Vui lòng chọn division")]
        public int OrganizationalUnitID { get; set; }
        public List<SelectListItem> OrganizationalUnitSelectList { get; set; }

        [Display(Name = "Access right applies to my own division")]
        public int SameOUAccessLevel { get; set; }
        [Display(Name = "Access right applies to my own location")]
        public int SameLocationAccessLevel { get; set; }
        [Display(Name = "Access right applies to other location")]
        public int OtherOUAccessLevel { get; set; }
        public List<SelectListItem> AccessLevelSelectList { get; set; }

        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
            return user;
        }

    }



    public class EditUserViewModel
    {

        public EditUserViewModel() { }


        // Allow Initialization with an instance of ApplicationUser:

        public EditUserViewModel(ApplicationUser user)
        {

            this.Id = user.Id;

            this.Email = user.Email;

            this.FirstName = user.FirstName;

            this.LastName = user.LastName;

        }


        [Required]

        public string Id { get; set; }

        [Required]

        public string Email { get; set; }


        [Required]

        [Display(Name = "First Name")]

        public string FirstName { get; set; }



        [Required]

        [Display(Name = "Last Name")]

        public string LastName { get; set; }

    }


    public class SelectUserRolesViewModel
    {

        public SelectUserRolesViewModel()
        {

            this.Roles = new List<SelectRoleEditorViewModel>();

        }





        // Enable initialization with an instance of ApplicationUser:

        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {

            this.Email = user.Email;

            this.FirstName = user.FirstName;

            this.LastName = user.LastName;



            var Db = new ApplicationDbContext();



            // Add all available roles to the list of EditorViewModels:

            var allRoles = Db.Roles;

            foreach (var role in allRoles)
            {

                // An EditorViewModel will be used by Editor Template:

                var rvm = new SelectRoleEditorViewModel(role);

                this.Roles.Add(rvm);

            }



            // Set the Selected property to true for those roles for 

            // which the current user is a member:

            foreach (var userRole in user.Roles)
            {

                var checkUserRole =

                    this.Roles.Find(r => r.RoleId == userRole.RoleId);

                checkUserRole.Selected = true;

            }

        }



        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }

    }


    // Used to display a single role with a checkbox, within a list structure:

    public class SelectRoleEditorViewModel
    {

        public SelectRoleEditorViewModel() { }

        public SelectRoleEditorViewModel(IdentityRole role)
        {

            this.RoleName = role.Name;
            this.RoleId = role.Id;

        }



        public bool Selected { get; set; }



        [Required]

        public string RoleId { get; set; }

        [Required]

        public string RoleName { get; set; }

    }




    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
