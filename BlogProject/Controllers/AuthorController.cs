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
        //We can instantiate the authorcontroller outside of each method
        private AuthorDataController controller = new AuthorDataController();

        // GET: Author
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Author/Error
        /// <summary>
        /// This window is for showing Author Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        //GET : /Author/List
        public ActionResult List(string SearchKey = null)
        {
            try{
                //Try to get a list of authors.
                IEnumerable<Author> Authors = controller.ListAuthors(SearchKey);
                return View(Authors);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex.Message);
                return RedirectToAction("Error","Home");
            }
        }

        //GET : /Author/Ajax_List
        public ActionResult Ajax_List()
        {
            return View();
        }
         

        //GET : /Author/Show/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Author SelectedAuthor = controller.FindAuthor(id);
                return View(SelectedAuthor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Author/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Author NewAuthor = controller.FindAuthor(id);
                return View(NewAuthor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }


        //POST : /Author/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                controller.DeleteAuthor(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET : /Author/New
        public ActionResult New()
        {
            return View();
        }

        //GET : /Author/Ajax_New
        public ActionResult Ajax_New()
        {
            return View();
        }

        //POST : /Author/Create
        [HttpPost]
        public ActionResult Create(string AuthorFname, string AuthorLname, string AuthorBio, string AuthorEmail)
        {
            try
            {
                Author NewAuthor = new Author();
                NewAuthor.AuthorFname = AuthorFname;
                NewAuthor.AuthorLname = AuthorLname;
                NewAuthor.AuthorBio = AuthorBio;
                NewAuthor.AuthorEmail = AuthorEmail;
                controller.AddAuthor(NewAuthor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }


        /// <summary>
        /// Routes to a dynamically generated "Author Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Author</param>
        /// <returns>A dynamic "Update Author" webpage which provides the current information of the author and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Author/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Author SelectedAuthor = controller.FindAuthor(id);
                return View(SelectedAuthor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        /// <summary>
        /// Routes to a dynamically rendered "Ajax Update" Page. The "Ajax Update" page will utilize JavaScript to send an HTTP Request to the data access layer (/api/AuthorData/UpdateAuthor)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Ajax_Update(int id)
        {
            try
            {
                Author SelectedAuthor = controller.FindAuthor(id);
                return View(SelectedAuthor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        /// Receives a POST request containing information about an existing author in the system, with new values. Conveys this information to the API, and redirects to the "Author Show" page of our updated author.
        /// </summary>
        /// <param name="id">Id of the Author to update</param>
        /// <param name="AuthorFname">The updated first name of the author</param>
        /// <param name="AuthorLname">The updated last name of the author</param>
        /// <param name="AuthorBio">The updated bio of the author.</param>
        /// <param name="AuthorEmail">The updated email of the author.</param>
        /// <returns>A dynamic webpage which provides the current information of the author.</returns>
        /// <example>
        /// POST : /Author/Update/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"AuthorFname":"Christine",
        ///	"AuthorLname":"Bittle",
        ///	"AuthorBio":"Loves Coding!",
        ///	"AuthorEmail":"christine@test.ca"
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string AuthorFname, string AuthorLname, string AuthorBio, string AuthorEmail)
        {
            try
            {
                Author AuthorInfo = new Author();
                AuthorInfo.AuthorFname = AuthorFname;
                AuthorInfo.AuthorLname = AuthorLname;
                AuthorInfo.AuthorBio = AuthorBio;
                AuthorInfo.AuthorEmail = AuthorEmail;
                controller.UpdateAuthor(id, AuthorInfo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("Show/" + id);
        }

    }
}