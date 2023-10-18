using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.InterfaceService
{
    public interface IServiceTask
    {
        Task AddTask(TaskModel taskModel);

        Task UpdateTask(TaskModel taskModel);

        Task<List<TaskModel>> ListActiveTasks();
    }
}
