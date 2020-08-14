using System;
using System.Text;

namespace Reception
{
    class Door : AbsDevice
    {
        private const string _type = "Door";

        private Enums.State _state;

        public Door(int id, string deviceName, Enums.State state)
        {
            SetDeviceType(_type);
            SetId(id);
            SetName(deviceName);
            _state = state;
            Console.WriteLine(GetCurrentState());
        }

        //Closes this door
        //If possible, sets Open, OpenForTooLong and OpenedForcibly to 0.
        //If not possible, explains why
        public void Close() {
            if (_state != Enums.State.Locked)   //I never thought about locking an open door. I did try it just because of this one line of code.
            {                                   //Pleasently surprised that it is possible lock a door that is open. 
                if (_state == Enums.State.Open)
                {
                    _state -= Enums.State.Open;
                    _state -= Enums.State.OpenForTooLong;
                    _state -= Enums.State.OpenedForcibly;
                    Console.WriteLine("Door is closed.");
                }
                else
                {
                    Console.WriteLine("Door was already closed.");
                }
            }
            else
            {
                Console.WriteLine("Door cannot be closed because it's locked");
            }
        }

        //Opens this door
        //If possible, sets Open to 1.
        //If not possible, explains why
        public void Open()
        {
            if (_state != Enums.State.Locked)
            {
                if (_state == Enums.State.Open)
                {
                    Console.WriteLine("Door was already open.");
                }
                else
                {
                    _state |= Enums.State.Open;
                    Console.WriteLine("Door is opened.");
                }
            }
            else
            {
                Console.WriteLine("Door cannot be opened because it's locked.");
            }
        }

        //Locks this door
        //If possible, sets Locked to 1.
        //If not possible, explains why
        public void Lock()
        {
            if (_state == Enums.State.Locked)
            {
                Console.WriteLine("Door was already locked.");
            }
            else
            {
                _state |= Enums.State.Locked;
                Console.WriteLine("Door is locked.");
            }
        }

        //Unlocks this door
        //If possible, sets Locked to 0.
        //If not possible, explains why
        public void Unlock()
        {
            if (_state == Enums.State.Locked)
            {
                _state -= Enums.State.Locked;
                Console.WriteLine("Door is unlocked");
            }
            else
            {
                Console.WriteLine("Door was already unlocked.");
            }
        }

        //Sets OpenForTooLong to 1
        //If possble, it does so
        //If not possible, explain why
        public void OpenForTooLong() 
        {
            if (_state != Enums.State.Open)
            {
                Console.WriteLine("Door cannot be open for too long since it's closed.");
            }
            else if (_state == Enums.State.OpenForTooLong) 
            {
                Console.WriteLine("Door was already opened for too long.");
            } 
            else 
            {
                _state |= Enums.State.OpenForTooLong;
                Console.WriteLine("Door is opened for too long now.");
            }
        }


        //Forcibly opens the door
        //If possbile, sets Locked to 0 and Open and OpenedForcibly to 1.
        //If not possible, explains why
        public void OpenForcibly() 
        {
            if (_state == Enums.State.Open)
            {
                Console.WriteLine("Door was already open.");
            }
            else
            {
                _state -= Enums.State.Locked;
                _state |= Enums.State.Open;
                _state |= Enums.State.OpenedForcibly;
                Console.WriteLine("Door is opened forcibly.");
            }
        }

        public override string GetCurrentState()
        {
            StringBuilder returnable = new StringBuilder().AppendFormat("{0} {1} with id {2} is {3}", GetDeviceType(), GetName(), GetId().ToString(), _state);
            return returnable.ToString();
        }

    }
}
