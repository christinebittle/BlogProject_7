using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models.ViewModels
{
    public class ShowArticle
    {

        //the article to view
        public Article SelectedArticle { get; set; }

        //comments posted on the article

        public IEnumerable<Comment> CommentsPosted { get; set; }

        //tags associated with the article

        public IEnumerable<Tag> TagsAssociated { get; set; }
    }
}