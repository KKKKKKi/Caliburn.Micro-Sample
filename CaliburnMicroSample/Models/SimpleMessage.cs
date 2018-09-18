namespace CaliburnMicroSample.Models
{
    public class SimpleMessage
    {
        public object Sender { get; }
        public string Content { get; }

        public SimpleMessage(object sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
