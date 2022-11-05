using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientApp
{
    public class ClientActions
    {
        private string url = "http://localhost:8888";
        public void Process()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            using var client = new HttpClient(handler);

            #region input
            Console.Write("Enter task number (range 1-4): ");
            string? input = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(input) || input.Length > 1)
                RetryInput();

            bool is_num = byte.TryParse(input, out byte task_num);
            if (!is_num) RetryInput();
            if (task_num < 1 || task_num > 5) RetryInput();
            #endregion

            if (task_num == 1) Task1_CallMyName(client);
            else if (task_num == 2) Task2(client);
            else if (task_num == 3) Task3_CallMyNameByHeader(client);
            else if (task_num == 4) Task4_CallMyNameByCookies(client, cookies);
            else if (task_num == 5)
            {
                Task1_CallMyName(client);
                Console.WriteLine();
                Task2(client);
                Console.WriteLine();
                Task3_CallMyNameByHeader(client);
                Console.WriteLine();
                Task4_CallMyNameByCookies(client, cookies);
            }
        }

        private void Task1_CallMyName(HttpClient client)
        {
            string method = url + "/MyName";
            Console.WriteLine("call " + method);
            try
            {
                string result = client.GetStringAsync(method).Result;

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Task2(HttpClient client)
        {
            var methods = new string[]
            {
                //"Information",
                "Success",
                "Redirection",
                "ClientError",
                "ServerError"
            };
            foreach (string m in methods)
            {
                string method = $"{url}/{m}";
                Console.WriteLine("call " + method);

                var result = CallHttpGet(client, method);
                if (result == null) return;

                Console.WriteLine($"{(int)result!.StatusCode} - {result.StatusCode}");
                Console.WriteLine();
            }
        }
        private void Task3_CallMyNameByHeader(HttpClient client)
        {
            string method = url + "/MyNameByHeader";
            Console.WriteLine("call " + method);

            var result = CallHttpGet(client, method);
            if (result == null) return;

            if (result.Headers.Contains("X-MyName"))
                Console.WriteLine(result.Headers.GetValues("X-MyName").FirstOrDefault());
            else Console.WriteLine("Not expected response");
        }
        private void Task4_CallMyNameByCookies(HttpClient client, CookieContainer cookies)
        {
            string method = url + "/MyNameByCookies";
            Console.WriteLine("call " + method);

            var result = CallHttpGet(client, method);
            if (result == null) return;

            var uri = new Uri(url);
            var cookie = cookies.GetCookies(uri)
                .Cast<Cookie>()
                .FirstOrDefault(x => x.Name == "MyName");
            var value = cookie?.Value;

            if (value == null) Console.WriteLine("Not expected response");
            else Console.WriteLine(value);
        }

        private HttpResponseMessage? CallHttpGet(HttpClient client, string method)
        {
            try
            {
                return client.GetAsync(method).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        private void RetryInput()
        {
            Console.WriteLine("wrong input");
            Process();
        }
    }
}