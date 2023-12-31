﻿using Domain.Interfaces.Generic;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITask : IGeneric<TaskModel>
    {
        Task<List<TaskModel>> ListTasks(Expression<Func<TaskModel, bool>> exTask);
    }
}