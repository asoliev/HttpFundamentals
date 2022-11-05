using System.Net;

namespace HttpFundamentals
{
    public class MyHttpListener
    {
        public void Process()
        {
            using var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();
            Console.WriteLine("Listening on port 8888...");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                TaskSolution solution = new TaskSolution();
                solution.ProsessRequest(context);
            }
        }
    }
}