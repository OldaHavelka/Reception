using System;

namespace Rec
{
    class Alarm : AbsDevice
    {
        private const string _type = "Alarm";

        public Alarm(int id, string name) 
        {
            Id = id;
            Name = name;
            Type = _type;
            Console.WriteLine(GetCurrentState());
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return _type + " " + Name + " with id " + Id + ".";
        }
    }
}
