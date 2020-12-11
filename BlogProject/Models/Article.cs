using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class Article
    {

        //primary key
        public int ArticleId;
        public string ArticleTitle;
        public string ArticleBody;
        public DateTime ArticleDate;
        //foreign key
        public int AuthorId;

        //can Execute Server Validation Logic here
        //see Author.cs as an example
        public bool IsValid() 
        {
            return true;
        }

        //paramter-less constructor function
        //used for auto-binding article properties in ajax call to ArticleData Controller
        public Article() { }
    }
}