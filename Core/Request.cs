namespace Core
{
    public class Request
    {
        public string Method { get; set; }

        public string Header { get; set; }

        public object Body { get; set; }
    }
}
