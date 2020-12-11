using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models.ViewModels
{
    public class ShowAuthor
    {
        //Defines all the metadata needed to show an author
        //The author themself
        public Author Author;

        //The articles they write
        public IEnumerable<Article> ArticlesWritten;

        //The comments they write
        public IEnumerable<Comment> CommentsWritten;
    }
}