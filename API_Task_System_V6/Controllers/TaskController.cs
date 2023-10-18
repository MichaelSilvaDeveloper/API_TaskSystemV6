using API_Task_System_V6.Models;
using Domain.Interfaces;
using Domain.InterfaceService;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Task_System_V6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask _iTask;

        private readonly IServiceTask _iServiceTask;

        public TaskController(ITask iTask, IServiceTask iServiceTask)
        {
            _iTask = iTask;
            _iServiceTask = iServiceTask;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/AddTask")]
        public async Task AddTask(TaskViewModel taskViewModel)
        {
            var newTask = new TaskModel
            {
                Title = taskViewModel.Title,
                Information = taskViewModel.Information,
                UserId = await ReturnUserIdLogged()
            };
            await _iServiceTask.AddTask(newTask);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/UpdateTask")]
        public async Task UpdateTask(TaskViewModel taskViewModel)
        {
            var getTaskById = await _iTask.GetById(taskViewModel.Id);
            getTaskById.Title = taskViewModel.Title;
            getTaskById.Information = taskViewModel.Information;
            getTaskById.UserId = await ReturnUserIdLogged();
            await _iServiceTask.UpdateTask(getTaskById);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/RemoveTask")]
        public async Task RemoveTask(TaskViewModel taskViewModel)
        {
            var getTaskById = await _iTask.GetById(taskViewModel.Id);
            await _iTask.Remove(getTaskById);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/GetTaskById")]
        public async Task<TaskModel> GetTaskById(TaskViewModel taskViewModel)
        {
            var getTaskById = await _iTask.GetById(taskViewModel.Id);
            return getTaskById;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/ListAllTaskActive")]
        public async Task<List<TaskModel>> ListAllTaskActive()
        {
            return await _iServiceTask.ListActiveTasks();
        }     

        private async Task<string> ReturnUserIdLogged()
        {
            if(User != null)
            {
                var idUser = User.FindFirst("idUser");
                return idUser.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}