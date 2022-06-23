using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCIdentityExample.Models;
using MVCIdentityExample.ViewModels;

namespace MVCIdentityExample.Controllers
{
    public class UserController : Controller {
      private readonly UserManager<User> _userManager;
      private readonly SignInManager<User> _signInManager;


      public UserController(UserManager<User> userManager,
        SignInManager<User> signInManager) {
              _userManager = userManager;
              _signInManager = signInManager;
      }

      public IActionResult Index(){
        return View(_userManager.Users.ToList());
      }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
          return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
          if (ModelState.IsValid)
          {
            User user =
            new User
            {
              Email = model.Email,
              UserName = model.Email,
              BirthYear = model.Year
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
              await _signInManager.SignInAsync(user, false);
              return RedirectToAction("Index", "Home");
            }
            else
            {
              foreach (var error in result.Errors)
              {
                ModelState.AddModelError(String.Empty, error.Description);
              }
            }
          }
          return View(model);
    
        }

    [HttpGet]
    public IActionResult Update(string id)
    {
      ViewBag.UserId = id;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        var newUser = _userManager.FindByIdAsync(ViewBag.UserId); 
        newUser =
        new User
        {
          Email = model.Email,
          UserName = model.Email,
          BirthYear = model.Year
        };
        var result = await _userManager.UpdateAsync(newUser);
        if (result.Succeeded)
        {
          await _signInManager.SignInAsync(newUser, false);
          return RedirectToAction("Index", "Home");
        }
        else
        {
          foreach (var error in result.Errors)
          {
            ModelState.AddModelError(String.Empty, error.Description);
          }
        }
      }
      return View(model);

    }

    //public IActionResult UpdatePassword(string id)
    //{
    //  var user = _userManager.FindByIdAsync(id);
    //  if(user == null)
    //  {
    //    return NotFound();
    //  }
    //  var updatePasswordModel = new UpdatePasswordViewModel { Id = user.Id, Email = user.Email };
    //  return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> UpdatePassword( UpdatePasswordViewModel model)
    //{
    //  if (ModelState.IsValid)
    //  {
    //    var user = await _userManager.FindByIdAsync(model.Id);
    //    if(user != null)
    //    {
    //      var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
    //      if (!result.Succeeded)
    //      {
    //        return Conflict();
    //      }
    //    }
    //  }
    //}
  }
}
