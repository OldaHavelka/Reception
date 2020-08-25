using System;
using System.Collections.Generic;
using System.Threading;

namespace Reception
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> additionalProperties = new List<String>();

            Root root = new Root();

            Console.WriteLine("Creating a new group.");
            CreateGroup();
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating a new group.");
            CreateGroup();
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating alarm.");
            CreateDevice(0, 0, 0, "Alarm0");
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating ledpanel.");
            additionalProperties.Add("I AM A LED PANEL CONTAINING A MESSAGE. I WILL BE REWRITTEN SOON.");
            CreateDevice(3, 0, 1, "LedPanel0", additionalProperties);
            additionalProperties.Clear();
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating door.");
            CreateDevice(2, 0, 2, "Door0");
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating speaker.");
            additionalProperties.Add("0");
            additionalProperties.Add("100,1");
            CreateDevice(4, 0, 3, "Speaker0", additionalProperties);
            additionalProperties.Clear();
            Thread.Sleep(1500);

            Console.WriteLine("\nRemoving Alarm0.");
            RemoveDevice(0);
            Thread.Sleep(1500);

            Console.WriteLine("\nRenaming LedPanel0 to aVeryImportantLedPanel0");
            Change(1, 1, "aVeryImportandLedPanel0");
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging the message of aVeryImportantLedPanel0");
            Change(3, 1, "NEW MESSAGE!");
            Thread.Sleep(1500);

            Console.WriteLine("\nMoving aVeryImportantLedPanel0 to group 1");
            Move(1, 1);
            Thread.Sleep(1500);

            Console.WriteLine("\nOpening Door0.");
            ChangeDoorStatus(2, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nClosing Door0.");
            ChangeDoorStatus(3, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging the sound of Speaker0 to Music");
            Change(4, 3, 1);
            Thread.Sleep(1500);

            //Interactive mode begins here
            string[] nextCommand;
            Console.WriteLine("\nInteractive mode begins.\nType help for help.");
            do
            {
                Console.Write("Next command: ");
                nextCommand = Console.ReadLine().ToLower().Split(" ");
                try
                {
                    switch (nextCommand[0])
                    {
                        case "create":
                            {
                                switch (nextCommand[1])
                                {
                                    case "group": if (CreateGroup()) { Console.WriteLine("Group created."); } continue;
                                    case "alarm": if (CreateDevice(0)) { Console.WriteLine("Alarm created."); } continue;
                                    case "cardreader": if (CreateDevice(1)) { Console.WriteLine("Card reader created."); } continue;
                                    case "door": if (CreateDevice(2)) { Console.WriteLine("Door created."); } continue;
                                    case "ledpanel": if (CreateDevice(3)) { Console.WriteLine("Led panel created."); } continue;
                                    case "speaker": if (CreateDevice(4)) { Console.WriteLine("Speaker created."); } continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "remove":
                            {
                                switch (nextCommand[1])
                                {
                                    case "group": if (RemoveGroup()) { Console.WriteLine("Group removed."); } continue;
                                    case "device": if (RemoveDevice()) { Console.WriteLine("Device removed."); } continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "change":
                            {
                                switch (nextCommand[1])
                                {
                                    case "id": if (Change(0)) { Console.WriteLine("ID changed."); } continue;
                                    case "name": if (Change(1)) { Console.WriteLine("Name changed."); } continue;
                                    case "accesscardnumber": if (Change(2)) { Console.WriteLine("Access card number changed."); } continue;
                                    case "message": if (Change(3)) { Console.WriteLine("Message changed."); } continue;
                                    case "sound": if (Change(4)) { Console.WriteLine("Sound changed."); } continue;
                                    case "volume": if (Change(5)) { Console.WriteLine("Volume changed."); } continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "move": if (Move()) { Console.WriteLine("Device moved."); } continue;
                        case "printtree": root.PrintTree(); continue;   //no user input required
                        case "printstatus": PrintStatus(); continue;
                        case "lock": if (ChangeDoorStatus(0)) { Console.WriteLine("Door locked."); } continue;
                        case "unlock": if (ChangeDoorStatus(1)) { Console.WriteLine("Door unlocked."); } continue;
                        case "open": if (ChangeDoorStatus(2)) { Console.WriteLine("Door opened."); } continue;
                        case "close": if (ChangeDoorStatus(3)) { Console.WriteLine("Door closed."); } continue;
                        case "openfortoolong": if (ChangeDoorStatus(4)) { Console.WriteLine("Door tagged as open for too long."); } continue;
                        case "nolongeropenfortoolong": if (ChangeDoorStatus(5)) { Console.WriteLine("Door is no longer tagged as open for too long."); } continue;
                        case "openedforcibly": if (ChangeDoorStatus(6)) { Console.WriteLine("Door tagged as opened forcibly."); } continue;
                        case "nolongeropenedforcibly": if (ChangeDoorStatus(7)) { Console.WriteLine("Door is no longer tagged as opened forcibly."); } continue;
                        case "help":
                            {
                                Console.WriteLine(
                                    "create > creates [group, alarm, cardreader, door, ledpanel, speaker]\n" +
                                    "remove > removes [group, device]\n" +
                                    "change > changes [id, name, message, accesscardnumber, volume, sound] of a device\n" +
                                    "move > moves a device from a group to another group\n" +
                                    "printtree > prints out a representation of the entire tree\n" +
                                    "printstatus > prints out the details of a device\n" +
                                    "lock > locks a door\n" +
                                    "unlock > unlocks a door\n" +
                                    "open > opens a door\n" +
                                    "close > closes a door\n" +
                                    "openfortoolong > tags a door as open for too long\n" +
                                    "nolongeropenfortoolong > tags a door as no longer open for too long\n" +
                                    "openedforcibly > tags a door as opened forcibly\n" +
                                    "nolongeropenedforcibly > tags a door as no longer opened forcibly\n" +
                                    "exit > exits application\n" +
                                    "ID: has to be an integer ranging from {0} to {1}", int.MinValue, int.MaxValue + "\n" +
                                    "name: can be any string including null\n" +
                                    "message: can be any string including null\n" +
                                    "access card number: has to have even length, has to have length equal to or less than 16, has to be made up from digits 0-9 and letters a-f\n" +
                                    "sound: can have values of 0 (None), 1 (Music) or 2 (Alarm)\n" +
                                    "volume: has to be equal to or higher than zero, has to be inputed as 0,0 or as 0, and cannot be higher than " + float.MaxValue + "\n" +
                                    "group number: has to be an integer ranging from 0 to " + int.MaxValue + ", has to exist"
                                    );
                                continue;
                            }

                        case "exit": Environment.Exit(0); break;
                        default: Console.WriteLine("Command does not exist. Type help for help."); continue;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Command is not complete. Type help for help.");
                }

            } while (true);

            //Creates a group.
            //Calls root.CreateGroup()
            bool CreateGroup()
            {
                root.CreateGroup();
                return true;
            }

            //Removes a group.
            //Asks user to input a group number.
            //If input is of a correct type, calls root.RemoveGroup()
            //If any check fails or any error occures, explains to the user why
            bool RemoveGroup() 
            {
                Console.Write("Select the group to be removed: ");
                try
                {
                    int groupIndex = int.Parse(Console.ReadLine());
                    if (groupIndex < 0) 
                    {
                        Console.WriteLine("Group number has to range from 0 to {0}", Root.ListOfGroups.Count - 1);
                        return false;
                    }
                    root.RemoveGroup(groupIndex);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Group number has to be an integer.");
                    return false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Group number has to range from 0 to {0}", Root.ListOfGroups.Count - 1);
                    return false;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Group number has to range from 0 to {0}", int.MaxValue);
                    return false;
                }
                return true;
            }

            //Creates a device
            //If called with arguments, simply passes them to root.CreateDevice().
            //If called without arguments other than deviceType, asks user for arguments, checks if arguments are of a correct type, calls root.CreateDevice()
            //If any check fails or any error occures, explains to the user why
            bool CreateDevice(int deviceType, dynamic groupIndex = null, dynamic deviceId = null, string deviceName = null, List<String> additionalProperties = null) 
            {
                if (groupIndex == null) {
                    additionalProperties = new List<String>();

                    Console.Write("Select the group for a device to be created in: ");
                    try { groupIndex = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Group index has to be an integer ranging from 0 to {0}.", Root.ListOfGroups.Count - 1); return false; }
                    Console.Write("Device ID: ");
                    try { deviceId = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); return false; }
                    Console.Write("Device name: ");
                    deviceName = Console.ReadLine();
                    switch (deviceType)
                    {
                        case 1:
                            {
                                Console.Write("Card reader access card number: ");
                                additionalProperties.Add(Console.ReadLine());
                            }
                            break;
                        case 3:
                            {
                                Console.Write("Led panel message: ");
                                additionalProperties.Add(Console.ReadLine());
                            }
                            break;
                        case 4:
                            {
                                Console.Write("Speaker sound: ");
                                additionalProperties.Add(Console.ReadLine());
                                Console.Write("Speaker volume: ");
                                additionalProperties.Add(Console.ReadLine());
                            }
                            break;
                    }
                }
                try { root.CreateDevice(groupIndex, deviceType, deviceId, deviceName, additionalProperties); }
                catch (DeviceIdIsNotUniqueException) { Console.WriteLine("Device ID has to be unique."); return false; }
                catch (ArgumentOutOfRangeException) { Console.WriteLine("Group number has to range from 0 to {0}", Root.ListOfGroups.Count - 1); return false; }
                catch (AccessCardNumberContainsInvalidCharactersException) { Console.WriteLine("Access card number has to be made up of digits 0-9 and letters a-f."); return false; }
                catch (AccessCardNumberInvalidLengthException) { Console.WriteLine("Access card number length has to be in the range of 0 to 16 and even."); return false; }
                catch (AccessCardNumberNotEvenLengthException) { Console.WriteLine("Access card number length has to be in the range of 0 to 16 and even."); return false; }
                catch (SoundIncorrectValueException) { Console.WriteLine("Values for sound are 0 [None], 1 [Music] or 2 [Alarm]."); return false; }
                catch (VolumeNegativeValueException) { Console.WriteLine("Volume cannot be negative."); return false; }
                catch (FormatException) { Console.WriteLine("An input is empty. Type \"help\" for help."); return false; }
                catch (OverflowException) { Console.WriteLine("A number value is too high or too low. Type \"help\" for help."); return false; }
                return true;
            }

            //Removes a device
            //If called without arguments, asks user to input deviceID, checks if deviceID is of a correct type, calls root.RemoveDevice()
            //If called with arguments, passes them to root.RemoveDevice()
            //If any check fails or any error occures, explains to the user why
            bool RemoveDevice(dynamic deviceId = null) 
            {
                if (deviceId == null) 
                {
                    Console.Write("ID of the device to be removed: ");
                    try { deviceId = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); return false; }
                }
                try { root.RemoveDevice(deviceId); }
                catch (DeviceDoesNotExistException) { Console.WriteLine("Device has to exist."); return false; }
                return true;
            }

            //Changed a property of a device
            //If called without any arguments other than changeType, asks user to input deviceId and a property, calls root.Change()
            //If called with arguments, passes them to root.Change()
            //If any check fails or any error occures, explains to the user why
            bool Change(int changeType, dynamic deviceId = null, dynamic property = null) 
            {
                if (deviceId == null) 
                {
                    Console.Write("ID of the device to be changed: ");
                    try { deviceId = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); return false; }
                    switch (changeType) 
                    {
                        case 0: 
                            {
                                Console.Write("New device ID: ");
                            }
                            break;
                        case 1:
                            {
                                Console.Write("New device name: ");
                            }
                            break;
                        case 2:
                            {
                                Console.Write("New card reader access card number: ");
                            }
                            break;
                        case 3:
                            {
                                Console.Write("New led panel message: ");
                            }
                            break;
                        case 4:
                            {
                                Console.Write("New speaker sound: ");
                            }
                            break;
                        case 5:
                            {
                                Console.Write("New speaker volume: ");
                            }
                            break;
                    }
                    property = Console.ReadLine();
                }
                try { root.Change(changeType, deviceId, property); }
                catch (DeviceIdIsNotUniqueException) { Console.WriteLine("Device ID has to be unique."); return false; }
                catch (AccessCardNumberContainsInvalidCharactersException) { Console.WriteLine("Access card number has to be made up of digits 0-9 and letters a-f."); return false; }
                catch (AccessCardNumberInvalidLengthException) { Console.WriteLine("Access card number length has to be in the range of 0 to 16 and even."); return false; }
                catch (AccessCardNumberNotEvenLengthException) { Console.WriteLine("Access card number length has to be in the range of 0 to 16 and even."); return false; }
                catch (SoundIncorrectValueException) { Console.WriteLine("Values for sound are 0 [None], 1 [Music] or 2 [Alarm]."); return false; }
                catch (VolumeNegativeValueException) { Console.WriteLine("Volume cannot be negative."); return false; }
                catch (FormatException) { Console.WriteLine("An input is empty or incorrect. Type \"help\" for help."); return false; }
                catch (OverflowException) { Console.WriteLine("A number value is too high or too low. Type \"help\" for help."); return false; }
                catch (DeviceDoesNotExistException) { Console.WriteLine("Device has to exist."); return false; }
                catch (InvalidCastException) { Console.WriteLine("Device is of an incorrect type."); return false; }
                return true;
            }

            //Moves a device into a group
            //If called without arguments, asks user to input deviceId and groupIndex, checks if they're of a correct type, calls root.Move()
            //If called with arguments, passes them to root.Move()
            //If any check fails or any error occures, explains to the user why
            bool Move(dynamic deviceId = null, dynamic groupIndex = null) 
            {
                if (deviceId == null) 
                {
                    Console.Write("ID of the device to move: ");
                    try { deviceId = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); return false; }
                    Console.Write("Number of the group to move the device into: ");
                    try { groupIndex = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Group index has to be an integer ranging from 0 to {0}.", Root.ListOfGroups.Count - 1); return false; }
                }
                try { root.Move(deviceId, groupIndex); }
                catch (DeviceDoesNotExistException) { Console.WriteLine("Device has to exist."); return false; }
                catch (ArgumentOutOfRangeException) { Console.WriteLine("Group number has to range from 0 to {0}", Root.ListOfGroups.Count - 1); return false; }
                return true;
            }

            //Prints the status of a device
            //Asks user to input deviceId, checks if it's of a correct type, calls root.PrintStatus(), prints result of root.PrintStatus()
            //If any check fails or any error occures, explains to the user why
            void PrintStatus() 
            {
                Console.Write("ID of the device to print the status of: ");
                try { Console.WriteLine(root.PrintStatus(int.Parse(Console.ReadLine()))); }
                catch (DeviceDoesNotExistException) { Console.WriteLine("Device has to exist."); }
                catch (FormatException) { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); }
            }

            //Changes the state of a door depending on changeType
            //If called without deviceId, asks the user for deviceId, checks if it's of a correct type, calls root.ChangeDoorStatus()
            //If called with deviceId, calls root.ChangeStatus()
            //If any check fails or any error occures, explains to the user why
            bool ChangeDoorStatus(int changeType, dynamic deviceId = null) 
            {
                if (deviceId == null) 
                {
                    Console.Write("ID of the door to change the status of: ");
                    try { deviceId = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Device ID has to be an integer ranging from {0} to {1}.", int.MinValue, int.MaxValue); return false; }
                }
                try { root.ChangeDoorStatus(changeType, deviceId); }
                catch (DeviceDoesNotExistException) { Console.WriteLine("Device has to exist."); return false; }
                catch (InvalidCastException) { Console.WriteLine("Device has to be a door."); return false; }
                return true;
            }
        }
    }
}
