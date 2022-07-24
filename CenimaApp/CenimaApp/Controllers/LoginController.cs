using Microsoft.AspNetCore.Mvc;
using CenimaApp;
using CenimaApp.Models;
using System.Linq;
using CenimaApp.Models;
using CenimaApp;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace CenimaApp.Controllers
{
    public class LoginController : Controller
    {
        CenimaDBContext context = new CenimaDBContext();

        public const string PERSONKEY = "person";
        public IActionResult Index()
        {
            ViewBag.Person = GetPerson();
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            Person person = context.Persons.Where(person => person.Email.Equals(email) && person.Password.Equals(password)).FirstOrDefault();
            if (person == null)
            {
                string message = "Tài khoản hoặc mật khẩu không đúng ! Vui lòng nhập lại.";
                ViewBag.Message = message;
            }
            else
            {
                //Redirect to Admin Dashboard
                if (person.Type == 1)
                {
                    SaveCartSession(person);
                    ViewBag.Person = GetPerson();
                    return RedirectToAction("Index", "Admin");
                }
                //Redirect to home.
                else
                {
                    SaveCartSession(person);
                    return RedirectToAction("Index", "Home");
                }

            }

            return RedirectToAction("Index", "Login");
        }
        Person GetPerson()
        {
            var session = HttpContext.Session;
            string jsonPerson = session.GetString(PERSONKEY);
            if (jsonPerson != null)
            {
                return JsonConvert.DeserializeObject<Person>(jsonPerson);
            }
            return new Person();
        }
        void SaveCartSession(Person per)
        {
            var session = HttpContext.Session;
            string jsonPerson = JsonConvert.SerializeObject(per);
            session.SetString(PERSONKEY, jsonPerson);
        }
        void DeleteSession()
        {
            //Delelte Session
            var session = HttpContext.Session;
            session.Remove(PERSONKEY);
        }
        public IActionResult Logout()
        {
            DeleteSession();
            return RedirectToAction("Index", "Home");
        }
    }
}