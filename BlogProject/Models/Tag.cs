using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogProject.Models
{
    public class Tag
    {
        public int TagId;
        public string TagColor;

        //can store validation logic here
        //example in Author.cs
        public bool IsValid()
        {
            bool valid = true;
            return valid;
        }
        public Tag() { }
    }
}