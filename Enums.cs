using System;
using System.Collections.Generic;
using System.Text;

namespace Rec
{
    //Contains all enums used in this program.
    class Enums
    { 
        public enum Sound 
        {
            None,
            Music,
            Alarm,
            Unacceptable
        }

        public enum State
        {
            Locked = 0b_0000_0001,
            Open = 0b_0000_0010,
            OpenForTooLong = 0b_0000_0100,
            OpenedForcibly = 0b_0000_1000
        }
    }
}
