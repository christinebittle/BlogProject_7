using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogProject.Models;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class TagController : Controller
    {

        //We can instantiate the Tagcontroller outside of each method
        private TagDataController controller = new TagDataController();

        //GET : /Tag/Error
        /// <summary>
        /// This window is for showing Tag Specific Errors!
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        //GET : /Tag/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //Try to get a list of Tags.
                IEnumerable<Tag> Tags = controller.ListTags(SearchKey);
                return View(Tags);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                //Debug.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Tag/Show/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Tag SelectedTag = controller.FindTag(id);
                return View(SelectedTag);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        //GET : /Tag/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Tag NewTag = controller.FindTag(id);
                return View(NewTag);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Tag/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                controller.DeleteTag(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET : /Tag/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Tag/Create
        [HttpPost]
        public ActionResult Create(string TagName, string TagColor)
        {
            try
            {
                Tag NewTag = new Tag();
                NewTag.TagName = TagName;
                NewTag.TagColor = TagColor;

                controller.AddTag(NewTag);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


            return RedirectToAction("List");
        }


        /// <summary>
        /// Routes to a dynamically generated "Tag Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Tag</param>
        /// <returns>A dynamic "Update Tag" webpage which provides the current information of the Tag and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Tag/Update/5</example>
        public ActionResult Update(int id)
        {
            try
            {
                Tag SelectedTag = controller.FindTag(id);
                return View(SelectedTag);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //POST : /Tag/Update
        [HttpPost]
        public ActionResult Update(int id, string TagName, string TagColor)
        {
            try
            {
                Tag NewTag = new Tag();
                NewTag.TagName = TagName;
                NewTag.TagColor = TagColor;

                controller.UpdateTag(id, NewTag);
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