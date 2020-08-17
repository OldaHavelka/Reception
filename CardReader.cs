using System;
using System.Text;

namespace Rec
{
    class CardReader : AbsDevice
    {
        private const string _type = "CardReader";
        private string _accessCardNumber;
        public string AccessCardNumber {
            get { return _accessCardNumber; }
            set { _accessCardNumber = ReverseBytesAndPad(value); } 
        }

        public CardReader(int id, string name, string accessCardNumber)
        {
            Id = id;
            Name = name;
            _accessCardNumber = ReverseBytesAndPad(accessCardNumber);
            Type = _type;
            Console.WriteLine(GetCurrentState());
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return _type + " " + Name + " with id " + Id + " has an access card number of " + _accessCardNumber + ".";
        }

        //Takes a string, makes it 16 chars long by filling it with zeros, reverses the string in pairs. (AB12CD becomes ...00CD12AB)
        //Expects input that has even length.
        private string ReverseBytesAndPad(string accessCardNumber)
        {
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
}
