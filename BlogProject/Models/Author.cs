using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;


namespace BlogProject.Models
{
    public class Author
    {
        //Server-Side Validation logic can occur in many places.
        //The Model is a good place to store constraints on data, as it is meant to act as a representation.
        //This technique is refined in ASP.NET, and utilizes "Data Annotations"
        //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netframework-4.7
        

        public int AuthorId;
        public string AuthorFname;
        public string AuthorLname;
        public string AuthorBio;
        public DateTime AuthorJoinDate;
        public string AuthorEmail;

        public bool IsValid()
        {
            bool valid = true;

            if (AuthorFname == null || AuthorLname == null || AuthorEmail == null)
            {
                //Base validation to check if the fields are entered.
                valid = false;
            }
            else 
            { 
                //Validation for fields to make sure they meet server constraints
                if (AuthorFname.Length <= 2 || AuthorFname.Length > 255) valid = false;
                if (AuthorLname.Length <= 2 || AuthorFname.Length > 255) valid = false;
                //C# email regex 
                //https://stackoverflow.com/questions/5342375/regex-email-validation
                Regex Email = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
                if (!Email.IsMatch(AuthorEmail)) valid = false;
            }


            return valid;
        }

        //Parameter-less constructor function
        //Necissary for AJAX requests to automatically bind from the [FromBody] attribute
        public Author() { }
    }
}