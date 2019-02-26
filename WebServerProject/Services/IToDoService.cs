using System.Collections.Generic;
using WebServerProject.Models;

namespace WebServerProject.Services
{
    interface IToDoService
    {
        List<ToDoItem> GetToDoList(int user);
        void Add(string todoName, int user);
        void Change(int index, string state, int user);
    }
}
