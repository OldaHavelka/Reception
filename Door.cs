using System;
using System.Text;

namespace Reception
{
    class Door : AbsDevice
    {
        [Flags]
        public enum States
        {
            Locked = 0b_0001,
            Open = 0b_0010,
            OpenForTooLong = 0b_0100,
            OpenedForcibly = 0b_1000
        }

        public States State { get; private set; }

        //Locks and unlocks this.Door
        public bool Locked { set
            {
                if (value == true) { State |= States.Locked; }
                else { State &= ~States.Locked; }
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //Opens and closes this.Door
        public bool Open
        {
            set
            {
                if (value == true) { State |= States.Open; }
                else { State &= ~States.Open; }
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //Tags and untags this.Door as open for too long 
        public bool OpenForTooLong
        {
            set
            {
                if (value == true) { State |= States.OpenForTooLong; }
                else { State &= ~States.OpenForTooLong; }
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //Tags and untags this.Door as open forcibly
        public bool OpenedForcibly
        {
            set
            {
                if (value == true) { State |= States.OpenedForcibly; }
                else { State &= ~States.OpenedForcibly; }
                OnDeviceUpdated(this, EventArgs.Empty);
            }
        }

        //Constructor, creates this.Door
        public Door(int id, string name) 
        {
            Id = id;
            State = 0;
            Type = (Types)2;
            var UpdateEvent = new Events();
            DeviceUpdated += UpdateEvent.OnDeviceUpdated;
            Name = name;  //I want only one OnDeviceUpdated triggered on device creation.
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            StringBuilder toAppend = new StringBuilder(" is");
            if ((State & States.Locked) == States.Locked) { toAppend.Append(" locked"); }
            else { toAppend.Append(" unlocked"); }
            if ((State & States.Open) == States.Open) { toAppend.Append(", open"); }
            else { toAppend.Append(", closed"); }
            if ((State & States.OpenForTooLong) == States.OpenForTooLong) { toAppend.Append(", open for too long"); }
            if ((State & States.OpenedForcibly) == States.OpenedForcibly) { toAppend.Append(", opened forcibly"); }

            return Type.ToString() + " " + Name + " with id " + Id + toAppend.ToString() + ".";
        }
    }
}
