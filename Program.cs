using System;
using System.Threading;

namespace Reception
{
    class Program
    {
        static void Main(string[] args)
        {

            Root root = new Root();

            Console.WriteLine("Creating a group:");
            root.CreateGroup();

            Console.WriteLine("\nCreating a group:");
            root.CreateGroup();

            Console.WriteLine("\nCreating a group:");
            root.CreateGroup();

            Console.WriteLine("\nAdding devices into previously created groups:");
            root.CreateDevice(0, 0, 0, "Alarm0");
            root.CreateDevice(0, 1, 1, "CardReader0", "AB12");
            root.CreateDevice(1, 0, 5, "Alarm1");
            root.CreateDevice(2, 0, 10, "Alarm2");
            root.CreateDevice(2, 1, 11, "CardReader2", "FF15");
            root.CreateDevice(2, 2, 12, "Door2", 0);
            root.CreateDevice(2, 3, 13, "LedPanel2", "An average message.");
            root.CreateDevice(2, 4, 14, "Speaker2", (Enums.Sound)2, 1.0f);

            Console.WriteLine("\nAdding a device into a previously created group.");
            root.CreateDevice(1, 0, 6, "newAlarm");
            Thread.Sleep(1500);

            Console.WriteLine("\nRemoving the just created alarm.");
            root.RemoveDevice(1, 1);
            Thread.Sleep(1500);

            Console.WriteLine("\nRenaming Alarm0 to DangerAlerter");
            root.ChangeAnyName(0,0,"DangerAlerter");
            Thread.Sleep(1500);

            Console.WriteLine("\nMoving LedPanel2 to group1");
            root.MoveDevice(2,3,1);
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging message on LedPanel2 to \"Changed message\"");
            root.ChangeLedPanelMessage(1, 1, "Changed message");
            Thread.Sleep(1500);

            Console.WriteLine("\nLocking Door2");
            root.LockDoor(2, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nUnlocking Door2");
            root.UnlockDoor(2, 2);
            Thread.Sleep(1500);

            Console.WriteLine("\nChanging Speaker2's sound to None.");
            root.ChangeSpeakerSound(2, 3, (Enums.Sound)0);
            Thread.Sleep(1500);

            string nextCommand;
            Console.WriteLine("Type help for the list of commands.");
            do
            {
                Console.Write("Next command: ");
                nextCommand = Console.ReadLine();
                switch (nextCommand)
                {
                    case "creategroup": root.CreateGroup(); continue;
                    case "createdevice": root.CreateDevice(); continue;
                    case "removegroup": root.RemoveGroup(); continue;
                    case "removedevice": root.RemoveDevice(); continue;
                    case "movedevice": root.MoveDevice(); continue;
                    case "renamedevice": root.ChangeAnyName(); continue;
                    case "changedeviceid": root.ChangeAnyId(); continue;
                    case "changemessage": root.ChangeLedPanelMessage(); continue;
                    case "changeaccesslistnumber": root.ChangeCardReaderAccessListNumber(); continue;
                    case "changevolume": root.ChangeSpeakerVolume(); continue;
                    case "changesound": root.ChangeSpeakerSound(); continue;
                    case "opendoor": root.OpenDoor(); continue;
                    case "closedoor": root.CloseDoor(); continue;
                    case "lockdoor": root.LockDoor(); continue;
                    case "unlockdoor": root.UnlockDoor(); continue;
                    case "forceopendoor": root.OpenDoorForcibly(); continue;
                    case "opendoorfortoolong": root.DoorOpenedForTooLong(); continue;
                    case "printtree": root.UpdateTree(); continue;
                    case "devicestatus": root.UpdateStatus(); continue;

                }
            } while (nextCommand != "exit");
        }
    }
}
