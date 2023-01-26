namespace TODOAppApi.Models.Requests
{
    public class CreateTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
    }
}
