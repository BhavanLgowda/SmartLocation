using System;

namespace SmartLocationApp.Source
{
  public class UsbInfo
  {
    public string usbID { get; set; }

    public DateTime date { get; set; }

    public string Password { get; set; }

    public string DriveLetter { get; set; }

    public UsbInfo()
    {
      this.usbID = (string) null;
      this.Password = (string) null;
    }

    public UsbInfo(string _usbID, DateTime _date, string _Password, string _DriveLetter)
    {
      this.usbID = _usbID;
      this.date = _date;
      this.Password = _Password;
      this.DriveLetter = _DriveLetter;
    }
  }
}
