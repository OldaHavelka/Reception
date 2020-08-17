using System;
using System.Text;

namespace Rec
{
    //I feel like there is a far better way to get the value of a flag than (int)_state % 16 - (int)_state % 8).
    //But hey, it works.
    class Door : AbsDevice
    {
        private const string _type = "Door";

        private Enums.State _state;

        public Door(int id, string name)
        {
            Id = id;
            Name = name;
            _state = 0;
            Type = _type;
            Console.WriteLine(GetCurrentState());
        }

        //Closes this door
        //If possible, sets Open, OpenForTooLong and OpenedForcibly to 0.
        //If not possible, explains why
        public void Close()
        {
            if ((int)_state % 2 != (int)Enums.State.Locked)   //I never thought about locking an open door. I did try it just because of this one line of code.
            {                                   //Pleasently surprised that it is possible lock a door that is open. 
                if (((int)_state % 4 - (int)_state % 2) == (int)Enums.State.Open)
                {
                     _state -= Enums.State.Open;
                    if (((int)_state % 8 - (int)_state % 4) == (int)Enums.State.OpenForTooLong) { _state -= Enums.State.OpenForTooLong; }
                    if (((int)_state % 16 - (int)_state % 8) == (int)Enums.State.OpenedForcibly) { _state -= Enums.State.OpenedForcibly; }
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
            Console.WriteLine(GetCurrentState());
        }

        //Opens this door
        //If possible, sets Open to 1.
        //If not possible, explains why
        public void Open()
        {
            if ((int)_state % 2 != (int)Enums.State.Locked)
            {
                if (((int)_state % 4 - (int)_state % 2) == (int)Enums.State.Open)
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
            Console.WriteLine(GetCurrentState());
        }

        //Locks this door
        //If possible, sets Locked to 1.
        //If not possible, explains why
        public void Lock()
        {
            if ((int)_state % 2 == (int)Enums.State.Locked)
            {
                Console.WriteLine("Door was already locked.");
            }
            else
            {
                _state |= Enums.State.Locked;
                Console.WriteLine("Door is locked.");
            }
            Console.WriteLine(GetCurrentState());
        }

        //Unlocks this door
        //If possible, sets Locked to 0.
        //If not possible, explains why
        public void Unlock()
        {
            if ((int)_state % 2 == (int)Enums.State.Locked)
            {
                _state -= Enums.State.Locked;
                Console.WriteLine("Door is unlocked");
            }
            else
            {
                Console.WriteLine("Door was already unlocked.");
            }
            Console.WriteLine(GetCurrentState());
        }

        //Sets OpenForTooLong to 1
        //If possble, it does so
        //If not possible, explain why
        public void OpenForTooLong()
        {
            if (((int)_state % 4 - (int)_state % 2) != (int)Enums.State.Open)
            {
                Console.WriteLine("Door cannot be open for too long since it's closed.");
            }
            else if (((int)_state % 8 - (int)_state % 4) == (int)Enums.State.OpenForTooLong)
            {
                Console.WriteLine("Door was already opened for too long.");
            }
            else
            {
                _state |= Enums.State.OpenForTooLong;
                Console.WriteLine("Door is opened for too long now.");
            }
            Console.WriteLine(GetCurrentState());
        }


        //Forcibly opens the door
        //If possbile, sets Locked to 0 and Open and OpenedForcibly to 1.
        //If not possible, explains why
        public void OpenForcibly()
        {
            if (((int)_state % 4 - (int)_state % 2) == (int)Enums.State.Open)
            {
                Console.WriteLine("Door was already open.");
            }
            else
            {
                if ((int)_state % 2 == (int)Enums.State.Locked) { _state -= Enums.State.Locked; } 
                _state |= Enums.State.Open;
                _state |= Enums.State.OpenedForcibly;
                Console.WriteLine("Door is opened forcibly.");
            }
            Console.WriteLine(GetCurrentState());
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            StringBuilder toAppend = new StringBuilder(" is");

            if ((int)_state % 2 == (int)Enums.State.Locked)
            {
                toAppend.Append(" locked");
            }
            else 
            {
                toAppend.Append(" unlocked");
            }

            if (((int)_state % 4 - (int)_state % 2) == (int)Enums.State.Open)
            {
                toAppend.Append(", open");
            }
            else
            {
                toAppend.Append(", closed");
            }

            if (((int)_state % 16 - (int)_state % 8) == (int)Enums.State.OpenedForcibly) 
            {
                toAppend.Append(", opened forcibly");
            }

            if (((int)_state % 8 - (int)_state % 4) == (int)Enums.State.OpenForTooLong)
            {
                toAppend.Append(", open for too long");
            }

            return _type + " " + Name + " with id " + Id + toAppend.ToString() + ".";
        }

    }
}
