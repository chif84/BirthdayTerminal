using System;
using System.IO;
using System.Reflection;
using ELMA.BirthdayTerminal.Models.Elma;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal.Models
{
    /// <summary>
    /// Личное событие
    /// </summary>
    public class BirthdayEvent : Event
    {
        public BirthdayEvent(User user) : base(user)
        {
        }

        protected override void SetData(User user)
        {
            Name = user.FullName;
            Description = "День рождения " + user.BirthDate.Value.ToString("dd.MM.yyyy");
            Date = (DateTime)user.BirthDate;

            Image = ImageSource.FromStream(() => new MemoryStream(DefaultPhoto)); //TODO локальное сохранение фотографий
        }

        private byte[] defaultPhoto;

        protected byte[] DefaultPhoto
        {
            get
            {
                if (defaultPhoto == null)
                {
                    var assembly = IntrospectionExtensions.GetTypeInfo(typeof(BirthdayEvent)).Assembly;
                    var stream = assembly.GetManifestResourceStream("ELMA.BirthdayTerminal.Resources.nophoto.png"); //TODO константа

                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        defaultPhoto = ms.ToArray();
                    }
                }

                return defaultPhoto;
            }
        }
    }
}
