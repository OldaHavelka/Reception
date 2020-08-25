using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Reception
{
    class Root
    {
        public delegate void PrintTreeEventHandler(object source, EventArgs args);

        public event PrintTreeEventHandler TreeToBePrinted;

        readonly Events PrintTreeEvent = new Events();

        //Stores all groups
        public static List<Group> ListOfGroups { get; } = new List<Group>();

        //Creates a group
        //Adds a new group into ListOfGroups
        //Calls OnPrintTree
        public void CreateGroup() 
        {
            ListOfGroups.Add(new Group());
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            OnPrintTree(this, EventArgs.Empty);
        }

        //Removes a group
        //Removes a group from ListOfGroups at GroupIndex
        //Calls OnPrintTree
        public void RemoveGroup(int groupIndex) 
        {
            ListOfGroups.RemoveAt(groupIndex);
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            OnPrintTree(this, EventArgs.Empty);
        }

        //Creates a device
        //Creates a device, fills it with properties of the device, adds it into group.ListOfDevices in the ListOfGroups[groupIndex]
        //Calls OnPrintTree
        public void CreateDevice(int groupIndex, int deviceType, int deviceId, string deviceName, List<String> additionalProperties) 
        {
            switch (deviceType) 
            {
                case 0: ListOfGroups[groupIndex].ListOfDevices.Add(new Alarm(deviceId, deviceName)); break;
                case 1: ListOfGroups[groupIndex].ListOfDevices.Add(new CardReader(deviceId, deviceName, additionalProperties[0])); break;
                case 2: ListOfGroups[groupIndex].ListOfDevices.Add(new Door(deviceId, deviceName)); break;
                case 3: ListOfGroups[groupIndex].ListOfDevices.Add(new LedPanel(deviceId, deviceName, additionalProperties[0])); break;
                case 4: ListOfGroups[groupIndex].ListOfDevices.Add(new Speaker(deviceId, deviceName, (Speaker.Sounds)int.Parse(additionalProperties[0]), float.Parse(additionalProperties[1]))); break;
            }
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            OnPrintTree(this, EventArgs.Empty);
        }

        //Removes a device
        //Find the groupIndex and deviceIndex of a device if exists, removes the device from group.ListOfDevices in the ListOfGroups[groupIndex]
        //If device does not exist, throws DeviceDoesNotExistException
        //If silently is not false, calls OnPrintTree
        public void RemoveDevice(int deviceId, bool silently = false) 
        {
            int[] indexes = GetGroupIndexAndDeviceIndexByDeviceId(deviceId);
            if (indexes == null) { throw new DeviceDoesNotExistException(); }
            ListOfGroups[indexes[0]].ListOfDevices.RemoveAt(indexes[1]);
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            if (!silently) { OnPrintTree(this, EventArgs.Empty); }
        }

        //Changes a property of a device
        //Finds a device by its id if exists, checks if the device has a property that is to be changed, changes property
        //If device does not exist, throws DeviceDoesNotExistException
        //If device is not of a correct type, throws InvalidCastException()
        public void Change(int changeType, int deviceId, dynamic property) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            if (device == null) { throw new DeviceDoesNotExistException(); }
            switch (changeType) 
            {
                case 0: device.Id = int.Parse(property); break;
                case 1: device.Name = property; break;
                case 2: 
                    {
                        if (device.Type == (AbsDevice.Types)1)
                        {
                            CardReader cardReader = (CardReader)device;
                            cardReader.AccessCardNumber = property;
                        }
                        else { throw new InvalidCastException(); }
                    } 
                    break;
                case 3:
                    {
                        if (device.Type == (AbsDevice.Types)3)
                        {
                            LedPanel ledPanel = (LedPanel)device;
                            ledPanel.Message = property;
                        }
                        else { throw new InvalidCastException(); }
                    }
                    break;
                case 4:
                    {
                        if (device.Type == (AbsDevice.Types)4)
                        {
                            Speaker speaker = (Speaker)device;
                            if (property is string) { property = int.Parse(property); }
                            speaker.Sound = (Speaker.Sounds)property;
                        }
                        else { throw new InvalidCastException(); }
                    }
                    break;
                case 5:
                    {
                        if (device.Type == (AbsDevice.Types)4)
                        {
                            Speaker speaker = (Speaker)device;
                            if (property is string) { property = float.Parse(property); }
                            speaker.Volume = property;
                        }
                        else { throw new InvalidCastException(); }
                    }
                    break;
            }
        }

        //Moves a device into a group
        //Finds a device by its id if exists, removes the device if exists, adds the device into ListOfGroups[newGroupIndex], calls OnPrintTree
        //If device does not exist, throws DeviceDoesNotExistException
        public void Move(int deviceId, int newGroupIndex) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            RemoveDevice(deviceId, true);
            ListOfGroups[newGroupIndex].ListOfDevices.Add(device);
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            OnPrintTree(this, EventArgs.Empty);
        }

        //Prints the status of a device
        //Finds a device by its id if exists, returns device.GetCurrentState()
        //If device does not exist, throws DeviceDoesNotExistException
        public string PrintStatus(int deviceId) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            if (device == null) { throw new DeviceDoesNotExistException(); }
            return device.GetCurrentState();
        }

        //Changes the status of a door
        //Gets device by its id if exists, checks if device is a door, sets a state to true or false dependig on changeType
        //If device does not exist, throws DeviceDoesNotExistException
        //If device is not a door, throws InvalidCastException
        public void ChangeDoorStatus(int changeType, int deviceId) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            if (device == null) { throw new DeviceDoesNotExistException(); }
            if (device.Type == (AbsDevice.Types)2)
            {
                Door door = (Door)device;
                switch (changeType)
                {
                    case 0: door.Locked = false; break;
                    case 1: door.Locked = true; break;
                    case 2: door.Open = true; break;
                    case 3: door.Open = false; break;
                    case 4: door.OpenForTooLong = true; break;
                    case 5: door.OpenForTooLong = false; break;
                    case 6: door.OpenedForcibly = true; break;
                    case 7: door.OpenedForcibly = false; break;
                }
            }
            else { throw new InvalidCastException(); }
        }

        //Calls OnPrintTree
        public void PrintTree() 
        {
            TreeToBePrinted = PrintTreeEvent.OnPrintTree;
            OnPrintTree(this, EventArgs.Empty);
        }

        //Returns a device by its id
        //Checks the ListOfGroups for all groups and then checks all groups for all device ids
        //First deviceId and device.Id match returns device
        //No matches returns null
        private AbsDevice GetDeviceById(int deviceId) 
        {
            foreach (Group group in ListOfGroups)
            {
                foreach (AbsDevice device in group.ListOfDevices)
                {
                    if (deviceId == device.Id) { return device; }
                }
            }
            return null;
        }

        //Returns group index and device index by device id
        //Checks the ListOfGroups for all groups and then checks all groups for all device ids
        //First deviceId and device.Id match returns {groupIndex, deviceIndex}
        //No matches returns null
        private int[] GetGroupIndexAndDeviceIndexByDeviceId(int deviceId) 
        {
            for (int i = 0; i < ListOfGroups.Count; i++) 
            {
                for (int ii = 0; ii < ListOfGroups[i].ListOfDevices.Count; ii++) 
                {
                    if (deviceId == ListOfGroups[i].ListOfDevices[ii].Id) {
                        int[] returnable = { i, ii };
                        return returnable;
                    }
                }
            }
            return null;
        }

        //Returns false if id exists
        //Checks the ListOfGroups for all groups and then checks all groups for all device ids
        //First deviceId and device.Id match returns false
        //No matches returns true
        public static bool IsIdUnique(int deviceId) 
        {
            foreach (Group group in ListOfGroups)
            {
                foreach (AbsDevice device in group.ListOfDevices)
                {
                    if (deviceId == device.Id) { return false; }
                }
            }
            return true;
        }

        protected virtual void OnPrintTree(object source, EventArgs e)
        {
            TreeToBePrinted?.Invoke(this, EventArgs.Empty);
        }
    }

    [Serializable]
    internal class DeviceDoesNotExistException : Exception
    {
        const string deviceDoesNotExistMessage = "Device does not exist.";
        public DeviceDoesNotExistException() : base(deviceDoesNotExistMessage)
        {
        }

        public DeviceDoesNotExistException(string message) : base(message + " - " + deviceDoesNotExistMessage)
        {
        }

        public DeviceDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeviceDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
