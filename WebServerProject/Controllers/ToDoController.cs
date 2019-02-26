using WebServerProject.Server.Attributes;
using WebServerProject.Services;

namespace WebServerProject.Controllers
{
    class ToDoController
    {
        private IToDoService todoService;

        public ToDoController(IToDoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpMethod("GET")]
        public string All()
        {
            string str = "<ol style='padding-left:20px;'>";
            int id = 0;
            foreach (var item in todoService.GetToDoList((int)AccountService.currentAccountID))
            {
                str += $"<li style='margin:5px;'><label style='margin-right:5px;'>{item.Name}</label>" +
                     $"<form style='display:inline;' method='POST' action='http://127.0.0.1:5600/toDo/check'>" +
                     $"<input type='checkbox' name='check' {item.State}> <input type='hidden' name='id' value={id}> <input type='submit'> </form> </li>";
                id++;
            }
            str += $"</ol>" +
                $"<form method='POST' action='http://127.0.0.1:5600/toDo/add' style='margin-left:8px;'><label>ToDo: </label>" +
                $"<input type='text' name='todoName' required><input type='submit' value='Add'></form>" +
                $"<a href='http://127.0.0.1:5600/account/login' style='margin-left:8px;font-size:15px;color:black;'>Exit</a>";

            return str;
        }

        [Authorize("Admin")]
        [HttpMethod("POST")]
        public string Add(string todoName)
        {
            todoService.Add(todoName, (int)AccountService.currentAccountID);
            return $"<script>window.location = 'http://127.0.0.1:5600/toDo/all'</script>";
        }

        [Authorize("Admin")]
        [HttpMethod("POST")]
        public string Check(string check, int id)
        {
            todoService.Change(id, check, (int)AccountService.currentAccountID);
            return $"<script>window.location = 'http://127.0.0.1:5600/toDo/all'</script>";
        }
    }
}
