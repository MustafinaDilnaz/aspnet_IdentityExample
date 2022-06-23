using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCIdentityExample.Models;
using MVCIdentityExample.ViewModels;

namespace MVCIdentityExample.Controllers
{
  public class RoleController : Controller
  {
    RoleManager<IdentityRole> _roleManager;
    UserManager<User> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager,
      UserManager<User> userManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;
    }

    public ActionResult Index()
    {
      return View(_roleManager.Roles.ToList());
    }

    // GET: RoleController/Details/5
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
      if (!string.IsNullOrEmpty(name))
      {
        var result = await _roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach(var item in result.Errors)
          {
            ModelState.AddModelError(String.Empty, item.Description); 
          }
        }
      }
      return View(name);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);
      if (role != null)
      {
        var result = await _roleManager.DeleteAsync(role);
       
      }
      return RedirectToAction("index");
    }

    public IActionResult UserList() => View(_userManager.Users.ToList());
    public async Task<IActionResult> Edit(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      ViewBag.UserId = userId;
      if(user != null)
      {
        var userRoles = await _userManager.GetRolesAsync(user);
        var roles = _roleManager.Roles.ToList();
        var model = new ChangeRoleViewModel
        {
          UserID = user.Id,
          UserEmail = user.Email,
          UserRoles = userRoles,
          AllRoles = roles
        };
        return View(model);
      }

      return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user != null)
      {
        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();
        var addedRoles = roles.Except(userRoles);
        var removeRoles = userRoles.Except(roles);
        await _userManager.RemoveFromRolesAsync(user, removeRoles);
        await _userManager.AddToRolesAsync(user, addedRoles);
        return RedirectToAction("UserList");

      }
      return NotFound();
    }

  }
}
