using System;
using System.Text;

namespace Reception
{
    class Alarm : AbsDevice
    {
        const string Type = "Alarm";

        public Alarm(int Id, string DeviceName) 
        {
            SetDeviceType(Type);
            SetId(Id);
            SetName(DeviceName);
            Console.WriteLine(GetCurrentState());
        }

        public override string GetCurrentState()
        {
            StringBuilder returnable = new StringBuilder().AppendFormat("{0} {1} with id {2}", GetDeviceType(), GetName(), GetId().ToString());
            return returnable.ToString();
        }

    }
}
