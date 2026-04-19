namespace beheersysteem_uitvaartcentrum.backend.application.Exceptions
{
    public class NotAllowedFileExtension : Exception
    {
        public string Title { get; set; }

        public NotAllowedFileExtension(string title, string message) : base(message)
        {
            Title = title;
        }
    }
}
