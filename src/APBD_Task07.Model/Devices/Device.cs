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

    public abstract string GenerateId();

    public static string RandomString()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[8];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(stringChars);
        return finalString;
    }
}