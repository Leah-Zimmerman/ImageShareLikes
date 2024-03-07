using ImageShareLikes.Web.Models;
using ImageShareLikesEF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ImageShareLikes.Web.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private string _connectionString;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetImages()
        {
            var repo = new ImageRepository(_connectionString);
            var images = repo.GetImages();
            return Json(images);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile image, string title)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            var fs = new FileStream(imagePath, FileMode.CreateNew);
            image.CopyTo(fs);

            var repo = new ImageRepository(_connectionString);
            repo.AddImage(imagePath, fileName, title);
            return Redirect("/");
        }
        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            return View(repo.GetImageById(id));
        }
        public IActionResult DisplayImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            var image = repo.GetImageById(id);
            var imageInSession = HttpContext.Session.Get<List<int>>("Session");
            if (imageInSession == null || !imageInSession.Contains(id))
            {
                return Json(new ViewImageViewModel
                {
                    Image = repo.GetImageById(id),
                    Disable = false
                });
            }
            return Json(new ViewImageViewModel
            {
                Image = repo.GetImageById(id),
                Disable = true
            });
        }
        [HttpPost]
        public IActionResult AddToSession(int id)
        {
            var imagesLiked = HttpContext.Session.Get<List<int>>("Session");
            if (imagesLiked == null)
            {
                imagesLiked = new List<int>();
            }
            imagesLiked.Add(id);
            HttpContext.Session.Set("Session", imagesLiked);
            return Redirect($"/home/increaselikes?id={id}");
        }
        public IActionResult IncreaseLikes(int id)
        {
            var repo = new ImageRepository(_connectionString);
            repo.IncreaseViews(id);
            return Redirect($"/home/viewimage?id={id}");
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}