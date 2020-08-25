namespace Reception
{
    class Alarm : AbsDevice
    {

        //constructor, creates this.Alarm
        public Alarm(int id, string name)
        {
            Id = id;
            Type = (Types)0;
            var UpdateEvent = new Events();
            DeviceUpdated += UpdateEvent.OnDeviceUpdated;
            Name = name;  //I want only one OnDeviceUpdated triggered on device creation.
        }

        //Returns a string that has all device attributes in it.
        public override string GetCurrentState()
        {
            return Type.ToString() + " " + Name + " with id " + Id + ".";
        }
    }
}