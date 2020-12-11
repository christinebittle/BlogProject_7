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

namespace BlogProject.Controllers
{
    public class ArticleDataController : ApiController
    {

        //The database context class which allows us to access our MySQL Database.
        //AccessDatabase switched to a static method, one that can be called without an object.
        MySqlConnection Conn = BlogDbContext.AccessDatabase();


        //This Controller Will access the Articles table of our blog database. Non-Deterministic.
        /// <summary>
        /// Returns a list of Articles in the system
        /// </summary>
        /// <returns>
        /// A list of Article Objects with fields mapped to the database column values (first name, last name, bio).
        /// </returns>
        /// <example>GET api/ArticleData/ListArticles -> {Article Object, Article Object, Article Object...}</example>
        [HttpGet]
        [Route("api/ArticleData/ListArticles/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Article> ListArticles(string SearchKey = null)
        {
            //Create an empty list of Articles
            List<Article> Articles = new List<Article> { };
            try
            {
                //Try to open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Articles where lower(articletitle) like lower(@key) or lower(articlebody) like lower(@key)";

                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int ArticleId = Convert.ToInt32(ResultSet["articleid"]);
                    string ArticleTitle = ResultSet["articletitle"].ToString();
                    string ArticleBody = ResultSet["articlebody"].ToString();
                    DateTime ArticleDate = (DateTime)ResultSet["articledate"];


                    Article NewArticle = new Article();
                    NewArticle.ArticleId = ArticleId;
                    NewArticle.ArticleTitle = ArticleTitle;
                    NewArticle.ArticleBody = ArticleBody;
                    NewArticle.ArticleDate = ArticleDate;

                    //Add the Article Name to the List
                    Articles.Add(NewArticle);
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

            //Return the final list of Article names
            return Articles;


        }


        /// <summary>
        /// Finds an Article from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Article ID</param>
        /// <returns>Article object containing information about the Article with a matching ID. Empty Article Object if the ID does not match any Articles in the system.</returns>
        /// <example>api/ArticleData/FindArticle/6 -> {Article Object}</example>
        /// <example>api/ArticleData/FindArticle/10 -> {Article Object}</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Article FindArticle(int id)
        {
            Article NewArticle = new Article();

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from Articles where Articleid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int ArticleId = Convert.ToInt32(ResultSet["articleid"]);
                    string ArticleTitle = ResultSet["articletitle"].ToString();
                    string ArticleBody = ResultSet["articlebody"].ToString();
                    DateTime ArticleDate = (DateTime)ResultSet["articledate"];



                    NewArticle.ArticleId = ArticleId;
                    NewArticle.ArticleTitle = ArticleTitle;
                    NewArticle.ArticleBody = ArticleBody;
                    NewArticle.ArticleDate = ArticleDate;




                }
                //checking for model validity after pulling from the db
                if (!NewArticle.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);

            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That Article was not found.", ex);
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


            return NewArticle;
        }


        /// <summary>
        /// Deletes an Article from the connected MySQL Database if the ID of that Article exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the Article.</param>
        /// <example>POST /api/ArticleData/DeleteArticle/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteArticle(int id)
        {
            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from Articles where articleid=@id";
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
        /// Adds an Article to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewArticle">An object with fields that map to the columns of the Article's table. </param>
        /// <example>
        /// POST api/ArticleData/AddArticle 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ArticleTitle":"My Sound Adventure in Italy",
        ///	"ArticleBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddArticle([FromBody] Article NewArticle)
        {
            //Exit method if model fields are not included.
            if (!NewArticle.IsValid()) throw new ApplicationException("Posted Data was not valid.");

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "insert into Articles (articletitle, articlebody, articledate) values (@articletitle,@articlebody, current_date())";
                cmd.Parameters.AddWithValue("@articletitle", NewArticle.ArticleTitle);
                cmd.Parameters.AddWithValue("@articlebody", NewArticle.ArticleBody);


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
        /// Updates an Article on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="ArticleInfo">An object with fields that map to the columns of the Article's table.</param>
        /// <example>
        /// POST api/ArticleData/UpdateArticle/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ArticleTitle":"My Sound Adventure in Italy",
        ///	"ArticleBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateArticle(int id, [FromBody] Article ArticleInfo)
        {


            //Exit method if model fields are not included.
            if (!ArticleInfo.IsValid()) throw new ApplicationException("Posted Data was not valid.");

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Articles SET articletitle=@articletitle, articlebody=@articlebody WHERE articleid=@ArticleId";
                cmd.Parameters.AddWithValue("@articletitle", ArticleInfo.ArticleTitle);
                cmd.Parameters.AddWithValue("@articlebody", ArticleInfo.ArticleBody);
                cmd.Parameters.AddWithValue("@ArticleId", id);
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


    }
}
