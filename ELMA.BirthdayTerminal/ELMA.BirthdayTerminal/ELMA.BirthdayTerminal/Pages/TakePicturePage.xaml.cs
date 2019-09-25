using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using System.Threading;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal
{
    /// <summary>
    /// Страница фотографирования
    /// </summary>
    public partial class TakePicturePage : ContentPage
    {
        private IBitmapService bitmapService;
        private ILocalStorageService localStorageService;

        private Event bEvent;
        private GlobalSettings settings;

        private bool cameraOn = true;

        private byte[] originalPicture;
        private ImageSource pictureSource = null;
        private string text;

        private string takePicture = "Сделать снимок";
        private string retakePicture = "Переснять снимок";

        /// <summary>
        /// Ctor
        /// </summary>
        public TakePicturePage()
        {
            InitializeComponent();

            bitmapService = DependencyService.Get<IBitmapService>();
            localStorageService = DependencyService.Get<ILocalStorageService>();

            PhotoCapturedEvent += (sender, source) =>
            {
                layoutCamera.IsVisible = false;
                layoutPhoto.IsVisible = true;

                if (source is StreamImageSource streamSource && streamSource.Stream != null)
                {
                    var stream = streamSource.Stream(CancellationToken.None).Result;

                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        originalPicture = ms.ToArray();

                        SetText(); //Установка текста поздравления
                    }
                }

                viewPhoto.Source = pictureSource;

                btnText.IsEnabled = !cameraOn;
                btnNext.IsEnabled = !cameraOn;
                btnTake.Text = cameraOn ? takePicture : retakePicture;
            };
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public TakePicturePage(GlobalSettings settings, Event bEvent) : base()
        {
            this.bEvent = bEvent;
            this.settings = settings;
        }

        /// <summary>
        /// Установка текста
        /// </summary>
        private void SetText()
        {
            var pictureWithText = bitmapService.TextOut(originalPicture, text);

            pictureSource = ImageSource.FromStream(() => new MemoryStream(pictureWithText));
            viewPhoto.Source = pictureSource;
        }

        /// <summary>
        /// Возврат на страницу списка поздравляемых
        /// </summary>
        private void OnButtonCancelClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new TerminalMenuPage(settings);
        }

        /// <summary>
        /// Нажатие на кнопку фотографирования
        /// </summary>
        private void OnButtonTakeClicked(object sender, EventArgs e)
        {
            cameraOn = !cameraOn;

            if (cameraOn)
            {
                layoutPhoto.IsVisible = false;
                layoutCamera.IsVisible = true;
                viewCamera.InvokeOnTakeClick(false);

                btnText.IsEnabled = !cameraOn;
                btnNext.IsEnabled = !cameraOn;
                btnTake.Text = cameraOn ? takePicture : retakePicture;
            }
            else
            {
                viewCamera.InvokeOnTakeClick(true);
            }
        }

        /// <summary>
        /// Нажатие на кнопку добавления текста
        /// </summary>
        private async void OnButtonTextClicked(object sender, EventArgs e)
        {
            var takePictureTextPopup = new TakePicturePopupPage();

            SetTextEvent += (eventSender, text) =>
            {
                this.text = text;
                SetText();
            };

            await PopupNavigation.Instance.PushAsync(takePictureTextPopup);
        }

        /// <summary>
        /// Нажатие на кнопку отправки поздравления
        /// </summary>
        private void OnButtonNextClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new TerminalMenuPage(settings, "Поздравление отправлено");
        }

        #region Events

        public static event EventHandler<ImageSource> PhotoCapturedEvent;
        public static void OnPhotoCaptured(ImageSource src)
        {
            PhotoCapturedEvent?.Invoke(null, src);
        }

        public static event EventHandler<string> SetTextEvent;
        public static void OnSetText(string text)
        {
            SetTextEvent?.Invoke(null, text);
        }

        #endregion
    }
}
