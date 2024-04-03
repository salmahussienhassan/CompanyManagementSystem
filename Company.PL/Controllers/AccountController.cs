using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUer> _userManager;
        private readonly SignInManager<ApplicationUer> _signInManager;

        public AccountController(UserManager<ApplicationUer> userManager,SignInManager<ApplicationUer> signInManager) 
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }


        //SingUp

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) //server side validation
            {
                var user = new ApplicationUer()
                {
                    UserName = registerViewModel.Email.Split("@")[0],
                    FName = registerViewModel.FName,
                    LName = registerViewModel.LName,
                    Email = registerViewModel.Email,
                    IsAgree = registerViewModel.IsAgree,

                };

                var result = await _userManager.CreateAsync(user,registerViewModel.Password);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);
                }
            }

            return View(registerViewModel);
        }

        //Login
        public IActionResult Login()
        {
           return View();
        }


        [HttpPost]
        public async Task< IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                 var result= await  _userManager.CheckPasswordAsync(user, model.Password);
                    if(result)
                    {
                      var LoginResult= await  _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,false);

                        if (LoginResult.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password or Email is Invaild");
                        return RedirectToAction(nameof(SignUp));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Not Exist");
                }
            }
            return View(model);
        }


        //SignOut

        public async Task< IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        //ForgetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> SendEmail(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if(ModelState.IsValid)
            {
                var user= await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);

                if(user is not null)
                {
                    var Token=await _userManager.GeneratePasswordResetTokenAsync(user);

                    //https://localhost:44344/Account/ResetPasswordLink?email=salmahussien417@gmail.com?token=akdnvksdnfkvdfvnjdfnjnjvkjf44

                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { forgetPasswordViewModel.Email, Token }, Request.Scheme);

                    //send Email
                    var email = new Email()
                    {
                        To = user.Email,
                        Subject="Reset Password",
                        Body= ResetPasswordLink
                    };

                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));

                }
                else
                    ModelState.AddModelError(string.Empty, "Email Is Not Found");
            }
                return View(nameof(ForgetPassword),forgetPasswordViewModel);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        //ResetPassword
        public IActionResult ResetPassword(string Email,string token)
        {
            TempData["Email"] = Email;
            TempData["Token"]=token;
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.NewPassword);

                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach(var e in  result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, e.Description);
                    }
                }
            }

            return View();
        }



    }
}
