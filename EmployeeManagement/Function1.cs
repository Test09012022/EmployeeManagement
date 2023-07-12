using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EmployeeCore.Model;
using EmployeeApplication.Interfaces;


namespace EmployeeManagement
{
    public  class Function1
    {
        //[FunctionName("Function1")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    string responseMessage = string.IsNullOrEmpty(name)
        //        ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
        //        : $"Hello, {name}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}



        //------------------------

        private readonly IUnitOfWork unitOfWork;
        private const string Route = "func";
        #region Constructor
        public Function1(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Function Get Employees
        /// <summary>
        /// Get List of Employees
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
          HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("Getting Employee list items");
                unitOfWork.Employees.GetAllAsync();
                return new OkObjectResult(await unitOfWork.Employees.GetAllAsync());

            }
            catch (System.Exception)
            {
                throw;
            }

        }
        #endregion

        #region Get Employee Based on Employee Id
        /// <summary>
        /// Get Employee by Querying with Id
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [FunctionName("GetEmployeebyId")]
        public async Task<IActionResult> GetEmployeebyId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{Id}")]
          HttpRequest req, ILogger log, int Id)
        {
            try
            {
                var result = await unitOfWork.Employees.GetByIdAsync(Id);
                if (result is null)
                {
                    log.LogInformation($"Item {Id} not found");
                    return new NotFoundResult();
                }
                return new OkObjectResult(result);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        #endregion

        #region Create Employee
        /// <summary>
        /// Create New Employee
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route +"/Create")]
          HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new employee list item");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Employee>(requestBody);
            var employee = new Employee { FirstName = input.FirstName, Surname = input.Surname, Email = input.Email, JobTitle = input.JobTitle};
            await unitOfWork.Employees.AddAsync(employee);
            return new OkObjectResult(new { Message = "Record Saved SuccessFully", Data = employee });
        }
        #endregion

        #region Update Employee
        /// <summary>
        /// Update Employee - Changes
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [FunctionName("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route +"/Update")]
          HttpRequest req, ILogger log)
        {
            log.LogInformation("Updating a new employee list item");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<Employee>(requestBody);
            var employee = await unitOfWork.Employees.GetByIdAsync(updated.EmployeeId);
            if (employee is null)
            {
                log.LogError($"Item {updated.EmployeeId} not found");
                return new NotFoundResult();
            }
         
           
            await  unitOfWork.Employees.UpdateAsync(updated);
            return new OkObjectResult(new { Message = "Record Updated SuccessFully", Data = employee });
        }
        #endregion

        #region Delete Employee
        /// <summary>
        /// Deletion of Employee
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [FunctionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteEmployee/{Id}")]
          HttpRequest req, ILogger log, int Id)
        {
            log.LogInformation("Updating a new employee list item");
            var employee = await unitOfWork.Employees.GetByIdAsync(Id);
            if (employee is null)
            {
                log.LogError($"Item {Id} not found");
                return new NotFoundResult();
            }
            await unitOfWork.Employees.DeleteAsync(Id);
          
            return new OkObjectResult("Record Deleted !");
        }
        #endregion




    }
}
