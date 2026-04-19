namespace beheersysteem_uitvaartcentrum.backend.application.Exceptions
{
    public class NotFoundForeignKey : Exception
    {
        public string Title { get; set; }

        public NotFoundForeignKey(string title, string message) : base(message) { 
            Title = title;
        }
    }
}
