using System;
using System.Collections.Generic;
using System.Text;

namespace Rec
{
    class LedPanel : AbsDevice
    {
        public string Message { get; set; }
        private const string _type = "LedPanel";

        public LedPanel(int id, string name, string message)
        {
            Id = id;
            Name = name;
            Message = message;
            Type = _type;
            Console.WriteLine(GetCurrentState());
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return _type + " " + Name + " with id " + Id + " displays a message: \"" + Message + "\".";
        }
    }
}
