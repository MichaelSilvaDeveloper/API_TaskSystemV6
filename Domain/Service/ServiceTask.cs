using Domain.Interfaces;
using Domain.InterfaceService;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class ServiceTask : IServiceTask
    {
        private readonly ITask _iTask;

        public ServiceTask(ITask iTask)
        {
            _iTask = iTask;
        }

        public async Task AddTask(TaskModel taskModel)
        {
            taskModel.RegistrationDate = DateTime.Now;
            taskModel.ChangeDate = DateTime.Now;
            taskModel.Active = true;
            await _iTask.Add(taskModel);
        }

        public async Task UpdateTask(TaskModel taskModel)
        {
            taskModel.ChangeDate = DateTime.Now;
            taskModel.Active = true;
            await _iTask.Update(taskModel);        
        }

        public Task<List<TaskModel>> ListActiveTasks()
        {
            return _iTask.ListTasks(x => x.Active);
        }
    }
}