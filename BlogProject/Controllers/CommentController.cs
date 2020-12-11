using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogProject.Models;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class CommentController : Controller
    {

        //We can instantiate the Commentcontroller outside of each method
        private CommentDataController controller = new CommentDataController();

        //GET : /Comment/Error
        /// <summary>
        /// This window is for showing Comment Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        //GET : /Comment/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //Try to get a list of Comments.
                IEnumerable<Comment> Comments = controller.ListComments(SearchKey);
                return View(Comments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Comment/Show/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Comment SelectedComment = controller.FindComment(id);
                return View(SelectedComment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Comment/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Comment NewComment = controller.FindComment(id);
                return View(NewComment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Comment/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                controller.DeleteComment(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET : /Comment/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Comment/Create
        [HttpPost]
        public ActionResult Create(string CommentDesc, int CommentRating)
        {
            try
            {
                Comment NewComment = new Comment();
                NewComment.CommentDesc = CommentDesc;
                NewComment.CommentRating = CommentRating;

                controller.AddComment(NewComment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }


        /// <summary>
        /// Routes to a dynamically generated "Comment Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Comment</param>
        /// <returns>A dynamic "Update Comment" webpage which provides the current information of the Comment and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Comment/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Comment SelectedComment = controller.FindComment(id);
                return View(SelectedComment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Comment/Update
        [HttpPost]
        public ActionResult Update(int id, string CommentDesc, int CommentRating)
        {
            try
            {
                Comment NewComment = new Comment();
                NewComment.CommentDesc = CommentDesc;
                NewComment.CommentRating = CommentRating;

                controller.UpdateComment(id, NewComment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }

    }
}