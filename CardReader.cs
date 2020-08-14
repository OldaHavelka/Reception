using System;
using System.Text;

namespace Reception
{
    class CardReader : AbsDevice
    {
        private const string _type = "CardReader";
        private string _accessCardNumber;
        public string GetAccessCardNumber() { return _accessCardNumber; }
        public void SetAccessCardNumber(string accessCardNumber) {
            _accessCardNumber = ReverseBytesAndPad(accessCardNumber);
        }

        public CardReader(int id, string deviceName, string accessCardNumber)
        {
            SetDeviceType(_type);
            SetId(id);
            SetName(deviceName);
            _accessCardNumber = ReverseBytesAndPad(accessCardNumber);
            Console.WriteLine(GetCurrentState());
        }

        public override string GetCurrentState()
        {
            StringBuilder returnable = new StringBuilder().AppendFormat("{0} {1} with id {2} has an access card number of {3}", GetDeviceType(), GetName(), GetId().ToString(), _accessCardNumber);
            return returnable.ToString();
        }

        private string ReverseBytesAndPad(string accessCardNumber)
        {
            StringBuilder returnable = new StringBuilder();
            StringBuilder accessCardNumberBuilder = new StringBuilder(accessCardNumber);
            for (int i = accessCardNumber.Length/2; i < 8; i++) 
            {
                accessCardNumberBuilder.Append("00");
            }
            for (int i = 7; i >= 0; i--)
            {
                returnable.Append(accessCardNumberBuilder[i*2]).Append(accessCardNumberBuilder[i * 2 + 1]);
            }
            return returnable.ToString();
        }
    }
}
