using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Reception
{
    class Root
    {

        private List<Group> _listOfGroups = new List<Group>();
        public List<Group> GetListOfGroups() { return _listOfGroups; }

        //Root only contains one list of groups: List<Group> ListOfGroups
        public Root()
        {
        }

        //Creates an empty group and inserts it into the list of groups: List<Group> ListOfGroups
        //Informs user that a group has been created. 
        public void CreateGroup() {
            _listOfGroups.Add(new Group());
            Console.WriteLine("Group {0} added.", _listOfGroups.Count - 1);
            UpdateTree();
        }
        
        //Calls DoesGroupExist() to check if group exists
        //If so, remove group from the group list: List<Group> ListOfGroups
        //If so, inform user about removing the group.
        //If not so, DoesGroupExist() explained why the group doesn't exist.
        public void RemoveGroup() 
        {
            Console.Write("What group would you like to remove?: ");
            int groupIndex = UserInputUntilValidGroupIndex();
            _listOfGroups.RemoveAt(groupIndex);
            Console.WriteLine("Group {0} removed.", groupIndex);
            UpdateTree();
        }

        //Checks if a group exists.
        //If group exists, return true.
        //If GroupIndex is less than zero, return false and explain why it does not exist.
        //If group does not exist, return false and explain why it does not exist.
        //If group does not exist because there is no group, create a group (since there is no use for a root without a group).
        public bool DoesGroupExist(int groupIndex) {
            if (_listOfGroups.Count == 0) 
            { 
                Console.WriteLine("There are no groups.");
                CreateGroup();
                return false; 
            }
            if (_listOfGroups.Count - 1 < groupIndex || groupIndex < 0)
            { 
                Console.WriteLine("The ammount of groups is {0}. Please select a group ranging from 0 to {1}", _listOfGroups.Count, _listOfGroups.Count - 1);
                return false;
            }
            return true;
        }

        //Asks user to input a value for group index until accepted and passed forward.
        //Also explains incorrect inputs.
        public int UserInputUntilValidGroupIndex() 
        {
            bool failure;
            string groupIndexInput;
            do
            {
                failure = false;
                groupIndexInput = Console.ReadLine();
                try { int.Parse(groupIndexInput); }
                catch (FormatException)
                {
                    Console.WriteLine("Provided input is not an integer.");
                    failure = true;
                    continue;
                }
                if (!DoesGroupExist(int.Parse(groupIndexInput))) { failure = true; }
            } while (failure);
            return int.Parse(groupIndexInput);
        }



        //Asks user to input a value for id until accepted and passed forward.
        //Also explains incorrect inputs.
        public int UserInputUntilValidId() 
        {
            bool failure;
            string deviceIdInput;
            do
            {
                failure = false;
                deviceIdInput = Console.ReadLine();
                try { int.Parse(deviceIdInput); }
                catch (FormatException)
                {
                    Console.WriteLine("Provided input is not an integer.");
                    failure = true;
                    continue;
                }
                catch when (IsIdUnique(int.Parse(deviceIdInput))) { failure = false; }
            } while (failure);
            return int.Parse(deviceIdInput);
        }

        //Asks user to input a value for device type until accepted and passed forward.
        //Also explains incorrect inputs.
        public int UserInputUntilValidDeviceType()
        {
            string deviceTypeInput;
            int deviceType;
            do
            {
                deviceTypeInput = Console.ReadLine();
                deviceType = DeviceTypeInputToDeviceType(deviceTypeInput);
            } while (deviceType == -1);  //If DeviceTypeInput is not an accepted input, DeviceTypeInputToDeviceType outputs -1.
            return deviceType;
        }

        //Asks the user what device to create, which attributes does the device have, and into which group they want the device to be put in.
        //Checks if all inputs are acceptable.
        //Calls CreateDevice() on an Group list in the list of groups: List<Group> ListOfGroups
        public void CreateDevice() {

            Console.Write("What group would you like to add a device to?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            Console.WriteLine("What device would you like to add to the group {0}?", groupIndex);
            Console.Write("Choices: Alarm (0), CardReader (1), Door (2), LedPanel (3), Speaker (4). : ");
            int deviceType = UserInputUntilValidDeviceType();

            Console.Write("What ID would you like for your device to have?: ");
            int deviceId = UserInputUntilValidId();

            Console.Write("What name would you like for your device to have?: ");
            string deviceName = Console.ReadLine(); //Any device name should be alright. Including null.

            _listOfGroups[groupIndex].CreateDevice(deviceType, deviceId, deviceName);
            UpdateTree();
        }

        public void RemoveDevice() 
        {
            Console.Write("What group would you like to remove a device from?: ");
            _listOfGroups[UserInputUntilValidGroupIndex()].RemoveDevice();
            UpdateTree();
        }


        //Transforms certain strings into integer values.
        //If DeviceTypeInput does not match any specific string, -1 is returned.
        private int DeviceTypeInputToDeviceType(string DeviceTypeInput)
        {
            switch (DeviceTypeInput)
            {
                case "Alarm": case "0": return 0;
                case "CardReader": case "1": return 1;
                case "Door": case "2": return 2;
                case "LedPanel": case "3": return 3;
                case "Speaker": case "4": return 4;
                default: return -1;
            }
        }

        //If Id is not found in any device, return true.
        //If Id is found in any device, return false and inform the user that the Id is not unique.
        public bool IsIdUnique(int Id)
        {
            foreach (Group Group in _listOfGroups)
            {
                foreach (AbsDevice Device in Group.GetListOfDevices())
                {
                    if (Device.GetId() == Id) 
                    { 
                        Console.WriteLine("Id {0} is not unique.", Id); 
                        return false; 
                    }
                }
            }
            return true;
        }

        //Asks user what group he wants to interact with and then calls a matching method from Group.cs
        public void ChangeCardReaderAccessListNumber()
        {
            Console.Write("What group is the CardReader in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].ChangeCardReaderAccessListNumber();
        }

        //Asks user what group he wants to interact with and then calls a matching method from Group.cs
        public void ChangeLedPanelMessage()
        {
            Console.Write("What group is the LedPanel in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].ChangeLedPanelMessage();
        }

        //Asks user what group he wants to interact with and then calls a matching method from Group.cs
        public void ChangeSpeakerVolume()
        {
            Console.Write("What group is the Speaker in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].ChangeSpeakerVolume();
        }

        //Asks user what group he wants to interact with and then calls a matching method from Group.cs
        public void ChangeSpeakerSound()
        {
            Console.Write("What group is the Speaker in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].ChangeSpeakerSound();
        }

        //Asks user what group he wants to interact with, asks for a specific device, copies it into another user-specified group
        //and removes it from the first group
        public void MoveDevice()
        {
            Console.Write("What group is the device in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();
            if (_listOfGroups[groupIndex].AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = _listOfGroups[groupIndex].UserInputUntilValidDeviceIndex();
                AbsDevice device = _listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                Console.Write("What group do you want to move the device into?: ");
                _listOfGroups[UserInputUntilValidGroupIndex()].GetListOfDevices().Add(device);
                _listOfGroups[groupIndex].GetListOfDevices().RemoveAt(deviceIndex);
            }
            UpdateTree();
        }

        //Asks user to change id in a chosen device and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeAnyId() {
            Console.Write("What group is the device in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();
            if (_listOfGroups[groupIndex].AreThereAnyDevices()) 
            {
                Console.Write("What device is it?: ");
                int deviceIndex = _listOfGroups[groupIndex].UserInputUntilValidDeviceIndex();
                Console.Write("What new id do you want for your device to have?: ");
                AbsDevice device = _listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                device.SetId(UserInputUntilValidId());
                Console.WriteLine(device.GetCurrentState());
            }
        }

        //Asks user to change name in a chosen device and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeAnyName()
        {
            Console.Write("What group is the device in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();
            if (_listOfGroups[groupIndex].AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = _listOfGroups[groupIndex].UserInputUntilValidDeviceIndex();
                Console.Write("What new name do you want for your device to have?: ");
                AbsDevice device = _listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                device.SetName(Console.ReadLine());
                Console.WriteLine(device.GetCurrentState());
            }
        }

        public void UpdateStatus()
        {
            Console.Write("What group is the device in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();
            if (_listOfGroups[groupIndex].AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = _listOfGroups[groupIndex].UserInputUntilValidDeviceIndex();
                Console.WriteLine(_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex].GetCurrentState());
            }
        }

        //Asks user in which group he wants to open a door
        public void OpenDoor()
        {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].OpenDoor();
        }

        //Asks user in which group he wants to close a door
        public void CloseDoor() {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].CloseDoor();
        }

        //Asks user in which group he wants to lock a door
        public void LockDoor() {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].LockDoor();
        }

        //Asks user in which group he wants to unlock a door
        public void UnlockDoor() {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].UnlockDoor();
        }

        //Asks user in which group he wants to forcibly open a door
        public void OpenDoorForcibly() {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].OpenDoorForcibly();
        }

        //Asks user in which group he wants to tag a door as open for too long
        public void DoorOpenedForTooLong() {
            Console.Write("What group is the Door in?: ");
            int groupIndex = UserInputUntilValidGroupIndex();

            _listOfGroups[groupIndex].DoorOpenedForTooLong();
        }


        public void CreateDevice(int groupIndex, int deviceType, int id, string deviceName, dynamic arg1 = null, dynamic arg2 = null)
        {
            if (IsIdUnique(id) && DoesGroupExist(groupIndex)) 
            {
                _listOfGroups[groupIndex].CreateDevice(deviceType, id, deviceName, arg1, arg2); 
            }
            UpdateTree();
        }

        public void RemoveDevice(int groupIndex, int deviceIndex) 
        {
            if (DoesGroupExist(groupIndex)) 
            {
                _listOfGroups[groupIndex].RemoveDevice(deviceIndex);
            }
            UpdateTree();
        }

        public void RemoveGroup(int groupIndex)
        {
            if (DoesGroupExist(groupIndex)) {
                _listOfGroups.RemoveAt(groupIndex);
                Console.WriteLine("Group {0} removed.", groupIndex);
            }
            UpdateTree();
        }

        public void ChangeAnyName(int groupIndex, int deviceIndex, string newName) {
            if (DoesGroupExist(groupIndex)) 
            {
                if(_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    AbsDevice device = _listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                    device.SetName(newName);
                    Console.WriteLine(device.GetCurrentState());
                }
            }
        }

        public void ChangeAnyId(int groupIndex, int deviceIndex, int newId)
        {
            if (DoesGroupExist(groupIndex) && IsIdUnique(newId))
            {
                if (_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    AbsDevice device = _listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                    device.SetId(newId);
                    Console.WriteLine(device.GetCurrentState());
                }
            }
        }

        public void MoveDevice(int currentGroupIndex, int deviceIndex, int newGroupIndex)
        {
            if (DoesGroupExist(currentGroupIndex))
            {
                if (_listOfGroups[currentGroupIndex].DoesDeviceExist(deviceIndex) && DoesGroupExist(newGroupIndex))
                {
                    AbsDevice device = _listOfGroups[currentGroupIndex].GetListOfDevices()[deviceIndex];
                    _listOfGroups[newGroupIndex].GetListOfDevices().Add(device);
                    _listOfGroups[currentGroupIndex].GetListOfDevices().RemoveAt(deviceIndex);
                }
            }
            UpdateTree();
        }

        public void ChangeLedPanelMessage(int groupIndex, int deviceIndex, string newMessage) 
        {
            if (DoesGroupExist(groupIndex)) 
            {
                if (_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    if (_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex].GetDeviceType() == "LedPanel")
                    {
                        LedPanel ledPanel = (LedPanel)_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                        ledPanel.SetMessage(newMessage);
                        Console.WriteLine(ledPanel.GetCurrentState());
                    }
                    else 
                    {
                        Console.WriteLine("Device {0} is not a LedPanel.", deviceIndex);
                    }
                }
            }
        }

        public void ChangeSpeakerSound(int groupIndex, int deviceIndex, Enums.Sound newSound)
        {
            if (DoesGroupExist(groupIndex))
            {
                if (_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    if (_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex].GetDeviceType() == "Speaker")
                    {
                        Speaker speaker = (Speaker)_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                        speaker.SetSound(newSound);
                        Console.WriteLine(speaker.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a LedPanel.", deviceIndex);
                    }
                }
            }
        }

        public void LockDoor(int groupIndex, int deviceIndex) 
        {
            if (DoesGroupExist(groupIndex)) 
            {
                if (_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    if (_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door door = (Door)_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                        door.Lock();
                        Console.WriteLine(door.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a LedPanel.", deviceIndex);
                    }
                }
            }
        }

        public void UnlockDoor(int groupIndex, int deviceIndex)
        {
            if (DoesGroupExist(groupIndex))
            {
                if (_listOfGroups[groupIndex].DoesDeviceExist(deviceIndex))
                {
                    if (_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door door = (Door)_listOfGroups[groupIndex].GetListOfDevices()[deviceIndex];
                        door.Unlock();
                        Console.WriteLine(door.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a LedPanel.", deviceIndex);
                    }
                }
            }
        }

        public void UpdateTree() 
        {
            int i = 0;
            foreach (Group group in _listOfGroups)
            {
                Console.Write("Group {0}: ",i);
                foreach (AbsDevice device in group.GetListOfDevices()) 
                {
                    Console.Write(device.GetName()+" ");
                }
                i++;
                Console.Write("\n");
            }
        }
    }
}
