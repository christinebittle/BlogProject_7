using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Web.Http.Cors;

namespace BlogProject.Controllers
{
    public class AuthorDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private BlogDbContext Blog = new BlogDbContext();
        
        //This Controller Will access the authors table of our blog database. Non-Deterministic.
        /// <summary>
        /// Returns a list of Authors in the system
        /// </summary>
        /// <returns>
        /// A list of Author Objects with fields mapped to the database column values (first name, last name, bio).
        /// </returns>
        /// <example>GET api/AuthorData/ListAuthors -> {Author Object, Author Object, Author Object...}</example>
        [HttpGet]
        [Route("api/AuthorData/ListAuthors/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Author> ListAuthors(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Authors where lower(authorfname) like lower(@key) or lower(authorlname) like lower(@key) or lower(concat(authorfname, ' ', authorlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Authors
            List<Author> Authors = new List<Author>{};

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int AuthorId = (int)ResultSet["authorid"];
                string AuthorFname = ResultSet["authorfname"].ToString();
                string AuthorLname = ResultSet["authorlname"].ToString();
                string AuthorBio = ResultSet["authorbio"].ToString();

                Author NewAuthor = new Author();
                NewAuthor.AuthorId = AuthorId;
                NewAuthor.AuthorFname = AuthorFname;
                NewAuthor.AuthorLname = AuthorLname;
                NewAuthor.AuthorBio = AuthorBio;

                //Add the Author Name to the List
                Authors.Add(NewAuthor);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Authors;
        }


        /// <summary>
        /// Finds an author from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Author ID</param>
        /// <returns>Author object containing information about the author with a matching ID. Empty Author Object if the ID does not match any authors in the system.</returns>
        /// <example>api/AuthorData/FindAuthor/6 -> {Author Object}</example>
        /// <example>api/AuthorData/FindAuthor/10 -> {Author Object}</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Author FindAuthor(int id)
        {
            Author NewAuthor = new Author();

            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Authors where authorid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int AuthorId = (int)ResultSet["authorid"];
                string AuthorFname = ResultSet["authorfname"].ToString();
                string AuthorLname = ResultSet["authorlname"].ToString();
                string AuthorBio = ResultSet["authorbio"].ToString();
                string AuthorEmail = ResultSet["authoremail"].ToString();
                DateTime AuthorJoinDate = (DateTime)ResultSet["authorjoindate"];

                NewAuthor.AuthorId = AuthorId;
                NewAuthor.AuthorFname = AuthorFname;
                NewAuthor.AuthorLname = AuthorLname;
                NewAuthor.AuthorBio = AuthorBio;
                NewAuthor.AuthorEmail = AuthorEmail;
                NewAuthor.AuthorJoinDate = AuthorJoinDate;
            }
            Conn.Close();

            return NewAuthor;
        }


        /// <summary>
        /// Deletes an Author from the connected MySQL Database if the ID of that author exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <example>POST /api/AuthorData/DeleteAuthor/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteAuthor(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from authors where authorid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

        /// <summary>
        /// Adds an Author to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewAuthor">An object with fields that map to the columns of the author's table. </param>
        /// <example>
        /// POST api/AuthorData/AddAuthor 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"AuthorFname":"Christine",
        ///	"AuthorLname":"Bittle",
        ///	"AuthorBio":"Likes Coding!",
        ///	"AuthorEmail":"christine@test.ca"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddAuthor([FromBody]Author NewAuthor)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            Debug.WriteLine(NewAuthor.AuthorFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into authors (authorfname, authorlname, authorbio, authorjoindate, authoremail) values (@AuthorFname,@AuthorLname,@AuthorBio, CURRENT_DATE(), @AuthorEmail)";
            cmd.Parameters.AddWithValue("@AuthorFname", NewAuthor.AuthorFname);
            cmd.Parameters.AddWithValue("@AuthorLname", NewAuthor.AuthorLname);
            cmd.Parameters.AddWithValue("@AuthorBio", NewAuthor.AuthorBio);
            cmd.Parameters.AddWithValue("@AuthorEmail", NewAuthor.AuthorEmail);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }

        /// <summary>
        /// Updates an Author on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="AuthorInfo">An object with fields that map to the columns of the author's table.</param>
        /// <example>
        /// POST api/AuthorData/UpdateAuthor/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"AuthorFname":"Christine",
        ///	"AuthorLname":"Bittle",
        ///	"AuthorBio":"Likes Coding!",
        ///	"AuthorEmail":"christine@test.ca"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateAuthor(int id, [FromBody]Author AuthorInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Debug.WriteLine(AuthorInfo.AuthorFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update authors set authorfname=@AuthorFname, authorlname=@AuthorLname, authorbio=@AuthorBio, authoremail=@AuthorEmail  where authorid=@AuthorId";
            cmd.Parameters.AddWithValue("@AuthorFname", AuthorInfo.AuthorFname);
            cmd.Parameters.AddWithValue("@AuthorLname", AuthorInfo.AuthorLname);
            cmd.Parameters.AddWithValue("@AuthorBio", AuthorInfo.AuthorBio);
            cmd.Parameters.AddWithValue("@AuthorEmail", AuthorInfo.AuthorEmail);
            cmd.Parameters.AddWithValue("@AuthorId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }
}
