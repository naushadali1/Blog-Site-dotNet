using BlogSite.Data;
using BlogSite.Models;
using BlogSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlogSite.Controllers
    {
    public class AdminController : Controller
        {
        AppDbContext db;
        IWebHostEnvironment env;
        public AdminController(AppDbContext _db, IWebHostEnvironment enviroment)
            {
            db = _db;
            env = enviroment;
            }
        public IActionResult Index()
            {
            if (HttpContext.Session.GetString("LoginFlag")!=null)
            {
                HttpContext.Session.SetString("LoginFlag", "True");
                ViewBag.NumberofPosts = db.tbl_Posts.Count();
                ViewBag.NumberofUsers = db.tbl_Profiles.Count();
                DisplayData();
                return View();
                }
            else
                {
                return RedirectToAction("login", "admin");
                }
            

            
            }

        public IActionResult AddPost()
            {
            if (HttpContext.Session.GetString("LoginFlag") != null)
                {
                DisplayData();
                return View();

                }
            else
                {
               // return RedirectToAction("login", "admin");
                return Redirect("/Admin/login?ReturnUrl=/Admin/addpost");
                }
            }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(PostVM mypost)
            
            {
            DisplayData();
            if (ModelState.IsValid)
                {
                string ImageName = mypost.Image.FileName.ToString();
                var Folderpath = Path.Combine(env.WebRootPath, "Images");
                var completePath = Path.Combine(Folderpath, ImageName);
                mypost.Image.CopyTo( new FileStream(completePath, FileMode.Create));

                Post post = new();
                post.Title = mypost.Title;
                post.Subtitle = mypost.Subtitle;
                post.Slug = mypost.Slug;
                post.Content = mypost.Content;
                post.Date = mypost.Date;
                post.Image = ImageName;
                db.Add(post);
                db.SaveChanges();
                    return RedirectToAction("Index", "Home");

               
                    
               
                }
            return View();

            }

        public IActionResult AllPosts()
            {
            if (HttpContext.Session.GetString("LoginFlag") != null)
                {
                DisplayData();
                var myPosts = db.tbl_Posts;
                return View(myPosts);
                }
            else
                {
                return RedirectToAction("login", "admin");
                }

            }

        public IActionResult DeletePost(int id)
            {
            var deletedPost = db.tbl_Posts.Find(id);
            if (deletedPost != null)
                {
                 db.Remove(deletedPost);
                db.SaveChanges();
                }
           return RedirectToAction("AllPosts", "Admin");
            }

        public IActionResult UpdatePost(int id)
            {
            if (HttpContext.Session.GetString("LoginFlag") != null)
                {
                DisplayData();
                var updatePost = db.tbl_Posts.Find(id);
                return View(updatePost);
                }
            else
                {
                return RedirectToAction("login", "admin");
                }

            }

        [HttpPost]
        public IActionResult UpdatePost(Post post)
            {
             db.tbl_Posts.Update(post);
            db.SaveChanges();
            return RedirectToAction("AllPosts", "Admin");

            }

        public IActionResult CreateProfile()
            {
            if (HttpContext.Session.GetString("LoginFlag") != null)
                {
                DisplayData();
                return View();
                }
            else
                {
                return RedirectToAction("login", "admin");
                }
            }

        [HttpPost]
        public IActionResult CreateProfile(ProfileVM profileVm)
            {
            if (ModelState.IsValid) {
                string ImageName = profileVm.Image.FileName.ToString();
                var Folderpath = Path.Combine(env.WebRootPath, "Images");
                var completePath = Path.Combine(Folderpath, ImageName);
                profileVm.Image.CopyTo(new FileStream(completePath, FileMode.Create));
                Profile profile = new Profile();    
                profile.Name = profileVm.Name;
                profile.UserName = profileVm.UserName;
                profile.Password = profileVm.Password;
                profile.Bio = profileVm.Bio;
               profile.Image = ImageName;
                db.tbl_Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
                }
            return View();
            }

        public IActionResult Login()
            {
            DisplayData();
            return View();
            }
        [HttpPost]
        public IActionResult Login(LoginVM loginVM, string? ReturnUrl)
            {
            if (ModelState.IsValid)
                {

                var result = db.tbl_Profiles.Where(opt=> opt.UserName.Equals(loginVM.UserName) &&  opt.Password.Equals(loginVM.Password)).FirstOrDefault();
                if (result != null) {
                    HttpContext.Session.SetInt32("ProfileId", result.Id);
                    HttpContext.Session.SetString("LoginFlag", "True");
                    if (ReturnUrl == null)
                        {
                        return RedirectToAction("Index", "admin");
                        }
                    else
                        {
                        return Redirect(ReturnUrl);
                        }
                    }
                ViewBag.LoginFlag = "Invalid Username or password";
                return View();
                }
                return View(new LoginVM());
            }
        public void DisplayData()
            {
            int? profileId = HttpContext.Session.GetInt32("ProfileId");

            if (profileId.HasValue)
                {
                ViewBag.Profile = db.tbl_Profiles.Where(x=> x.Id.Equals(profileId)).AsNoTracking().FirstOrDefault();
                }
            else
                {
                ViewBag.Profile = db.tbl_Profiles;
                }
            }

        public IActionResult UpdateProfile(int id)
            {
            DisplayData();
            var myProfile = db.tbl_Profiles.Find(id);
            ProfileVM vm = new ProfileVM();
            vm.Bio = myProfile.Bio;
            vm.Name = myProfile.Name;
            vm.Password = myProfile.Password;
            vm.ConfirmPassword = myProfile.Password;
            vm.UserName = myProfile.UserName;
            ViewData["Image"] = myProfile.Image;
            return View(vm);
            }

        [HttpPost]
        public IActionResult UpdateProfile(ProfileVM myProfile, string? oldPic)
            {
            DisplayData();
            string ImageName = null;
            if (ModelState.IsValid) {

               
                if (myProfile.Image!=null)
                {
                    ImageName = myProfile.Image.FileName.ToString();
                    var FolderPath = Path.Combine(env.WebRootPath, "Images");
                    var ImagePath = Path.Combine(FolderPath, ImageName);
                    myProfile.Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                }

                Profile originalProfile = new();
                originalProfile.Bio = myProfile.Bio;
                originalProfile.Name = myProfile.Name;
                originalProfile.Password = myProfile.Password;
                originalProfile.UserName = myProfile.UserName;
                originalProfile.Id= myProfile.Id;

                if (!string.IsNullOrEmpty(ImageName))
                    {
                    originalProfile.Image = ImageName;
                    }
                else
                    {
                    originalProfile.Image = oldPic;

                    }
               db.tbl_Profiles.Update(originalProfile);
                db.SaveChanges();
                return RedirectToAction("Index", "admin");
                
                }

            if (!string.IsNullOrEmpty(ImageName))
                {
                ViewData["Image"] = ImageName;
                }
            else
                {
                ViewData["Image"] = oldPic;

                }


            return View();
            }

        public IActionResult Logout ()
            {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "admin");
            }


        }
    }
