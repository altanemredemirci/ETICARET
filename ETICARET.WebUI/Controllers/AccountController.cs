using ETICARET.WebUI.EmailServices;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
      

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //generated token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });


                //send email

                string siteUrl = "http://localhost:5174";
                string activeUrl = $"{siteUrl}{callbackUrl}";

                //EmailSender.Execute("Hesabınızı onaylayınız.", $"Lütfen email hasebınızı onayalamak için linke <a href='{activeUrl}' target='_blank'> tıklayınız.</a>..", model.Email).Wait();

                string body = $"Hesabınızı onaylayınız. <br> <br> Lütfen email hasebınızı onayalamak için linke <a href='{activeUrl}' target='_blank'> tıklayınız.</a>..";

                MailHelper.SendEmail(body, model.Email, "ETICARET HESAP AKTİFLEŞTİRME");

                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Email adresinize gelen link ile hesabınızı onaylanyınız.",
                    Css = "warning"
                });

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu.");
            return View(model);
        }

        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl=ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.Remove("ReturnUrl");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamıştır.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email ile onaylayınız");
                return View(model);
            }

            //isPersistent = Oturum kapatıldığında giriş unutulsun mu?
            //lock: 5 yanlış girişte kilitleme
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Email veya şifre yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı.",
                Css = "warning"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
           if(userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Geçersiz Token",
                    Message = "Hesap onayı için bilgileriniz yanlış",
                    Css = "danger"
                });
                return Redirect("~/");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız onaylanmıştır.",
                        Css = "success"
                    });
                    return RedirectToAction("Login","Account");
                }
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanmadı.",
                Css = "danger"
            });
            return Redirect("~/");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Email adresi ile bir kullanıcı bulunamadı.",
                    Css = "danger"
                });
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });

            string siteUrl = "http://localhost:5174";
            string body = $"Parolanızı yenilemek için linke <a href='{siteUrl}{callbackUrl}'> tıklayınız.</a>";

            MailHelper.SendEmail(body, Email, "ETICARET Şifre Resetleme");

            return RedirectToAction("Login");
        }

        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel() { Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

    }
}
