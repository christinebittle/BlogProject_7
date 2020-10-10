using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class Author
    {
        //The following fields define an Author
        public int AuthorId;
        public string AuthorFname;
        public string AuthorLname;
        public string AuthorBio;
        public DateTime AuthorJoinDate;
        public string AuthorEmail;

        //parameter-less constructor function
        public Author() { }
    }
}