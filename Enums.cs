using System;
using System.Collections.Generic;
using System.Text;

namespace Reception
{
    class Enums
    {
        public enum Sound
        {
            None,
            Music,
            Alarm
        }

        public enum State
        {
            Default = 0b_0000_0000,
            Locked = 0b_0000_0001,
            Open = 0b_0000_0010,
            OpenForTooLong = 0b_0000_0100,
            OpenedForcibly = 0b_0000_1000
        }
    }
}
