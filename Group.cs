using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rec
{
    class Group
    {
        public List<AbsDevice> _listOfDevices = new List<AbsDevice>();

        //Creates a device with arguments passed from Root.CreateDevice() and adds it into the _listOfDevices
        //Any device type specific attributes are handled here - ammonunt of attributes, their datatype, and validity
        public void CreateDevice(int deviceType, int deviceId, string deviceName, dynamic arg1, dynamic arg2) 
        {
            switch (deviceType) 
            {
                case 0: _listOfDevices.Add(new Alarm(deviceId, deviceName)); break;

                case 1: 
                    {
                        string accessCardNumber;
                        if (arg1 == null)
                        {
                            accessCardNumber = GetAccessCardNumberFromUser();
                        }
                        else { accessCardNumber = arg1; }
                        
                        if (IsAccessCardNumberValid(accessCardNumber)) { 
                            _listOfDevices.Add(new CardReader(deviceId, deviceName, accessCardNumber));
                        }
                    }
                    break;

                case 2: _listOfDevices.Add(new Door(deviceId, deviceName)); break;

                case 3:
                    {
                        string message;
                        if (arg1 == null)
                        {
                            Console.Write("Please input the message of the led panel: ");
                            message = Console.ReadLine();
                        }
                        else { message = arg1; }
                        _listOfDevices.Add(new LedPanel(deviceId, deviceName, message));  //No need to check the validity of a message since any input is valid.
                    }
                    break;

                case 4:
                    {
                        if (arg1 == null)
                        {
                            arg1 = GetSoundFromUser();
                            if (arg1 != (Enums.Sound)3) 
                            { 
                                arg2 = GetVolumeFromUser(); 
                            }
                            
                        }
                        if (arg1 != (Enums.Sound)3 && arg2 != float.MinValue) 
                        {
                            _listOfDevices.Add(new Speaker(deviceId, deviceName, arg1, arg2)); 
                        }
                    }
                    break;
            }
        }

        //Removes a device.
        //Checks if a device exists before removing it.
        //If any check fails, explains why.
        public void RemoveDevice(int deviceId, bool silently = false) 
        {
            int deviceIndex = GetDeviceIndexById(deviceId, silently);
            if (deviceIndex != int.MinValue) 
            {
                _listOfDevices.RemoveAt(deviceIndex);
                if (!silently) { Console.WriteLine("Device with id {0} removed.", deviceId); }
            }
        }

        //Changes any attribute of any device.
        //Checks for their validity.
        //If any check fails, explains why.
        public void Change(int changeType, int deviceId, dynamic arg1) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            switch (changeType) 
            {
                case 0: 
                    { 
                        device.Id = arg1;
                        Console.WriteLine(device.GetCurrentState());
                    } 
                    break;
                case 1:
                    {
                        device.Name = arg1;
                        Console.WriteLine(device.GetCurrentState());
                    }
                    break;
                case 2: 
                    {
                        if (device.Type == "LedPanel")
                        {
                            LedPanel ledPanel = (LedPanel)device;
                            if (arg1 == null)
                            {
                                Console.Write("Please input a new message: ");
                                ledPanel.Message = Console.ReadLine();
                            }
                            else 
                            {
                                ledPanel.Message = arg1;
                            }
                            Console.WriteLine(ledPanel.GetCurrentState());
                        }
                        else { Console.WriteLine("Device is not a led panel."); }
                    } 
                    break;
                case 3:
                    {
                        if (device.Type == "Speaker")
                        {
                            Speaker speaker = (Speaker)device;
                            if (arg1 == null)
                            {
                                Console.WriteLine("Sound inputs are 0 - None, 1 - Music, 2 - Alarm ");
                                Console.Write("Please input a new sound: ");
                                Enums.Sound sound = GetSoundFromUser(true);
                                if (sound != (Enums.Sound)3) { 
                                    speaker.Sound = sound;
                                }
                            }
                            else
                            {
                                speaker.Sound = arg1;
                            }
                            Console.WriteLine(speaker.GetCurrentState());
                        }
                        else { Console.WriteLine("Device is not a speaker."); }
                    }
                    break;
                case 4:
                    {
                        if (device.Type == "Speaker")
                        {
                            Speaker speaker = (Speaker)device;
                            if (arg1 == null)
                            {
                                Console.WriteLine("Please input a new volume as 0,0 or as 0. Volume cannot be less than zero.");
                                Console.Write("Please input a new volume: ");
                                float volume = GetVolumeFromUser(true);
                                if (volume != float.MinValue) { 
                                    speaker.Volume = volume;
                                }
                            }
                            else
                            {
                                speaker.Volume = arg1;
                            }
                            Console.WriteLine(speaker.GetCurrentState());
                        }
                        else { Console.WriteLine("Device is not a speaker."); }
                    }
                    break;
                case 5:
                    {
                        if (device.Type == "CardReader")
                        {
                            CardReader cardReader = (CardReader)device;
                            if (arg1 == null)
                            {
                                Console.WriteLine("Please input a new access card number. This number has to have even length of less than or equal to 16, and has to be made up with hexadecimal numbers.");
                                Console.Write("Please input a new access card number: ");
                                string accessCardNumber = GetAccessCardNumberFromUser(true);
                                if (IsAccessCardNumberValid(accessCardNumber)) { 
                                    cardReader.AccessCardNumber = accessCardNumber;
                                }
                            }
                            else
                            {
                                cardReader.AccessCardNumber = arg1;
                            }
                            Console.WriteLine(cardReader.GetCurrentState());
                        }
                        else { Console.WriteLine("Device is not a card reader."); }
                    }
                    break;
            }
        }

        //Opens, closes, locks, unlocks, opens forcibly and tags open for too long any door.
        //If a device with deviceId is not a door, explains.
        public void ChangeDoorStatus(int doorChangeType, int deviceId) 
        {
            AbsDevice device = GetDeviceById(deviceId);
            if (device.Type == "Door")
            {
                Door door = (Door)device;
                switch (doorChangeType) 
                {
                    case 0: door.Lock(); break;
                    case 1: door.Unlock();  break;
                    case 2: door.Open(); break;
                    case 3: door.Close(); break;
                    case 4: door.OpenForTooLong(); break;
                    case 5: door.OpenForcibly(); break;
                }
            }
            else
            {
                Console.WriteLine("Device is not a door.");
            }
        }

        //Gets a vaild volume setting from the user.
        //Asks user for a volume, checks if float, checks is more than or equal to zero, return volume.
        //If check fails, explains why, returns float.MinValue.
        public float GetVolumeFromUser(bool customInteraction = false) 
        {
            float volume;
            if (!customInteraction) 
            {
                Console.WriteLine("Please input the volume of the speaker.");
                Console.WriteLine("Please input your number as 0,0 or as 0. Volume cannot be less than zero.");
                Console.Write("Volume: ");
            }
            try
            {
                volume = float.Parse(Console.ReadLine());
                if (volume >= 0)
                {
                    return volume;
                }
                else
                {
                    Console.WriteLine("Volume cannot be less than zero.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Volume has to be a float.");
            }
            return float.MinValue;
        }

        //Gets a string from user and outputs it with .ToUpper().
        public string GetAccessCardNumberFromUser(bool customInteraction = false) 
        {
            if (!customInteraction) {
                Console.WriteLine("Please input an access card number. This number has to have even length of less than or equal to 16, and has to be made up with hexadecimal numbers.");
                Console.Write("Access card number: ");
            }
            return Console.ReadLine().ToUpper();
        }

        //Gets a vaild sound setting from the user.
        //Asks user for a sound, checks if valid, return sound.
        //If check fails, explains why, returns (Enums.Sound)4.
        public Enums.Sound GetSoundFromUser(bool customInteraction = false) 
        {
            Enums.Sound sound;
            if (!customInteraction) 
            {
                Console.WriteLine("Please input the sound the speaker will play.");
                Console.WriteLine("Valid inputs are 0 - None, 1 - Music, 2 - Alarm");
                Console.Write("Volume: ");
            }
            sound = SoundInputToSound(Console.ReadLine());
            if (sound == (Enums.Sound)3) { Console.WriteLine("Sound input is incorrect."); }
            return sound;
        }

        //Checks if accessCardNumber is a valid access card number
        //If valid, return true.
        //If invalid, return false, explains why.
        public bool IsAccessCardNumberValid(string accessCardNumber) 
        {
            if (accessCardNumber.Length <= 16)
            {
                if (accessCardNumber.Length % 2 == 0)
                {
                    Regex rgx = new Regex("^[0123456789ABCDEF]*$");
                    if (rgx.IsMatch(accessCardNumber))
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Access card number has to be made up from numbers ranging from 0 to 9 and letters ranging from A to F.");
                    }
                }
                else
                {
                    Console.WriteLine("Access card numbers length has to be even.");
                }
            }
            else 
            {
                Console.WriteLine("Access card numbers length has to be equal to or less than 16.");
            }
            return false;
        }

        //Takes a string, if string matches with hardcoded strings, returns their value translated to (Enums.Sound)
        //If does not match, return (Enums.Sound)3
        public Enums.Sound SoundInputToSound(string soundInput) 
        {
            switch (soundInput.ToLower()) 
            {
                case "0": case "none": return (Enums.Sound)0;
                case "1": case "music": return (Enums.Sound)1;
                case "2": case "alarm": return (Enums.Sound)2;
                default: return (Enums.Sound)3;
            }
        }

        //Returns a device with matching id
        //If no device exists, explains why, return null
        public AbsDevice GetDeviceById(int deviceId, bool silently = false) 
        {
            foreach (AbsDevice device in _listOfDevices)
            {
                if (device.Id == deviceId) 
                {
                    return device;
                }
            }
            if (!silently) { Console.WriteLine("There is no device with id {0} in this group.", deviceId); }
            return null;
        }

        //Returns the device index of a device with matching id
        //If there is no device with matching id in this group, return int.MinValue.
        public int GetDeviceIndexById(int deviceId, bool silently = false) 
        {
            AbsDevice device = GetDeviceById(deviceId, silently);
            if (device != null) { return _listOfDevices.IndexOf(device); }
            return int.MinValue;
        }
    }
}
