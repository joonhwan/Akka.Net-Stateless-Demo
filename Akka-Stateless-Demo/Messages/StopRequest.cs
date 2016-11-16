namespace Akka_Stateless_Demo.Messages
{
    public class StopRequest
    {
        public StopRequest(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; private set; }

        public override string ToString()
        {
            return $"RequesterName: {Tag}";
        }
    }
}