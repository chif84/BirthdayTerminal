using System;
using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Services;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal
{
    /// <summary>
    /// Страница меню терминала
    /// </summary>
    public partial class TerminalMenuPage : ContentPage
    {
        private GlobalSettings settings;

        private IBirthdayEventService birthdayEventService;
        private IMessageService messageService;

        /// <summary>
        /// Ctor
        /// </summary>
        public TerminalMenuPage()
        {
            InitializeComponent();

            birthdayEventService = DependencyService.Get<IBirthdayEventService>();

            todayEvents.ItemsSource = birthdayEventService.GetTodayEvents();
            nextEvents.ItemsSource = birthdayEventService.GetNexEvents();
            messageService = DependencyService.Get<IMessageService>();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public TerminalMenuPage(GlobalSettings settings): this()
        {
            this.settings = settings;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public TerminalMenuPage(GlobalSettings settings, string text) : this()
        {
            messageService.LongAlert("Поздравление отправлено");
            this.settings = settings;
        }

        /// <summary>
        /// Выбор события
        /// </summary>
        private void OnEventSelect(object sender, EventArgs e)
        {
            var bEvent = (sender as ViewCell)?.BindingContext as Event;
            if (bEvent != null)
            {
                App.Current.MainPage = new TakePicturePage();
            }
        }

    }

}
