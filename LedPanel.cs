using System;

namespace Reception
{
    class LedPanel : AbsDevice
    {
        private string _message;
        public string Message { 
            get
            {
                return _message;
            } 
            set
            {
                _message = value;
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //constructor, creates this.LedPanel
        public LedPanel(int id, string name, string message)
        {
            Id = id;
            Message = message;
            Type = (Types)3;
            var UpdateEvent = new Events();
            DeviceUpdated += UpdateEvent.OnDeviceUpdated;
            Name = name;  //I want only one OnDeviceUpdated triggered on device creation.
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return Type.ToString() + " " + Name + " with id " + Id + " displays a message: \"" + Message + "\".";
        }
    }
}