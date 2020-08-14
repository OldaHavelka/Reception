using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

//There are far too many lines for my liking. There *has* to be a way to make this far simpler.
//There is probably a way to put many functions into just one or a few ones.

namespace Reception
{
    class Group
    {

        //This list holds all devies in this group.
        private List<AbsDevice> _listOfDevices = new List<AbsDevice>();
        public List<AbsDevice> GetListOfDevices() { return _listOfDevices; }

        //Group only contains one list of devices: List<AbsDevice> _listOfDevices
        public Group()
        {     
        }

        //Creates a device with all its parameters.
        //CreateDevice from Root.cs has passed parameters from AbsDevice.cs.
        //This CreateDevice asks the user for parameters specific to its type.
        //Inserts said device into this list of devices : List<AbsDevice> _listOfDevices
        //Type of device created depends on the first int argument.
        public void CreateDevice(int deviceType, int id, string deviceName) 
        {
            switch (deviceType)
            {
                case 0: _listOfDevices.Add(new Alarm(id, deviceName)); break;
                case 1:
                    {
                        string accessCardNumber = UserInputUntilValidAccessCardNumber();
                        _listOfDevices.Add(new CardReader(id, deviceName, accessCardNumber));
                        break;
                    }
                case 2: 
                    {
                        _listOfDevices.Add(new Door(id, deviceName, 0));
                        break;
                    }
                case 3: 
                    {
                        Console.Write("What message would you like for your LedPanel to have?: ");
                        _listOfDevices.Add(new LedPanel(id, deviceName, Console.ReadLine()));
                        break;
                    }
                case 4: 
                    {
                        Console.WriteLine("What sound would you like for your Speaker to play?: ");
                        Console.Write("None (0), Music (1), Alarm (2)");
                        Enums.Sound sound = UserInputUntilValidSound();
                        Console.WriteLine("How loud would you like for your Speaker to play?: ");
                        Console.Write("Please type in the number as \"0.0\": ");
                        _listOfDevices.Add(new Speaker(id, deviceName, sound, UserInputUntilValidVolume()));
                        break; 
                    }
                default: Console.WriteLine("Device type {0} doesn't exist.", deviceType); break;
            }
        }

        //Asks user to input a value for sound until accepted and passed forward.
        //Also explains incorrect inputs.
        private Enums.Sound UserInputUntilValidSound() 
        {
            string soundInput;
            do
            {
                soundInput = Console.ReadLine();
                switch (soundInput)
                {
                    case "None": case "0": return 0;
                    case "Music": case "1": return (Enums.Sound)1;
                    case "Alarm": case "2": return (Enums.Sound)2;
                    default: Console.Write("Please select a value from 0 to 2 or None, Music or Alarm: "); break;
                }
            } while (true);
        }

        private bool IsSoundValid(dynamic soundInput) 
        {
            switch (soundInput) 
            {
                case (Enums.Sound)0: case (Enums.Sound)1: case (Enums.Sound)2: return true;
                default: Console.WriteLine("Sound is not valid."); return false;
            }
        }

        //Asks user to input a value for volume until accepted and passed forward.
        //Also explains incorrect inputs.
        private float UserInputUntilValidVolume()
        {
            bool failure;
            string volumeInput;
            do 
            {
                failure = false;
                volumeInput = Console.ReadLine();
                try { float.Parse(volumeInput); }
                catch (FormatException)
                {
                    failure = true;
                    Console.WriteLine("Provided input is not correct format.");
                    continue;
                }
            } while (failure);
            return float.Parse(volumeInput);
        }

        private bool IsVolumeValid(dynamic dvolume) 
        {
            try { float volume = dvolume; }
            catch
            {
                Console.WriteLine("Provided input is not float.");
                return false;
            }
            return true;
        }

        //Asks user to input a value for device index until accepted and passed forward.
        //Also explains incorrect inputs.
        public int UserInputUntilValidDeviceIndex()
        {
            bool failure;
            string deviceIndexInput;
            do
            {
                failure = false;
                deviceIndexInput = Console.ReadLine();
                try { int.Parse(deviceIndexInput); }
                catch (FormatException)
                {
                    Console.WriteLine("Provided input is not an integer.");
                    failure = true;
                    continue;
                }
                if (!DoesDeviceExist(int.Parse(deviceIndexInput))) { failure = true; }
            } while (failure);
            return int.Parse(deviceIndexInput);
        }

        public bool IsDeviceIndexValid(dynamic index) 
        {
            try { int.Parse(index); }
            catch (FormatException)
            {
                Console.WriteLine("Provided input is not an integer.");
                return false;
            }
            if (!DoesDeviceExist(int.Parse(index))) { return false; }
            return true;
        }

        //Asks user to input a value for access card number until accepted and passed forward.
        //Also explains incorrect inputs.
        private string UserInputUntilValidAccessCardNumber() 
        {
            string accessCardNumber;
            do
            {
                accessCardNumber = Console.ReadLine();
            } while (IsAccessCardNumberValid(accessCardNumber));
            return accessCardNumber;
        }

        private bool IsAccessCardNumberValid(dynamic accessCardNumber) 
        {
            if (accessCardNumber.Length > 16)
            {
                Console.WriteLine("Access card number is too long. The maximum access card number length is 16.");
                return false;
            }
            if (accessCardNumber.Length % 2 == 1)
            {
                Console.WriteLine("Input does not contain an even ammount of hexadecimal numbers.");
                return false;
            }
            Regex rgx = new Regex("^[0123456789ABCDEF]*$");
            if (!rgx.IsMatch(accessCardNumber))
            {
                Console.WriteLine("Input is not a hexadecimal number. Please use numbers from 0-9 and letters A-F.");
                return false;
            }
            return true;
        }

        //Checks if there are any devies in this group.
        //If there are any, return true
        //If there are none, return false
        public bool AreThereAnyDevices()
        {
            if (_listOfDevices.Count == 0)
            {
                Console.WriteLine("There are no devices.\nPlease, create a device.");
                return false;
            }
            return true;
        }


        //Checks if a certain device exists
        //If it does not, return false
        //If not, explain why so.
        //If it does, return true
        public bool DoesDeviceExist(int deviceIndex)
        {

            if (_listOfDevices.Count - 1 < deviceIndex || deviceIndex < 0)
            {
                Console.WriteLine("The ammount of devices is {0}. Please select a device ranging from 0 to {1}.", _listOfDevices.Count, _listOfDevices.Count - 1);
                return false;
            }
            return true;
        }


        //Asks user to change AccessListNumber in a chosen CardReader and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeCardReaderAccessListNumber() 
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "CardReader")
                    {
                        CardReader device = (CardReader)_listOfDevices[deviceIndex];
                        Console.Write("What access card number would you like for your CardReader to have?: ");
                        device.SetAccessCardNumber(UserInputUntilValidAccessCardNumber());
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a CardReader", deviceIndex);
                    }
                }
            }            
        }

        //Asks user to change Message in a chosen LedPanel and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeLedPanelMessage()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "LedPanel")
                    {
                        LedPanel device = (LedPanel)_listOfDevices[deviceIndex];
                        Console.Write("What message would you like for your LedPanel to have?: ");
                        device.SetMessage(Console.ReadLine());
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a LedPanel", deviceIndex);
                    }
                }
            }
        }

        //Asks user to change Volume in a chosen Speaker and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeSpeakerVolume()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Speaker")
                    {
                        Speaker device = (Speaker)_listOfDevices[deviceIndex];
                        Console.Write("How loud would you like for your Speaker to be?: ");
                        device.SetVolume(UserInputUntilValidVolume());
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Speaker", deviceIndex);
                    }
                }
            }
        }

        //Asks user to change Sound in a chosen Speaker and then does so.
        //Any errors are explained to the user if they occur and then the user will be asked again.
        public void ChangeSpeakerSound()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Speaker")
                    {
                        Speaker device = (Speaker)_listOfDevices[deviceIndex];
                        Console.Write("What sound would you like for your Speaker to play?: ");
                        device.SetSound(UserInputUntilValidSound());
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Speaker", deviceIndex);
                    }
                }
            }
        }

        //Removes a device based on users input.
        //If there are no devices or a non-existing device is being pointed at, user will be warned and asked again.
        public void RemoveDevice()
        {
            Console.Write("What device is it?: ");
            int deviceIndex = UserInputUntilValidDeviceIndex();
            _listOfDevices.RemoveAt(deviceIndex);
            Console.WriteLine("Device {0} removed.", deviceIndex);
        }

        //Asks user what door he wants to open and then proceeds to open it if possible.
        //Any errors are explained
        public void OpenDoor()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.Open();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Asks user what door he wants to close and then proceeds to close it if possible.
        //Any errors are explained
        public void CloseDoor()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.Close();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Asks user what door he wants to lock and then proceeds to lock it if possible.
        //Any errors are explained
        public void LockDoor()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.Lock();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Asks user what door he wants to unlock and then proceeds to unlock it if possible.
        //Any errors are explained
        public void UnlockDoor()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.Unlock();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Asks user what door he wants to open forcibly and then proceeds to open it forcibly if possible.
        //Any errors are explained
        public void OpenDoorForcibly()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.OpenForcibly();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Asks user what door he wants to tag as opened for too long and then proceeds to tag it if possible.
        //Any errors are explained
        public void DoorOpenedForTooLong()
        {
            if (AreThereAnyDevices())
            {
                Console.Write("What device is it?: ");
                int deviceIndex = UserInputUntilValidDeviceIndex();
                if (DoesDeviceExist(deviceIndex))
                {
                    if (_listOfDevices[deviceIndex].GetDeviceType() == "Door")
                    {
                        Door device = (Door)_listOfDevices[deviceIndex];
                        device.OpenForTooLong();
                        Console.WriteLine(device.GetCurrentState());
                    }
                    else
                    {
                        Console.WriteLine("Device {0} is not a Door", deviceIndex);
                    }
                }
            }
        }

        //Creates device without any user input.
        public void CreateDevice(int deviceType, int id, string deviceName, dynamic arg1 = null, dynamic arg2 = null) 
        {
            switch (deviceType)
            {
                case 0: _listOfDevices.Add(new Alarm(id, deviceName)); break;
                case 1:
                    {
                        if (IsAccessCardNumberValid(arg1)) 
                        {
                            _listOfDevices.Add(new CardReader(id, deviceName, arg1)); 
                        }
                        break;
                    }
                case 2:
                    {
                        _listOfDevices.Add(new Door(id, deviceName, 0));
                        break;
                    }
                case 3:
                    {
                        _listOfDevices.Add(new LedPanel(id, deviceName, arg1));
                        break;
                    }
                case 4:
                    {
                        if (IsSoundValid(arg1) && IsVolumeValid(arg2)) { _listOfDevices.Add(new Speaker(id, deviceName, arg1, arg2)); }
                        break;
                    }
                default: Console.WriteLine("Device type {0} doesn't exist.", deviceType); break;
            }
        }

        public void RemoveDevice(int deviceIndex) 
        {
            if (DoesDeviceExist(deviceIndex)) 
            {
                _listOfDevices.RemoveAt(deviceIndex);
                Console.WriteLine("Device {0} deleted.", deviceIndex);
            }
        }
    }
}
