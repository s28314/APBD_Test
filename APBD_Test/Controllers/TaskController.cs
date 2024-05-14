using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

 namespace APBD_Test.Controllers
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
        public IActionResult GetTasks(int? IdProject = null)
        {
            string query = "";
            if (IdProject == null)
            {
                query = $"SELECT * FROM Task ORDER BY Deadline ASC";
            }
            else
            {
                query = $"SELECT Task.Name, Task.Description, Task.Deadline, Task.IdProject, Task.IdTskType, Task.IdAssignedTo, Task.IdCreator," +
                        $" Project.Name, TeamMember.LastName, TaskType.Name" +
                        $" FROM Task" +
                        $" INNER JOIN Project ON Task.IdProject=Project.IdProject" +
                        $" INNER JOIN TaskType ON Task.IdTaskType=TaskType.IdTaskType" +
                        $" INNER JOIN TeamMember ON Task.IdAssigedTo=TeamMember.IdTeamMeber" +
                        $" INNER JOIN TeamMember ON Task.IdCreator.IdTeamMeber";
            }
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //myCommand.Parameters.AddWithValue("@orderBy", orderBy);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }


    }
}