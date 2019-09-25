using System;
using System.Runtime.Serialization;

namespace ELMA.BirthdayTerminal.Models
{
    [DataContract]
    public class GlobalSettings : ICloneable
    {
        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
