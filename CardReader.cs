using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Reception
{
    class CardReader : AbsDevice
    {

        private string _accessCardNumber;
        public string AccessCardNumber
        {
            get 
            {
                return _accessCardNumber; 
            }
            set 
            {
                _accessCardNumber = ReverseBytesAndPad(value);
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //constructor, creates this.CardReader
        public CardReader(int id, string name, string accessCardNumber)
        {
            Id = id;
            AccessCardNumber = accessCardNumber;
            Type = (Types)1;
            var UpdateEvent = new Events();
            DeviceUpdated += UpdateEvent.OnDeviceUpdated;
            Name = name;  //I want only one OnDeviceUpdated triggered on device creation.
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return Type.ToString() + " " + Name + " with id " + Id + " has an access card number of " + AccessCardNumber + ".";
        }

        //Takes a string, makes it 16 chars long by filling it with zeros, reverses the string in pairs. (AB12CD becomes ...00CD12AB)
        //Makes sure that the input string is acceptable
        //If input string has odd length, throws AccessCardNumberNotEvenLengthException
        //If input string is shorter than 0 or longer than 16, calls AccessCardNumberInvalidLengthException
        //If input string contains a non-hexadecimal character, throws AccessCardNumberContainsInvalidCharactersException
        private string ReverseBytesAndPad(string accessCardNumber)
        {
            accessCardNumber = accessCardNumber.ToUpper();
            if (accessCardNumber.Length % 2 == 1) { throw new AccessCardNumberNotEvenLengthException(); }
            if (accessCardNumber.Length > 16 && accessCardNumber.Length < 0) { throw new AccessCardNumberInvalidLengthException(); }
            Regex rgx = new Regex("^[0123456789ABCDEF]*$");
            if (!rgx.IsMatch(accessCardNumber)) { throw new AccessCardNumberContainsInvalidCharactersException(); }

            StringBuilder returnable = new StringBuilder();
            StringBuilder accessCardNumberBuilder = new StringBuilder(accessCardNumber);
            for (int i = accessCardNumber.Length / 2; i < 8; i++)
            {
                accessCardNumberBuilder.Append("00");
            }
            for (int i = 7; i >= 0; i--)
            {
                returnable.Append(accessCardNumberBuilder[i * 2]).Append(accessCardNumberBuilder[i * 2 + 1]);
            }
            return returnable.ToString();
        }
    }

    [Serializable]
    internal class AccessCardNumberContainsInvalidCharactersException : Exception
    {
        const string accessCardNumberContainsInvalidCharactersMessage = "Access card nubmer has to be made up of digits ranging from 0 to 9 and letters ranging from A to F.";

        public AccessCardNumberContainsInvalidCharactersException() : base(accessCardNumberContainsInvalidCharactersMessage)
        {
        }

        public AccessCardNumberContainsInvalidCharactersException(string message) : base(message + " - " + accessCardNumberContainsInvalidCharactersMessage)
        {
        }

        public AccessCardNumberContainsInvalidCharactersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccessCardNumberContainsInvalidCharactersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class AccessCardNumberInvalidLengthException : Exception
    {
        const string accessCardNumberInvalidLengthMessage = "Access card number length has to be equal to or more than 0 and equal to or less than 16.";
        public AccessCardNumberInvalidLengthException() : base(accessCardNumberInvalidLengthMessage)
        {
        }

        public AccessCardNumberInvalidLengthException(string message) : base(message + " - " + accessCardNumberInvalidLengthMessage)
        {
        }

        public AccessCardNumberInvalidLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccessCardNumberInvalidLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class AccessCardNumberNotEvenLengthException : Exception
    {
        const string accessCardNumberNotEvenLengthMessage = "Access card number has to have even length.";
        public AccessCardNumberNotEvenLengthException() : base(accessCardNumberNotEvenLengthMessage)
        {
        }

        public AccessCardNumberNotEvenLengthException(string message) : base(message + " - " + accessCardNumberNotEvenLengthMessage)
        {
        }

        public AccessCardNumberNotEvenLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccessCardNumberNotEvenLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
