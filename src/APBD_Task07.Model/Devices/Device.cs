namespace APBD_Task07.Model.Devices;

public abstract class Device
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public bool TurnedOn { get; set; }

    public virtual void TurnOn()
    {
        TurnedOn = true;
    }

    public virtual void TurnOff()
    {
        TurnedOn = false;
    }

    public virtual void Update(Device device)
    {
        if (device.GetType() != GetType())
            throw new InvalidCastException();
    }

    public virtual string ToFileRepresentation()
    {
        return $"{Id}, {Name}, {TurnedOn}";
    }
}