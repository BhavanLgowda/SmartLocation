
using SmartLocationApp.Source;
using System;

namespace SmartLocationApp.Models
{
  internal class NotifyData
  {
    public string Application { get; set; }

    public string Version { get; set; }

    public string Environment { get; set; }

    public string Subject { get; set; }

    public string Message { get; set; }

    public string Parameters { get; set; }

    public string Date { get; set; }

    public NotifyData()
    {
      this.Application = "Smart Location";
      this.Version = Function.GetVersionNumber();
      this.Environment = string.Format("\r\n                UserName: {0}, \r\n                UserDomainName: {1}, \r\n                MachineName: {2}, \r\n                OSVersion: {3}", (object) System.Environment.UserName, (object) System.Environment.UserDomainName, (object) System.Environment.MachineName, (object) System.Environment.OSVersion);
      this.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
  }
}
