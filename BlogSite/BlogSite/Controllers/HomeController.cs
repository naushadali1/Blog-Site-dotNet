using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;


namespace BlogSite.Controllers
    {
    public class HomeController : Controller
        {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
            {
            db= _db;
            }

        public IActionResult Index(string? searchquery)
            {
           
            if (!string.IsNullOrEmpty(searchquery))
            {
                PostAndProfile();
                IEnumerable<Post> Posts = db.tbl_Posts.Where(x=> x.Content.Contains(searchquery));
                return View(Posts);

            }
            PostAndProfile();
            IEnumerable<Post> myPosts= db.tbl_Posts;
            return View(myPosts);
            }

        public IActionResult Post(string Slug)
            {
            PostAndProfile();
            Post? singlePost =  db.tbl_Posts.Where(x=> x.Slug == Slug).FirstOrDefault();
            return View(singlePost);
            }

        public void PostAndProfile()
            {
           ViewBag.Post = db.tbl_Posts;
           ViewBag.Profile = db.tbl_Profiles.FirstOrDefault();
            }

        public IActionResult Error()
            {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
