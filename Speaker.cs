using System;
using System.Runtime.Serialization;

namespace Reception
{
    class Speaker : AbsDevice
    {
        public enum Sounds 
        {
            None,
            Music,
            Alarm
        }

        private Sounds _sound;
        public Sounds Sound { 
            get
            {
                return _sound;
            }
            set
            {
                if (value != (Sounds)0 && value != (Sounds)1 && value != (Sounds)2) 
                {
                    throw new SoundIncorrectValueException();
                }
                _sound = value;
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        private float _volume;
        public float Volume { get 
            {
                return _volume;
            } set 
            {
                if (value < 0) 
                {
                    throw new VolumeNegativeValueException();
                }
                if (value > float.MaxValue) 
                {
                    throw new OverflowException();
                }
                _volume = value;
                OnDeviceUpdated(this, EventArgs.Empty);
            } 
        }

        //constructor, creates this.Speaker
        public Speaker(int id, string name, Sounds sound, float volume)
        {
            Id = id;
            Sound = sound;
            Volume = volume;
            Type = (Types)4;
            var UpdateEvent = new Events();
            DeviceUpdated += UpdateEvent.OnDeviceUpdated;
            Name = name;  //I want only one OnDeviceUpdated triggered on device creation.
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return Type.ToString() + " " + Name + " with id " + Id + " plays " + Sound.ToString() + " at the volume of " + Volume + ".";
        }
    }

    [Serializable]
    internal class VolumeNegativeValueException : Exception
    {
        const string volumeNegativeValueMessage = "Volume cannot have a negative value.";

        public VolumeNegativeValueException() : base(volumeNegativeValueMessage)
        {
        }

        public VolumeNegativeValueException(string message) : base(message + " - " + volumeNegativeValueMessage)
        {
        }

        public VolumeNegativeValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VolumeNegativeValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class SoundIncorrectValueException : Exception
    {
        const string soundIncorrectValueMessage = "Sound has to have a value of 0, (Sounds)1 or (Sounds)2.";

        public SoundIncorrectValueException() : base(soundIncorrectValueMessage)
        {
        }

        public SoundIncorrectValueException(string message) : base(message + " - " + soundIncorrectValueMessage)
        {
        }

        public SoundIncorrectValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SoundIncorrectValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}