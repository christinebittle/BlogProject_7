using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogProject.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using System.Diagnostics;
using System.Xml.Linq;

namespace BlogProject.Controllers
{
    public class CommentDataController : ApiController
    {

        //The database context class which allows us to access our MySQL Database.
        //AccessDatabase switched to a static method, one that can be called without an object.
        MySqlConnection Conn = BlogDbContext.AccessDatabase();


        //This Controller Will access the Comments table of our blog database. 
        /// <summary>
        /// Returns a list of Comments in the system
        /// </summary>
        /// <returns>
        /// A list of Comment Objects with fields mapped to the database column values (first name, last name, bio).
        /// </returns>
        /// <example>GET api/CommentData/ListComments -> {Comment Object, Comment Object, Comment Object...}</example>
        [HttpGet]
        [Route("api/CommentData/ListComments/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Comment> ListComments(string SearchKey = null)
        {
            //Create an empty list of Comments
            List<Comment> Comments = new List<Comment> { };
            try
            {
                //Try to open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Comments where lower(commentdesc) like lower(@key)";

                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int CommentId = Convert.ToInt32(ResultSet["Commentid"]);
                    string CommentDesc  = ResultSet["commentdesc"].ToString();
                    DateTime CommentDate = (DateTime)ResultSet["Commentdate"];
                    int CommentRating = Convert.ToInt32(ResultSet["CommentRating"]);


                    Comment NewComment = new Comment();
                    NewComment.CommentId = CommentId;
                    NewComment.CommentDesc = CommentDesc;
                    NewComment.CommentRating = CommentRating;
                    NewComment.CommentDate = CommentDate;

                    //Add the Comment Name to the List
                    Comments.Add(NewComment);
                }
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }

            //Return the final list of Comment names
            return Comments;


        }


        /// <summary>
        /// Returns a list of comments made by that author
        /// </summary>
        /// <param name="AuthorId">The primary key of the  author</param>
        /// <returns>
        /// A list of comment objects related to that author
        /// </returns>
        /// <example>
        /// GET api/CommentData/ListCommentsForAuthor/3 -> [{Comment Object},{Comment Object},..]
        /// </example>
        [HttpGet]
        [Route("api/CommentData/ListCommentsForAuthor/{AuthorId}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Comment> ListCommentsForAuthor(int AuthorId)
        {
            //Create an empty list of Comments
            List<Comment> Comments = new List<Comment> { };
            try
            {
                //Try to open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Comments where comments.authorid = @AuthorId";

                cmd.Parameters.AddWithValue("@AuthorId", @AuthorId);
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int CommentId = Convert.ToInt32(ResultSet["Commentid"]);
                    string CommentDesc = ResultSet["commentdesc"].ToString();
                    DateTime CommentDate = (DateTime)ResultSet["Commentdate"];
                    int CommentRating = Convert.ToInt32(ResultSet["CommentRating"]);


                    Comment NewComment = new Comment();
                    NewComment.CommentId = CommentId;
                    NewComment.CommentDesc = CommentDesc;
                    NewComment.CommentRating = CommentRating;
                    NewComment.CommentDate = CommentDate;

                    //Add the Comment Name to the List
                    Comments.Add(NewComment);
                }
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }

            //Return the final list of Comment names
            return Comments;


        }

        /// <summary>
        /// Finds an Comment from the MySQL Database through an id. 
        /// </summary>
        /// <param name="id">The Comment ID</param>
        /// <returns>Comment object containing information about the Comment with a matching ID. Empty Comment Object if the ID does not match any Comments in the system.</returns>
        /// <example>api/CommentData/FindComment/6 -> {Comment Object}</example>
        /// <example>api/CommentData/FindComment/10 -> {Comment Object}</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Comment FindComment(int id)
        {
            Comment NewComment = new Comment();

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from Comments where Commentid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int CommentId = Convert.ToInt32(ResultSet["Commentid"]);
                    string CommentDesc = ResultSet["commentdesc"].ToString();
                    DateTime CommentDate = (DateTime)ResultSet["Commentdate"];
                    int CommentRating = Convert.ToInt32(ResultSet["CommentRating"]);


                    
                    NewComment.CommentId = CommentId;
                    NewComment.CommentDesc = CommentDesc;
                    NewComment.CommentRating = CommentRating;
                    NewComment.CommentDate = CommentDate;




                }
                //checking for model validity after pulling from the db
                if (!NewComment.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);

            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That Comment was not found.", ex);
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }


            return NewComment;
        }


        /// <summary>
        /// Deletes an Comment from the connected MySQL Database if the ID of that Comment exists. Does NOT maintain relational integrity. 
        /// </summary>
        /// <param name="id">The ID of the Comment.</param>
        /// <example>POST /api/CommentData/DeleteComment/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteComment(int id)
        {
            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from Comments where Commentid=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }
        }

        /// <summary>
        /// Adds an Comment to the MySQL Database. 
        /// </summary>
        /// <param name="NewComment">An object with fields that map to the columns of the Comment's table. </param>
        /// <example>
        /// POST api/CommentData/AddComment 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"CommentTitle":"My Sound Adventure in Italy",
        ///	"CommentBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddComment([FromBody] Comment NewComment)
        {
            //Exit method if model fields are not included.
            if (!NewComment.IsValid()) return;

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "insert into Comments (CommentDesc, CommentRating) values (@commentdesc,@commentrating)";
                cmd.Parameters.AddWithValue("@commentdesc", NewComment.CommentDesc);
                cmd.Parameters.AddWithValue("@commentrating", NewComment.CommentRating);


                cmd.Prepare();

                cmd.ExecuteNonQuery();

                Conn.Close();
            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }



        }

        /// <summary>
        /// Updates an Comment on the MySQL Database. 
        /// </summary>
        /// <param name="CommentInfo">An object with fields that map to the columns of the Comment's table.</param>
        /// <example>
        /// POST api/CommentData/UpdateComment/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"CommentTitle":"My Sound Adventure in Italy",
        ///	"CommentBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateComment(int id, [FromBody] Comment CommentInfo)
        {


            //Exit method if model fields are not included.
            if (!CommentInfo.IsValid()) return;

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Comments SET commentdesc=@commentdesc, commentrating=@commentrating WHERE Commentid=@CommentId";
                cmd.Parameters.AddWithValue("@commentdesc", CommentInfo.CommentDesc);
                cmd.Parameters.AddWithValue("@commentrating", CommentInfo.CommentRating);
                cmd.Parameters.AddWithValue("@CommentId", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                //Catches issues with MySQL.
                Debug.WriteLine(ex);
                throw new ApplicationException("Issue was a database issue.", ex);
            }
            catch (Exception ex)
            {
                //Catches generic issues
                Debug.Write(ex);
                throw new ApplicationException("There was a server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

            }

        }

        /// <summary>
        /// Receives an article id and outputs the comments associated with an article
        /// </summary>
        /// <param name="articleid">The foreign key article id</param>
        /// <returns>
        /// comments associated with the article
        /// </returns>
        /// <example>
        /// GET api/CommentData/ListCommentsForArticle/5 => [{"CommentId":"1", "CommentDesc":"That was a great article"},{"CommentId":"2", "CommentDesc":"That was an okay article"}]
        /// </example>
        [HttpGet]
        [Route("api/CommentData/ListCommentsForArticle/{articleid}")]
        public IEnumerable<Comment> ListCommentsForArticle(int articleid)
        {
            Conn.Open();

            MySqlCommand Cmd = Conn.CreateCommand();

            string query = "select * from comments where articleid=@id";

            Cmd.CommandText = query;

            Cmd.Parameters.AddWithValue("@id", articleid);

            Cmd.Prepare();

            //Gather Result Set of Query into a variable

            MySqlDataReader ResultSet = Cmd.ExecuteReader();

            List<Comment> Comments = new List<Comment>();

            //Loop Through Each Row the Result Set               
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int CommentId = Convert.ToInt32(ResultSet["Commentid"]);
                string CommentDesc = ResultSet["commentdesc"].ToString();
                DateTime CommentDate = (DateTime)ResultSet["Commentdate"];
                int CommentRating = Convert.ToInt32(ResultSet["CommentRating"]);


                Comment NewComment = new Comment();
                NewComment.CommentId = CommentId;
                NewComment.CommentDesc = CommentDesc;
                NewComment.CommentRating = CommentRating;
                NewComment.CommentDate = CommentDate;

                //Add the Comment Name to the List
                Comments.Add(NewComment);
            }

            Conn.Close();


            return Comments;

        }


    }
}
