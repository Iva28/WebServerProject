using System.Collections.Generic;
using System.Linq;
using WebServerProject.Models;

namespace WebServerProject.Services
{
    class ToDoService : IToDoService
    {
        private static Dictionary<int, List<ToDoItem>> todoItems = new Dictionary<int, List<ToDoItem>>();

        static ToDoService()
        {
            var list = new List<ToDoItem>();
            list.Add(new ToDoItem(1, "admin_todo1"));
            list.Add(new ToDoItem(2, "admin_todo2"));
            list.Add(new ToDoItem(3, "admin_todo3"));
            todoItems.Add(1, list);

            var list2 = new List<ToDoItem>();
            list2.Add(new ToDoItem(1, "user_todo1"));
            list2.Add(new ToDoItem(2, "user_todo2"));
            list2.Add(new ToDoItem(3, "user_todo3"));
            todoItems.Add(2, list2);
        }

        public void Add(string todoName, int user)
        {
            var ts = todoItems.FirstOrDefault(t => t.Key == user).Value;
            var last = ts.LastOrDefault();
            ts.Add(new ToDoItem(last.Id + 1, todoName));
        }

        public void Change(int index, string state, int user)
        {
            var ts = todoItems.FirstOrDefault(t => t.Key == user).Value;
            if (state == "on")
                ts[index].State = "checked";
            else
                ts[index].State = " ";
        }

        public List<ToDoItem> GetToDoList(int user)
        {
            return todoItems.FirstOrDefault(t => t.Key == user).Value;
        }
    }
}
