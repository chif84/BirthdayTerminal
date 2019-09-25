using ELMA.BirthdayTerminal.Services;
using System.IO;
using System.Net;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(ConnectionService))]
namespace ELMA.BirthdayTerminal.Services
{

    /// <summary>
    /// Сервис соединения с ELMA
    /// </summary>
    internal class ConnectionService : IConnectionService
    {
        private readonly string applicationToken = "285C8352AA7C67BFF882E4F236DECF51098C141AFB33A2AA4F7B34B4B3CEEF5DA30C848591DA55D5226C5D8D2C36432B12A5EF86C3D2EDF7E7C5781EC9D4E14A";

        /// <summary>
        /// Отправить Post-запрос
        /// </summary>
        public string SendPost(string path, string content, string authToken = null)
        {
            if (!path.StartsWith("http"))
            {
                path = @"http://" + path;
            }

            var request = WebRequest.Create(path);
            request.Method = "POST";

            string postData = content ?? string.Empty;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json;";
            request.Headers.Add("ApplicationToken", applicationToken);
            request.Headers.Add("WebData-Version", "2.0");

            if (authToken != null)
            {
                request.Headers.Add("AuthToken", authToken);
            }

            request.ContentLength = byteArray.Length;

            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var response = request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Отправить Get-запрос
        /// </summary>
        public string SendGet(string path, string authToken)
        {
            if (!path.StartsWith("http"))
            {
                path = @"http://" + path;
            }

            var request = WebRequest.Create(path);
            request.Method = "GET";

            request.ContentType = "application/json;";
            request.Headers.Add("ApplicationToken", applicationToken);
            request.Headers.Add("WebData-Version", "2.0");

            if (authToken != null)
            {
                request.Headers.Add("AuthToken", authToken);
            }

            using (var response = request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

        }

        /// <summary>
        /// Отправить Get-запрос
        /// </summary>
        public byte[] SendGet2(string path, string authToken)
        {
            if (!path.StartsWith("http"))
            {
                path = @"http://" + path;
            }

            var request = WebRequest.Create(path);
            request.Method = "GET";

            request.ContentType = "application/json;";
            request.Headers.Add("ApplicationToken", applicationToken);
            request.Headers.Add("WebData-Version", "2.0");

            if (authToken != null)
            {
                request.Headers.Add("AuthToken", authToken);
            }

            using (var response = request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        dataStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }

        }

    }
}
