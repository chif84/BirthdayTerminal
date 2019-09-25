using System;
using System.Net;
using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Models.Elma;
using ELMA.BirthdayTerminal.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SecurityService))]
namespace ELMA.BirthdayTerminal.Services
{
    public interface ISecurityService
    {
        string CheckConnection(GlobalSettings settings);

        void ResetConnection();

        string AuthToken { get; }
    }

    public class SecurityService : ISecurityService
    {
        private ISerializationService serializationService;
        private IConnectionService connectionService;

        private LoginWithResponse loginWith;

        public SecurityService()
        {
            serializationService = DependencyService.Get<ISerializationService>();
            connectionService = DependencyService.Get<IConnectionService>();
        }

        public string CheckConnection(GlobalSettings settings)
        {
            if (loginWith != null && CheckToken(settings.Host, loginWith))
            {
                return string.Empty;
            }

            return LoginWith(settings);
        }

        public async void ResetConnection()
        {
            loginWith = null;
        }

        public string AuthToken { get { return loginWith?.AuthToken; } }

        private bool CheckToken(string host, LoginWithResponse loginWith)
        {
            if (loginWith == null)
                return false;

            var path = host + @"/API/REST/Authorization/CheckToken?token=" + loginWith.AuthToken;

            try
            {
                var response = connectionService.SendGet(path, null);

                if (response.Contains("Запуск сервера ELMA"))
                    return false;

                loginWith = serializationService.Deserialize<LoginWithResponse>(response);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string LoginWith(GlobalSettings settings)
        {
            var path = settings.Host + @"/API/REST/Authorization/LoginWith?username=" + settings.Login;

            try
            {
                var response = connectionService.SendPost(path, settings.Password, null);

                if (response.Contains("Запуск сервера ELMA"))
                    return "Не удалось подключиться к серверу ELMA: " + "Запуск сервера ELMA";

                loginWith = serializationService.Deserialize<LoginWithResponse>(response);

                return string.Empty;
            }
            catch (WebException ex)
            {
                return "Не удалось найти сервер ELMA";
            }
            catch (Exception ex)
            {
                return "Не удалось подключиться к серверу ELMA: " + ex.Message;
            }

            return "Не удалось подключиться к серверу ELMA: " + "Неизвестная ошибка";
        }
    }
}
