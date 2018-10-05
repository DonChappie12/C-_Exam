using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using c__exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace c__exam.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Create(ValidateUser user)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<ValidateUser> Hasher = new PasswordHasher<ValidateUser>();
                user.Password = Hasher.HashPassword(user, user.Password);
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id",newUser.Id);
                return RedirectToAction("Home");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginIn(string Email, string Password)
        {
            var user = _context.user.Where(u=> u.Email == Email).FirstOrDefault();
            if(user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, Password))
                {
                    HttpContext.Session.SetInt32("user_id", user.Id);
                    return RedirectToAction("Home");
                }
            }
            ViewBag.error="Email and/or Password dont match";
            return View("Index");
        }

        [Route("home")]
        public IActionResult Home()
        {
            User currUser = _context.user.SingleOrDefault(c => c.Id == HttpContext.Session.GetInt32("user_id"));
            if( currUser != null )
            {
                User user = _context.user.Include(j => j.Joining).Include(a => a.Activity).SingleOrDefault(c => c.Id == HttpContext.Session.GetInt32("user_id"));
                List <Activities> active = _context.activity.Include(x => x.user).Include(join => join.Joining).ToList();
                ViewBag.activities = active;
                ViewBag.id = currUser.Id;
                // TimeSpan timespan = new TimeSpan(user.Activity.Time);
                // DateTime time = DateTime.Today.Add(timespan);
                // string displayTime = time.ToString("hh:mm tt");
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Route("New")]
        public IActionResult New()
        {
            User currUser = _context.user.SingleOrDefault(c => c.Id == HttpContext.Session.GetInt32("user_id"));
            if( currUser != null )
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Route("activity/{Id}")]
        public IActionResult Actividad(int Id)
        {
            User currUser = _context.user.SingleOrDefault(c => c.Id == HttpContext.Session.GetInt32("user_id"));
            if( currUser != null )
            {
                Activities thisActivity = _context.activity.Include(x => x.user).SingleOrDefault(x => x.Id == Id);
                List<Activities> activities = _context.activity.Where(I => I.Id == Id).Include(u => u.user).Include(j => j.Joining).ThenInclude(uj => uj.user).ToList();
                ViewBag.info = activities;
                ViewBag.name = thisActivity.user.FirstName;
                return View("Activity", thisActivity);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("create_activity")]
        public IActionResult NewActivity(Activities activity)
        {
            User currUser = _context.user.SingleOrDefault(c=>c.Id == HttpContext.Session.GetInt32("user_id"));
            if(currUser!=null)
            {
                if(ModelState.IsValid)
                {
                    if(activity.Date > DateTime.Now)
                    {
                        Activities newActivity = new Activities
                        {
                            Title = activity.Title,
                            Time = activity.Time,
                            Date = activity.Date,
                            Duration = activity.Duration,
                            Description = activity.Description,
                            User_Id = currUser.Id
                        };
                        _context.activity.Add(newActivity);
                        _context.SaveChanges();
                        return RedirectToAction("Home");
                    }
                    ViewBag.date = "You's a time traveller?";
                    return View("New");
                }
                return View("New");
            }
            return RedirectToAction("Index");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            User currUser = _context.user.SingleOrDefault(c=>c.Id == HttpContext.Session.GetInt32("user_id"));
            if( currUser != null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Route("join/{Id}")]
        public IActionResult Join(int Id)
        {
            int curId = (int)HttpContext.Session.GetInt32("user_id");
            Activities thisActivity = _context.activity.Include(x => x.Joining).ThenInclude(x => x.user).SingleOrDefault(x => x.Id == Id);
            Join newCommer = new Join()
            {
                User_Id = curId,
                Activity_Id = Id
            };
            _context.join.Add(newCommer);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [Route("unjoin/{Id}")]
        public IActionResult UnJoin(int Id)
        {
            int curId = (int)HttpContext.Session.GetInt32("user_id");
            Join thisJoin = _context.join.SingleOrDefault(x => x.Activity_Id == Id && x.User_Id == curId);
            if(thisJoin!=null)
            {
                _context.join.Remove(thisJoin);
                _context.SaveChanges();
            }
            return RedirectToAction("Home");
        }

        [Route("delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            Activities thisActivity = _context.activity.Include(x => x.Joining).SingleOrDefault(x => x.Id == Id);
            foreach(var at in thisActivity.Joining)
            {
                _context.Remove(at);
            }
            _context.Remove(thisActivity);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
