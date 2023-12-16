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
    public class TagDataController : ApiController
    {

        //The database context class which allows us to access our MySQL Database.
        //AccessDatabase switched to a static method, one that can be called without an object.
        MySqlConnection Conn = BlogDbContext.AccessDatabase();


        //This Controller Will access the Tags table of our blog database. 
        /// <summary>
        /// Returns a list of Tags in the system
        /// </summary>
        /// <returns>
        /// A list of Tag Objects with fields mapped to the database column values (first name, last name, bio).
        /// </returns>
        /// <example>GET api/TagData/ListTags -> {Tag Object, Tag Object, Tag Object...}</example>
        [HttpGet]
        [Route("api/TagData/ListTags/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Tag> ListTags(string SearchKey = null)
        {
            //Create an empty list of Tags
            List<Tag> Tags = new List<Tag> { };
            try
            {
                //Try to open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "SELECT * from Tags where lower(tagname) like lower(@key)";

                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable

                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set               
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int TagId = Convert.ToInt32(ResultSet["Tagid"]);
                    string TagName = ResultSet["tagname"].ToString();
                    string TagColor = ResultSet["tagcolor"].ToString();


                    Tag NewTag = new Tag();
                    NewTag.TagId = TagId;
                    NewTag.TagName = TagName;
                    NewTag.TagColor = TagColor;

                    //Add the Tag Name to the List
                    Tags.Add(NewTag);
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

            //Return the final list of Tag names
            return Tags;


        }


        /// <summary>
        /// Finds an Tag from the MySQL Database through an id. 
        /// </summary>
        /// <param name="id">The Tag ID</param>
        /// <returns>Tag object containing information about the Tag with a matching ID. Empty Tag Object if the ID does not match any Tags in the system.</returns>
        /// <example>api/TagData/FindTag/6 -> {Tag Object}</example>
        /// <example>api/TagData/FindTag/10 -> {Tag Object}</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Tag FindTag(int id)
        {
            Tag NewTag = new Tag();

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from Tags where Tagid = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                while (ResultSet.Read())
                {
                    int TagId = Convert.ToInt32(ResultSet["Tagid"]);
                    string TagName = ResultSet["tagname"].ToString();
                    string TagColor = ResultSet["tagcolor"].ToString();


                    NewTag.TagId = TagId;
                    NewTag.TagName = TagName;
                    NewTag.TagColor = TagColor;




                }
                //checking for model validity after pulling from the db
                if (!NewTag.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);

            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("That Tag was not found.", ex);
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


            return NewTag;
        }


        /// <summary>
        /// Deletes an Tag from the connected MySQL Database if the ID of that Tag exists. Does NOT maintain relational integrity. 
        /// </summary>
        /// <param name="id">The ID of the Tag.</param>
        /// <example>POST /api/TagData/DeleteTag/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteTag(int id)
        {
            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from Tags where Tagid=@id";
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
        /// Adds an Tag to the MySQL Database. 
        /// </summary>
        /// <param name="NewTag">An object with fields that map to the columns of the Tag's table. </param>
        /// <example>
        /// POST api/TagData/AddTag 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TagTitle":"My Sound Adventure in Italy",
        ///	"TagBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTag([FromBody] Tag NewTag)
        {
            //Exit method if model fields are not included.
            if (!NewTag.IsValid()) return;

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "insert into Tags (tagname, tagcolor) values (@tagname,@tagcolor)";
                cmd.Parameters.AddWithValue("@Tagdesc", NewTag.TagName);
                cmd.Parameters.AddWithValue("@Tagrating", NewTag.TagColor);


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
        /// Updates an Tag on the MySQL Database. 
        /// </summary>
        /// <param name="TagInfo">An object with fields that map to the columns of the Tag's table.</param>
        /// <example>
        /// POST api/TagData/UpdateTag/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TagTitle":"My Sound Adventure in Italy",
        ///	"TagBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTag(int id, [FromBody] Tag TagInfo)
        {


            //Exit method if model fields are not included.
            if (!TagInfo.IsValid()) return;

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Tags SET tagname=@tagname, tagcolor=@tagcolor WHERE Tagid=@TagId";
                cmd.Parameters.AddWithValue("@tagname", TagInfo.TagName);
                cmd.Parameters.AddWithValue("@tagcolor", TagInfo.TagColor);
                cmd.Parameters.AddWithValue("@TagId", id);
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
        /// Lists the tags associated to a particular article
        /// </summary>
        /// <param name="articleid">the article id</param>
        /// <returns>all tags associated with that article</returns>
        /// <example>
        /// GET api/tagdata/listtagsforarticle/10 ->
        /// [{"TagId":1,"TagColor":"#ffffff","TagName":"LifeStyle"},{"TagId":2,"TagColor":"#ffff0f","TagName":"Adventure"}]

        /// </example> 
        [HttpGet]
        [Route("api/Tagdata/ListTagsForArticle/{articleid}")]
        public IEnumerable<Tag> ListTagsForArticle(int articleid)
        {
            string query = "select tags.tagid, tagname, tagcolor from articlesxtags inner join tags on articlesxtags.tagid=tags.tagid where articleid=@id";

            //Try to open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", articleid);
            cmd.Prepare();

            //Gather Result Set of Query into a variable

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Tag> Tags = new List<Tag>();

            //Loop Through Each Row the Result Set               
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TagId = Convert.ToInt32(ResultSet["Tagid"]);
                string TagName = ResultSet["tagname"].ToString();
                string TagColor = ResultSet["tagcolor"].ToString();


                Tag NewTag = new Tag();
                NewTag.TagId = TagId;
                NewTag.TagName = TagName;
                NewTag.TagColor = TagColor;

                //Add the Tag Name to the List
                Tags.Add(NewTag);
            }

            Conn.Close();

            return Tags;

        }


    }
}
