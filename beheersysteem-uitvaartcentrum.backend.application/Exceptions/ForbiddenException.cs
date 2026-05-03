namespace beheersysteem_uitvaartcentrum.backend.application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public string Title { get; set; }

        public ForbiddenException(string title, string message) : base(message)
        {
            Title = title;
        }
    }
}
