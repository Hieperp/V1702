using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using Owin;




using TotalCore.Repositories.Sales;
using TotalPortal.Models;
using TotalPortal.ViewModels.Home;
using TotalPortal.APIs.Sessions;
using TotalModel.Models;
using System.Data.Entity.Core.Objects;
using TotalCore.Repositories;



namespace TotalPortal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private readonly IBaseRepository baseRepository;
        public AccountController(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        #region Added by HIEP



        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Vendor")]
        public ActionResult Index()
        {

            var Db = new ApplicationDbContext();

            var users = Db.Users;

            var model = new List<EditUserViewModel>();

            foreach (var user in users)
            {

                var u = new EditUserViewModel(user);

                model.Add(u);

            }

            return View(model);

        }




        [Authorize(Roles = "Admin")]

        public ActionResult Edit(string id, TotalPortal.Controllers.ManageController.ManageMessageId? Message = null)
        {

            var Db = new ApplicationDbContext();

            var user = Db.Users.First(u => u.Id == id);

            var model = new EditUserViewModel(user);

            ViewBag.MessageId = Message;

            return View(model);

        }

        [HttpPost]

        [Authorize(Roles = "Admin")]

        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Edit(EditUserViewModel model)
        {

            if (ModelState.IsValid)
            {

                var Db = new ApplicationDbContext();

                var user = Db.Users.First(u => u.Email == model.Email);

                // Update the user data:

                user.FirstName = model.FirstName;

                user.LastName = model.LastName;

                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                await Db.SaveChangesAsync();

                return RedirectToAction("Index");

            }

            // If we got this far, something failed, redisplay form

            return View(model);

        }


        [Authorize(Roles = "Admin")]

        public ActionResult Delete(string id = null)
        {

            var Db = new ApplicationDbContext();

            var user = Db.Users.First(u => u.Id == id);

            var model = new EditUserViewModel(user);

            if (user == null)
            {

                return HttpNotFound();

            }

            return View(model);

        }


        [Authorize(Roles = "Admin")]

        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();

            var user = Db.Users.First(u => u.Id == id);

            var model = new SelectUserRolesViewModel(user);

            return View(model);

        }





        [HttpPost]

        [Authorize(Roles = "Admin")]

        [ValidateAntiForgeryToken]

        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {

            if (ModelState.IsValid)
            {

                var idManager = new IdentityManager();

                var Db = new ApplicationDbContext();

                var user = Db.Users.First(u => u.Email == model.Email);

                idManager.ClearUserRoles(user.Id);

                foreach (var role in model.Roles)
                {

                    if (role.Selected)
                    {

                        idManager.AddUserToRole(user.Id, role.RoleName);

                    }

                }

                return RedirectToAction("index");

            }

            return View();

        }



        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirmed(string id)
        {

            var Db = new ApplicationDbContext();

            var user = Db.Users.First(u => u.Id == id);

            Db.Users.Remove(user);

            Db.SaveChanges();

            return RedirectToAction("Index");

        }

        #endregion Added by HIEP

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")] //[AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            this.GetOrganizationalUnitSelectList(registerViewModel);

            return View(registerViewModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")] //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            this.GetOrganizationalUnitSelectList(model);
            return View(model);
        }

        private void GetOrganizationalUnitSelectList(RegisterViewModel registerViewModel)
        {
            if (this.baseRepository != null)
            {
                ICollection<LocationOrganizationalUnit> locationOrganizationalUnits = this.baseRepository.ExecuteFunction<LocationOrganizationalUnit>("GetLocationOrganizationalUnits", new ObjectParameter[] { new ObjectParameter("Nothing", 0) });
                registerViewModel.OrganizationalUnitSelectList = locationOrganizationalUnits.Select(pt => new SelectListItem { Text = pt.LocationOrganizationalUnitCode, Value = pt.OrganizationalUnitID.ToString() }).ToList();
            }
            registerViewModel.OrganizationalUnitSelectList.Add(new SelectListItem { Text = "[Vui lòng chọn division]", Value = "0" });
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion





















        #region Smart Biztech
        [Authorize(Roles = "Admin")]
        public ActionResult UserReferences()
        {
            this.AddUserAndRoles();

            return View();
        }
        #endregion Smart Biztech

        #region AddUserAndRoles: Run only one
        private bool AddUserAndRoles()
        {
            return true;

            bool success = false;
            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");
            if (!success == true) return success; //NEED Admin FOR ACCOUNT MANAGEMENT

            success = idManager.AddUserToRole("dade9d9f-2a76-4d5d-9322-f4d47a12e50a", "Admin");
            if (!success) return success; //ADD ROLE Admin TO hieperp@gmail.com




            return true;
            return true;
            success = idManager.CreateRole("Vendor");
            if (!success == true) return success; //NEED Vendor FOR SPECIAL TASK ONLY FOR VENDOR

            success = idManager.AddUserToRole("dade9d9f-2a76-4d5d-9322-f4d47a12e50a", "Vendor");
            if (!success) return success; //ADD ROLE Vendor TO hieperp@gmail.com


            return true; //AT VCP: JUST RUN AS ABOVE










            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;








            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "User");

            if (!success) return success;



            return success;

        }


        private bool AddUserAndRoles2()
        {

            bool success = false;



            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;





            var newUser = new ApplicationUser()

            {
                Email = "tanthanhhotel@gmail.com",

                FirstName = "Tan Thanh",

                LastName = "Admin"

            };



            // Be careful here - you  will need to use a password which will 

            // be valid under the password rules for the application, 

            // or the process will abort:

            success = idManager.CreateUser(newUser, "TanThanh@014");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "User");

            if (!success) return success;



            return success;

        }

        #endregion AddUserAndRoles: Run only one



    }
}