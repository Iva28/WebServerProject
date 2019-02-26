namespace WebServerProject.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }

        public ToDoItem(int id, string task)
        {
            this.Id = id;
            this.Name = task;
            this.State = " ";
        }
    }
}
