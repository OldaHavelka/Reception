using System;
using System.Collections.Generic;
using System.Text;

namespace Rec
{
    class Speaker : AbsDevice
    {
        private const string _type = "Speaker";
        public Enums.Sound Sound { get; set; }
        public float Volume { get; set; }

        //constructor
        public Speaker(int id, string name, Enums.Sound sound, float volume)
        {
            Id = id;
            Name = name;
            Sound = sound;
            Volume = volume;
            Type = _type;
            Console.WriteLine(GetCurrentState());
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return _type + " " + Name + " with id " + Id + " plays " + Sound.ToString() + " at the volume of " + Volume + ".";
        }
    }
}