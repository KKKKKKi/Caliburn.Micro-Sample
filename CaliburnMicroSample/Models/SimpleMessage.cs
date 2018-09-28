namespace CaliburnMicroSample.Models
{
    public class SimpleMessage
    {
        public object Sender { get; }

        public string Title { get; }
        public string Content { get; }

        public SimpleMessage(object sender, string title, string content)
        {
            Sender = sender;
            Title = title;
            Content = content;
        }
    }
}
