using System.Net;

namespace HttpFundamentals
{
    public class BasicHttpServer
    {
        public string Endpoint = "localhost";
        public int Port = 8888;

        private HttpListener _listener;

        public void Start()
        {
            string prefix = $"http://{Endpoint}:{Port}/";

            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            Receive();
            Console.WriteLine($"Listening on port {Port}...");
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);

                var request = context.Request;
                Console.WriteLine($"{request.Url}");

                TaskSolution solution = new TaskSolution();
                solution.ProsessRequest(context);

                Receive();
            }
        }
    }
}