using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Parsers;

public interface IParser
{
    public bool CanParse(string s);
    public Device Parse(string s);
}