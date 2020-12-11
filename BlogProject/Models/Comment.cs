using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class Comment
    {
        public int CommentId;
        public string CommentDesc;
        public int CommentRating;
        public DateTime CommentDate;
        //foreign key
        public int ArticleId;
        //foreign key
        public int AuthorId;

        //can store validation logic here
        //example in Author.cs
        public bool IsValid()
        {
            bool valid = true;
            return valid;
        }

        //paramter-less constructor function
        //used for auto-binding article properties in ajax call to CommentData Controller
        public Comment() { }
    }
}