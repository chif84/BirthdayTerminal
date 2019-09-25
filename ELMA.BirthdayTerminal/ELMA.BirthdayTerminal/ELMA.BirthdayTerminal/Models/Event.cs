using System;
using System.Diagnostics.Contracts;
using System.IO;
using ELMA.BirthdayTerminal.Models.Elma;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal.Models
{
    public abstract class Event
    {
        public Event(User user)
        {
            Contract.Requires(user != null);
            Contract.Requires(user.BirthDate != null);

            Id = user.Id;
            SetData(user);
        }

        /// <summary>
        /// Отображаемый заголовок
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Идентификатор пользователя в ELMA
        /// </summary>
        public long Id { get; set; }

        public ImageSource Image { get; set; }

        protected abstract void SetData(User user);
    }
}
