using System;
using System.Runtime.Serialization;

namespace Reception
{
    abstract class AbsDevice
    {
        public delegate void DeviceUpdatedEventHandler(object source, EventArgs args);

        public event DeviceUpdatedEventHandler DeviceUpdated;

        public enum Types
        {
            Alarm,
            CardReader,
            Door,
            LedPanel,
            Speaker,
            AbsDevice
        }

        private Types _type = (Types)5;
        public Types Type 
        {
            get 
            {
                return _type;
            }
            protected set 
            {
                if (_type == (Types)5) 
                {
                    _type = value;
                    OnDeviceUpdated(this, EventArgs.Empty);
                }
            }
        }

        private int _id;
        public int Id { 
            get 
            {
                return _id;
            } 
            set 
            {
                if (!Root.IsIdUnique(value)) { throw new DeviceIdIsNotUniqueException(); }
                _id = value;
                OnDeviceUpdated(this, EventArgs.Empty);
            } 
        }

        private string _name;
        public string Name {
            get 
            {
                return _name;
            } 
            set 
            {
                _name = value;
                OnDeviceUpdated(this, EventArgs.Empty); 
            } 
        }

        //Should be overridden, should return a string that has all device attributes in it.
        virtual public string GetCurrentState()
        {
            return "I was supposed to be overridden!";
        }

        protected virtual void OnDeviceUpdated(object source, EventArgs e) 
        {
            DeviceUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    [Serializable]
    internal class DeviceIdIsNotUniqueException : Exception
    {
        const string deviceIdIsNotUniqueMessage = "Device ID is not unique.";
        public DeviceIdIsNotUniqueException() : base (deviceIdIsNotUniqueMessage)
        {
        }

        public DeviceIdIsNotUniqueException(string message) : base(message + " - " + deviceIdIsNotUniqueMessage)
        {
        }

        public DeviceIdIsNotUniqueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeviceIdIsNotUniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
