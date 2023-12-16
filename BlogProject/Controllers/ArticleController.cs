using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogProject.Models;
using System.Diagnostics;
using BlogProject.Models.ViewModels;

namespace BlogProject.Controllers
{
    public class ArticleController : Controller
    {

        //We can instantiate the Articlecontroller outside of each method
        private ArticleDataController articlecontroller = new ArticleDataController();

        private TagDataController tagcontroller = new TagDataController();

        private CommentDataController commentcontroller = new CommentDataController();

        //GET : /Article/Error
        /// <summary>
        /// This window is for showing Article Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        //GET : /Article/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //Try to get a list of Articles.
                IEnumerable<Article> Articles = articlecontroller.ListArticles(SearchKey);
                return View(Articles);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Article/Show/{id}
        public ActionResult Show(int id)
        {
            try
            {
                ShowArticle ViewModel = new ShowArticle();

                Article SelectedArticle = articlecontroller.FindArticle(id);

                ViewModel.SelectedArticle = SelectedArticle;

                //The comments associated to the article

                IEnumerable<Comment> CommentsPosted = commentcontroller.ListCommentsForArticle(id);

                ViewModel.CommentsPosted = CommentsPosted;

                //which controller should we build ListTagsForArticle?
                ViewModel.TagsAssociated = tagcontroller.ListTagsForArticle(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Article/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Article NewArticle = articlecontroller.FindArticle(id);
                return View(NewArticle);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Article/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                articlecontroller.DeleteArticle(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET : /Article/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Article/Create
        [HttpPost]
        public ActionResult Create(string ArticleTitle, string ArticleBody)
        {
            try
            {
                Article NewArticle = new Article();
                NewArticle.ArticleTitle = ArticleTitle;
                NewArticle.ArticleBody = ArticleBody;

                articlecontroller.AddArticle(NewArticle);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }


        /// <summary>
        /// Routes to a dynamically generated "Article Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Article</param>
        /// <returns>A dynamic "Update Article" webpage which provides the current information of the Article and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Article/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Article SelectedArticle = articlecontroller.FindArticle(id);
                return View(SelectedArticle);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Article/Update
        [HttpPost]
        public ActionResult Update(int id, string ArticleTitle, string ArticleBody)
        {
            try
            {
                Article NewArticle = new Article();
                NewArticle.ArticleTitle = ArticleTitle;
                NewArticle.ArticleBody = ArticleBody;

                articlecontroller.UpdateArticle(id, NewArticle);
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