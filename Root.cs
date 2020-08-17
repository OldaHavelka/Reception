using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Rec
{
    class Root
    {
        private List<Group> _listOfGroups = new List<Group>();

        //Creates a group and writes its number into the console.
        public void CreateGroup()
        {
            _listOfGroups.Add(new Group());
            Console.WriteLine("Group {0} added.", _listOfGroups.Count - 1);
            PrintTree();
        }

        //Removes a group.
        //Asks user for input, checks if input is an integer, checks if group exists, removes group, confirms removal to the user, prints tree.
        //If any check fails, explains to the user why did a check fail.
        public void RemoveGroup()
        {
            int groupIndex = GetGroupIndexFromUser();
            if (groupIndex != int.MinValue) {
                _listOfGroups.RemoveAt(groupIndex);
                Console.WriteLine("Group {0} removed.", groupIndex);
                PrintTree();
            }

        }

        //Moves a device into a group.
        //If called without arguments, asks user for deviceId and groupIndex and checks if they're valid integers.
        //If called with arguments, expects them to be valid.
        //Finds a device by its id and a group by its index, checks their existence, moves device to its new group.
        //If any check fails, explains why.
        public void Move(int deviceId = int.MinValue, int groupIndex = int.MinValue)
        {
            if (deviceId == int.MinValue)
            {
                deviceId = GetIdFromUser(true, true);
                if (deviceId != int.MinValue) {
                    groupIndex = GetGroupIndexFromUser();
                }
            }

            if (deviceId != int.MinValue && groupIndex != int.MinValue)
            {
                AbsDevice device = GetDeviceById(deviceId);
                RemoveDevice(deviceId, true);
                InsertDevice(device, groupIndex);
                Console.WriteLine("Device with id {0} moved to group {1}.", deviceId, groupIndex);
                PrintTree();
            }
        }

        //Prints groups and their device names and ids.
        public void PrintTree()
        {
            for (int i = 0; i < _listOfGroups.Count; i++)
            {
                Console.Write("Group {0}:", i);
                foreach (AbsDevice device in _listOfGroups[i]._listOfDevices)
                {
                    Console.Write(" [{0}, {1}]", device.Id, device.Name);
                }
                Console.Write("\n");
            }
        }

        //Checks if a group exists.
        //If group exists, return true.
        //If group does not exist, explain why, return false.
        public bool DoesGroupExist(int groupIndex)
        {
            if (_listOfGroups.Count - 1 < groupIndex)
            {
                Console.WriteLine("Group {0} does not exist. Current groups range from 0 to {1}", groupIndex, _listOfGroups.Count - 1);
                return false;
            }
            return true;
        }

        //Inserts device into a group.
        //Expects correct inputs.
        private void InsertDevice(AbsDevice device, int groupIndex)
        {
            _listOfGroups[groupIndex]._listOfDevices.Add(device);
        }

        //Returns a device if exists.
        //If a device with matching id is found, return device.
        //If not, and silently is false, explains why, return null.
        public AbsDevice GetDeviceById(int deviceId)
        {
            foreach (Group group in _listOfGroups)
            {
                AbsDevice device = group.GetDeviceById(deviceId, true);
                if (device != null)
                {
                    return device;
                }
            }
            return null;
        }

        //Outputs group index of a device.
        //Checks ids of all devices and compares them to deviceId
        //If found, returns index of the group the device is in
        //If not found, returns int.MinValue
        public int GetGroupIndexById(int deviceId)
        {
            for (int i = 0; i < _listOfGroups.Count; i++)
            {
                if (_listOfGroups[i].GetDeviceById(deviceId, true) != null)
                {
                    return i;
                }
            }
            return int.MinValue;
        }

        //Creates a device and inserts it into a specified group
        //If called without arguments, asks user for device id and for device name
        //If id is unique, passes type, id and name to Group.CreateDevice() that handles creating specific devices.
        public void CreateDevice(int deviceType, int groupIndex = int.MinValue, int deviceId = int.MinValue, string deviceName = null, dynamic arg1 = null, dynamic arg2 = null)
        {
            {
                if (_listOfGroups.Count != 0)
                {
                    if (deviceId == int.MinValue)
                    {
                        groupIndex = GetGroupIndexFromUser();
                        if (groupIndex != int.MinValue) {
                            deviceId = GetIdFromUser(false);
                            if (deviceId != int.MinValue)
                            {
                                Console.Write("Please input the name of the device: ");
                                deviceName = Console.ReadLine();        //Any name is alright. Including null.
                            }

                        }
                    }

                    if (groupIndex != int.MinValue && deviceId != int.MinValue)
                    {
                        if (GetDeviceById(deviceId) == null) {
                            _listOfGroups[groupIndex].CreateDevice(deviceType, deviceId, deviceName, arg1, arg2);
                        }
                    }
                    PrintTree();
                }
                else
                {
                    Console.WriteLine("There are no groups. A group will be created.");
                    CreateGroup();
                }
            }
        }

        //Removes a device.
        //If called without arguments, asks user for device id, checks if device with matching id exists, passes id to Group.RemoveDevice()
        //If called with arguments, passes arguments to Group.RemoveDevice(). Expects arguments to be valid.
        //If silently is false, prints tree.
        public void RemoveDevice(int deviceId = int.MinValue, bool silently = false)
        {
            if (deviceId == int.MinValue)
            {
                deviceId = GetIdFromUser(true, true);
            }
            if (deviceId != int.MinValue)
            {
                _listOfGroups[GetGroupIndexById(deviceId)].RemoveDevice(deviceId, silently);
            }
            if (!silently) { PrintTree(); }
        }

        //Changes an attribute of a device.
        //If called without arguments other than changeType, asks user for device id, checks if device with matching id exists, passes args to Group.Change()
        //Checks for ids and names are done here.
        //If called with arguments, passes arguments to Group.Change(). Expects arguments to be valid.
        //If any check fails, explains why
        public void Change(int changeType, int deviceId = int.MinValue, dynamic arg1 = null)
        {
            if (deviceId == int.MinValue)
            {
                deviceId = GetIdFromUser(true, true);
            }
            if (deviceId != int.MinValue)
            {
                if (arg1 == null)
                {
                    switch (changeType)
                    {
                        case 0:
                            {
                                Console.Write("Please input a new id: ");
                                arg1 = GetIdFromUser(false, false, true);
                                if (arg1 != int.MinValue) {
                                    _listOfGroups[GetGroupIndexById(deviceId)].Change(changeType, deviceId, arg1);
                                }
                            }
                            break;
                        case 1:
                            {
                                Console.Write("Please input a new name: ");
                                _listOfGroups[GetGroupIndexById(deviceId)].Change(changeType, deviceId, Console.ReadLine());
                            }
                            break;
                        default: _listOfGroups[GetGroupIndexById(deviceId)].Change(changeType, deviceId, arg1); break;
                    }
                }
                else {
                    _listOfGroups[GetGroupIndexById(deviceId)].Change(changeType, deviceId, arg1);
                    if (changeType == 0) { PrintStatus(arg1); }
                }
            }
        }

        //Asks user for device id, if device exists, print the status of the device.
        public void PrintStatus(int deviceId = int.MinValue)
        {
            if (deviceId == int.MinValue) {
                deviceId = GetIdFromUser(true);
            }
            if (deviceId != int.MinValue)
            {
                Console.WriteLine(GetDeviceById(deviceId).GetCurrentState());
            }

        }

        //Opens, closes, locks, unlocks, force opens and tags a door as open for too long.
        //If called without arguments, asks user for device id.
        //Passes arguments to Group.ChangeDoorStatus().
        public void ChangeDoorStatus(int doorChangeType, int deviceId = int.MinValue)
        {
            if (deviceId == int.MinValue)
            {
                deviceId = GetIdFromUser(true, true);
            }
            if (deviceId != int.MinValue) { _listOfGroups[GetGroupIndexById(deviceId)].ChangeDoorStatus(doorChangeType, deviceId); }

        }

        //If device with deviceId does not exist, return true
        //If device with deviceId does exist, return false
        public bool IsIdUnique(int deviceId)
        {
            if (GetDeviceById(deviceId) == null) { return true; }
            return false;
        }

        //Asks user for id, outputs a correct id. Returns are dependent on shouldBeUnique and shouldExist - either returns a valid id or int.MinValue.
        //Asks user for id, checks if integer, checks if exists.
        //If shouldBeUnique is true, return id if id does not exist.
        //If shouldBeUnique is false, return int.MinValue if id exists.
        //shouldExist changes error messages
        //If any check fails, explains why
        public int GetIdFromUser(bool shouldBeUnique, bool shouldExist = false, bool customInteraction = false) 
        {
            if (!customInteraction) 
            {
                Console.WriteLine("Please input device id.");
                Console.WriteLine("Id has to be an integer and cannot have the value of {0}.", int.MinValue);
                Console.Write("Device id: ");
            }
            try
            {
                int deviceId = int.Parse(Console.ReadLine());
                if (shouldBeUnique)
                {
                    if (!IsIdUnique(deviceId)) { return deviceId; }
                    else
                    {
                        if (shouldExist) { Console.WriteLine("Device with id {0} does not exist.", deviceId); }
                        else { Console.WriteLine("Device with id {0} exists.", deviceId); }
                    }
                }
                else
                {
                    if (IsIdUnique(deviceId)) { return deviceId; }
                    else
                    {
                        if (shouldExist) { Console.WriteLine("Device with id {0} does not exist.", deviceId); }
                        else { Console.WriteLine("Device with id {0} exists.", deviceId); }
                    }
                }
                

            }
            catch (FormatException)
            {
                Console.WriteLine("Device id has to be an integer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return int.MinValue;
        }

        //Gets group index from user if group exists
        //Asks user for group index, chceks if int, checks if exists, returns group index
        //If any check fails, explains why.
        public int GetGroupIndexFromUser(bool customInteraction = false)
        {
            if (!customInteraction)
            {
                Console.WriteLine("Please input number of the group.");
                Console.WriteLine("Group number has to be an integer and has to be more than or equal to zero.");
                Console.Write("Group number: ");
            }

            try
            {
                int groupNumber = int.Parse(Console.ReadLine());
                if (groupNumber < 0) 
                {
                    Console.WriteLine("Group number has to be more than or equal to zero.");
                }
                else
                {
                    if (DoesGroupExist(groupNumber)) { return groupNumber; }
                }
                
            }
            catch (FormatException)
            {
                Console.WriteLine("Group number has to be an integer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return int.MinValue;
        }
    }
}
