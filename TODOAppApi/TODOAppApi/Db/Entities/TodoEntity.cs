namespace TODOAppApi.Db.Entities
{
    public class TodoEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
