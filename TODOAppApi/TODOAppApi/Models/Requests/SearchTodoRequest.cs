namespace TODOAppApi.Models.Requests
{
    public class SearchTodoRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
    }
}
