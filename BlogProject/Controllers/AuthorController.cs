using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogProject.Models;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Author/List
        public ActionResult List(string SearchKey = null)
        {
            AuthorDataController controller = new AuthorDataController();
            IEnumerable<Author> Authors = controller.ListAuthors(SearchKey);
            return View(Authors);
        }

        //GET : /Author/Show/{id}
        public ActionResult Show(int id)
        {
            AuthorDataController controller = new AuthorDataController();
            Author NewAuthor = controller.FindAuthor(id);
            

            return View(NewAuthor);
        }

        //GET : /Author/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            AuthorDataController controller = new AuthorDataController();
            Author NewAuthor = controller.FindAuthor(id);


            return View(NewAuthor);
        }


        //POST : /Author/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AuthorDataController controller = new AuthorDataController();
            controller.DeleteAuthor(id);
            return RedirectToAction("List");
        }

        //GET : /Author/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Author/Create
        [HttpPost]
        public ActionResult Create(string AuthorFname, string AuthorLname, string AuthorBio, string AuthorEmail)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(AuthorFname);
            Debug.WriteLine(AuthorLname);
            Debug.WriteLine(AuthorBio);

            Author NewAuthor = new Author();
            NewAuthor.AuthorFname = AuthorFname;
            NewAuthor.AuthorLname = AuthorLname;
            NewAuthor.AuthorBio = AuthorBio;
            NewAuthor.AuthorEmail = AuthorEmail;

            AuthorDataController controller = new AuthorDataController();
            controller.AddAuthor(NewAuthor);

            return RedirectToAction("List");
        }

    }
}