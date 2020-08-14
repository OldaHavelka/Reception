using System;
using System.Text;

namespace Reception
{
    class Speaker : AbsDevice
    {
        const string _type = "Speaker";

        private Enums.Sound _sound;
        public Enums.Sound GetSound() { return _sound; }
        public void SetSound( Enums.Sound sound ) { _sound = sound; }

        private float _volume;
        public float GetVolume() { return _volume; }
        public void SetVolume( float volume ) { _volume = volume; }

        public Speaker(int id, string deviceName, Enums.Sound sound, float volume) 
        {
            SetDeviceType(_type);
            SetId(id);
            SetName(deviceName);
            _sound = sound;
            _volume = volume;
            Console.WriteLine(GetCurrentState());
        }

        public override string GetCurrentState()
        {
            StringBuilder returnable = new StringBuilder().AppendFormat("{0} {1} with id {2} has sound set to {3} at volume {4}.", GetDeviceType(), GetName(), GetId().ToString(), _sound, _volume);
            return returnable.ToString();
        }
    }
}
