using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestProject.Models;
using Task = TestProject.Models.Task;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        public TaskController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;TrustServerCertificate=True";
        
        [HttpGet]
        //[Route("api/tasks/{IdProject?}")]
        public IActionResult GetTask(int? IdProject = null)
        {
            string query = "";
            if (IdProject == null)
            {
                query = $"SELECT * FROM Task ORDER BY Task.Deadline ASC";
            }
            else
            {
                query = $"SELECT Task.Name, Task.Description, Task.Deadline, Task.IdProject, Task.IdTskType, Task.IdAssignedTo, Task.IdCreator," +
                        $" Project.Name, TeamMember.LastName, TaskType.Name" +
                        $" FROM Task" +
                        $" INNER JOIN Project ON Task.IdProject=Project.IdProject WHERE Project.IdProject = @IdProject" +
                        $" INNER JOIN TaskType ON Task.IdTaskType=TaskType.IdTaskType" +
                        $" INNER JOIN TeamMember ON Task.IdAssigedTo=TeamMember.IdTeamMeber" +
                        $" INNER JOIN TeamMember ON Task.IdCreator.IdTeamMeber" +
                        $" ORDER BY Task.Deadline ASC";
            }
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdProject", IdProject);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }
        
        
        [HttpPost]
        public IActionResult PostTask(Task task)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                var transaction = myCon.BeginTransaction();

                try
                {
                    string query = @"
           INSERT INTO Task (Name, Description, DeadLine, IdProject, IdTaskType, IdAssignedTo, IdCreator) 
           VALUES (@Name, @Description, @DeadLine, @IdProject, @IdTaskType, @IdAssignedTo, @IdCreator)";
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", task.Name);
                        myCommand.Parameters.AddWithValue("@Description", task.Description);
                        myCommand.Parameters.AddWithValue("@DeadLine", task.DeadLine);
                        myCommand.Parameters.AddWithValue("@IdProject", task.IdProject);
                        myCommand.Parameters.AddWithValue("@IdTaskType", task.IdTaskType);
                        myCommand.Parameters.AddWithValue("@IdAssignedTo", task.IdAssignedTo);
                        myCommand.Parameters.AddWithValue("@IdCreator", task.IdCreator);
                        myCommand.ExecuteNonQuery();
                    }
                } catch(Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"An error occured: {ex.Message}");
                }
                transaction.Commit();
                return new JsonResult("Added Successfully");
            }
        }
        
        
        [HttpDelete]
        public IActionResult DeleteTeamMember(Member member)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                var transaction = myCon.BeginTransaction();

                try
                {
                    string query = @"";
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.ExecuteNonQuery();
                    }
                } catch(Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"An error occured: {ex.Message}");
                }
                transaction.Commit();
                return new JsonResult("Added Successfully");
            }
        }
    }
}