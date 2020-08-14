using Microsoft.VisualBasic.CompilerServices;
using System;

namespace Reception
{
    //Functions and variables found in every type of device
    abstract class AbsDevice
    {
        private string _type = "Undefined type";
        public string GetDeviceType() { return _type; }
        protected void SetDeviceType(string type) {
            _type = type;
        }

        private int _id;
        public int GetId() { return _id; }
        public void SetId(int id) { 
            _id = id;
        }

        private string _name;
        public string GetName() { return _name; }
        public void SetName(string name) { 
            _name = name;
        }

        public virtual string GetCurrentState() { return "This string was supposed to be overridden."; }

    }
}
