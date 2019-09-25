using System;

namespace ELMA.BirthdayTerminal.Models.Elma
{
    /// <summary>
    /// Модель пользователя в ELMA
    /// </summary>
    public class User
    {
        public long Id { get; set; }

        public Guid Uid { get; set; }

        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public Photo Photo { get; set; }
    }
}
