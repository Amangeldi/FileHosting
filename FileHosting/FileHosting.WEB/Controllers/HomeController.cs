using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileHosting.WEB.Models;
using Microsoft.AspNetCore.Identity;
using FileHosting.DAL.Entities;
using FileHosting.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using FileHosting.BLL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace FileHosting.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IFileService _fileService;
        public HomeController(SignInManager<User> signInManager, IUserService userService, IWebHostEnvironment appEnvironment,
            IFileService fileService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _appEnvironment = appEnvironment;
            _fileService = fileService;
        }
        [HttpGet("/Home")]
        [HttpGet("/")]
        [HttpGet("/Index")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<FileModel> userFiles = null;
            if(User.Identity.IsAuthenticated)
            {
                string userId = await _userService.GetUserId(User.Identity.Name);
                userFiles = await _fileService.GetUserFiles(userId);
            }
            return View(userFiles);
        }

        [HttpPost("~/Login")]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, false);
            if (result.Succeeded)
            {
                if (loginModel.ReturnUrl == null)
                {
                    return RedirectToAction("/Index");
                }
                else
                {
                    return new LocalRedirectResult(loginModel.ReturnUrl);
                }
            }
            ViewData["error"] = "Неправильный логин или пароль";
            return View();
        }

        [HttpGet("/Login/{ReturnUrl}")]
        [HttpGet("/Login")]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            ViewData["CurrentUrl"] = "Login";
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpGet("/Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new LocalRedirectResult("/");
        }
        [HttpGet("/Registration")]
        public IActionResult Registration()
        {
            ViewData["CurrentUrl"] = "Registration";
            return View();
        }

        [HttpPost("/Registration")]
        public async Task<IActionResult> Registration(UserRegistrationDTO userRegistrationDTO)
        {
            try
            {
                await _userService.CreateUser(userRegistrationDTO);
                await _signInManager.PasswordSignInAsync(userRegistrationDTO.Email, userRegistrationDTO.Password, true, false);
                return new LocalRedirectResult("/");
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View();
            }
        }

        [HttpPost("/UploadFile")]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if(file==null)
                {
                    throw new Exception("Пустой файл");
                }
                string userId = await _userService.GetUserId(User.Identity.Name);
                await _fileService.AddFile(file, _appEnvironment.WebRootPath, userId);
            }
            catch (Exception ex)
            {
                ViewBag.EmptyFile = true;
            }
            return new LocalRedirectResult("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
