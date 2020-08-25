using System;

namespace Reception
{
    class Events
    {
        //Whenever a device is updated, prints the status of the device into the console
        public void OnDeviceUpdated(object source, EventArgs e)
        {
            AbsDevice absDevice = (AbsDevice)source;
            Console.WriteLine(absDevice.GetCurrentState());
        }

        //Whenever the tree is called to be printed, prints the list of groups with all their device ids and names
        public void OnPrintTree(object source, EventArgs e) 
        {
            for (int i = 0; i < Root.ListOfGroups.Count; i++)
            {
                Console.Write("Group {0}: ", i);
                foreach (AbsDevice device in Root.ListOfGroups[i].ListOfDevices) 
                {
                    Console.Write("[{0}, {1}]", device.Id, device.Name);
                }
                Console.Write("\n");
            }
        }
    }
}
