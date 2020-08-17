using System;
using System.Threading;

namespace Rec
{
    class Program
    {
        static void Main(string[] args)
        {
            Root root = new Root();

            Console.WriteLine("Creating a new group.");
            root.CreateGroup();
            Thread.Sleep(1500);
            
            Console.WriteLine("\nCreating a new group.");
            root.CreateGroup();
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating alarm.");
            root.CreateDevice(0, 0, 0, "Alarm0");
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating ledpanel.");
            root.CreateDevice(3, 0, 1, "LedPanel0", "I AM A LED PANEL CONTAINING A MESSAGE. I WILL BE REWRITTEN SOON.");
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating door.");
            root.CreateDevice(2, 0, 2, "Door0");
            Thread.Sleep(1500);

            Console.WriteLine("\nCreating speaker.");
            root.CreateDevice(4, 0, 3, "Speaker0", (Enums.Sound)0, 100.1f);
            Thread.Sleep(1500);

            Console.WriteLine("\nRemoving Alarm0.");
            root.RemoveDevice(0);
            Thread.Sleep(1500);

            Console.WriteLine("\nRenaming LedPanel0 to aVeryImportantLedPanel0");
            root.Change(1, 1, "aVeryImportandLedPanel0");
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging the message of aVeryImportantLedPanel0");
            root.Change(2, 1, "NEW MESSAGE!");
            Thread.Sleep(1500);

            Console.WriteLine("\nMoving aVeryImportantLedPanel0 to group 1");
            root.Move(1, 1);
            Thread.Sleep(1500);

            Console.WriteLine("\nOpening Door0.");
            root.ChangeDoorStatus(2, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nClosing Door0.");
            root.ChangeDoorStatus(3, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging the sound of Speaker0 to Music");
            root.Change(3, 3, (Enums.Sound)1);
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
                                    case "group": root.CreateGroup(); continue;
                                    case "alarm": root.CreateDevice(0); continue;
                                    case "cardreader": root.CreateDevice(1); continue;
                                    case "door": root.CreateDevice(2); continue;
                                    case "ledpanel": root.CreateDevice(3); continue;
                                    case "speaker": root.CreateDevice(4); continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "remove":
                            {
                                switch (nextCommand[1])
                                {
                                    case "group": root.RemoveGroup(); continue;
                                    case "device": root.RemoveDevice(); continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "change":
                            {
                                switch (nextCommand[1])
                                {
                                    case "id": root.Change(0); continue;
                                    case "name": root.Change(1); continue;
                                    case "message": root.Change(2); continue;
                                    case "sound": root.Change(3); continue;
                                    case "volume": root.Change(4); continue;
                                    case "accesscardnumber": root.Change(5); continue;
                                    default: Console.WriteLine("Subcommand does not exist. Type help for help."); continue;
                                }
                            }
                        case "move": root.Move(); continue;
                        case "printtree": root.PrintTree(); continue;
                        case "printstatus": root.PrintStatus(); continue;
                        case "lock": root.ChangeDoorStatus(0); continue;
                        case "unlock": root.ChangeDoorStatus(1); continue;
                        case "open": root.ChangeDoorStatus(2); continue;
                        case "close": root.ChangeDoorStatus(3); continue;
                        case "openfortoolong": root.ChangeDoorStatus(4); continue;
                        case "openforcibly": root.ChangeDoorStatus(5); continue;
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
                                    "openforcibly > opens a door using force\n" +
                                    "exit > exits application"
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
        }
    }
}
