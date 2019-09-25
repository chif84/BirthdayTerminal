using Android.App;
using Android.Widget;
using ELMA.BirthdayTerminal.Droid.Services;
using ELMA.BirthdayTerminal.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessageService))]

namespace ELMA.BirthdayTerminal.Droid.Services
{
    /// <summary>
    /// Сервис вывода сообщений
    /// </summary>
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Долгое сообщение
        /// </summary>
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        /// <summary>
        /// Короткое сообщение
        /// </summary>
        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}