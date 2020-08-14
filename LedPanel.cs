using System;
using System.Data;
using System.Text;

namespace Reception
{
    class LedPanel : AbsDevice
    {
        private string _message;
        public void SetMessage(string Message) { 
            _message = Message;
        }
        public string GetMessage() { return _message; }

        private const string _type = "LedPanel";

        public LedPanel(int id, string deviceName, string message) 
        {
            SetDeviceType(_type);
            SetId(id);
            SetName(deviceName);
            _message = message;
            Console.WriteLine(GetCurrentState());
        }

        public override string GetCurrentState() 
        {
            StringBuilder returnable = new StringBuilder().AppendFormat("{0} {1} with id {2} displays a message: {3}", GetDeviceType(), GetName(), GetId().ToString(), _message); 
            return returnable.ToString();
        }
    }
}
