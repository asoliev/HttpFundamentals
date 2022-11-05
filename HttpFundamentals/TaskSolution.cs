using System.Net;
using System.Text;

namespace HttpFundamentals
{
    public class TaskSolution
    {
        private string[] task2_status_messages = new string[]
        {
            "Information",
            "Success",
            "Redirection",
            "ClientError",
            "ServerError"
        };
        private Dictionary<string, int> task2_resp;
        public TaskSolution()
        {
            task2_resp = new Dictionary<string, int>
            {
                [task2_status_messages[0]] = 100,
                [task2_status_messages[1]] = 200,
                [task2_status_messages[2]] = 300,
                [task2_status_messages[3]] = 400,
                [task2_status_messages[4]] = 500
            };
        }
        public void ProsessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            string? path = request.Url?.LocalPath;

            using HttpListenerResponse response = context.Response;

            if (string.IsNullOrEmpty(path))
                response.StatusCode = (int)HttpStatusCode.Accepted;

            string path_method = path!.Remove(0, 1);

            if (path == "/MyName") GetMyName(response);
            else if (path == "/MyNameByHeader") GetMyNameByHeader(response);
            else if (path == "/MyNameByCookies") MyNameByCookies(response);
            else if (task2_status_messages.Contains(path_method))
                Task2Response(response, path_method);
        }
        private void GetMyName(HttpListenerResponse response)
        {
            response.Headers.Set("Content-Type", "text/plain");

            string data = "My name is HttpFundamentals.";
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            response.ContentLength64 = buffer.Length;

            using Stream ros = response.OutputStream;
            ros.Write(buffer, 0, buffer.Length);
        }
        private void Task2Response(HttpListenerResponse response, string path_method)
        {
            response.StatusCode = task2_resp[path_method];
        }
        private void GetMyNameByHeader(HttpListenerResponse response)
        {
            response.Headers.Set("X-MyName", "HttpFundamentals");
        }
        private void MyNameByCookies(HttpListenerResponse response)
        {
            Cookie timeStampCookie = new Cookie("MyName", "HttpFundamentals");
            response.SetCookie(timeStampCookie);
        }
    }
}