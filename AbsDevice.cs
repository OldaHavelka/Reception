using System;
using System.Collections.Generic;
using System.Text;

namespace Rec
{
    abstract class AbsDevice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private string _type = null;
        public string Type {
            get 
            {
                return _type;
            }
            set
            {
                if (_type == null)  //_type cannot be overwritten
                {
                    _type = value;
                }
            } 
        }

        //Should return a string that has all device attributes in it.
        virtual public string GetCurrentState() 
        {
            return "I was supposed to be overridden!";
        }
    }
}
