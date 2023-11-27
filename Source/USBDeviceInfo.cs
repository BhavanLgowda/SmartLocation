
using System.Management;

namespace SmartLocationApp.Source
{
  public class USBDeviceInfo
  {
    public USBDeviceInfo(ManagementObject device)
    {
      this.Name = device.GetPropertyValue(nameof (Name)).ToString();
      this.Caption = device.GetPropertyValue(nameof (Caption)).ToString();
      this.PNPDeviceID = device.GetPropertyValue(nameof (PNPDeviceID)).ToString();
      this.ConfigManagerErrorCode = device.GetPropertyValue(nameof (ConfigManagerErrorCode)) == null ? ErrorCode.Device_is_not_present_not_working_properly_or_does_not_have_all_of_its_drivers_installed : (ErrorCode) device.GetPropertyValue(nameof (ConfigManagerErrorCode));
      this.DeviceID = device.GetPropertyValue(nameof (DeviceID)).ToString();
      this.SystemName = device.GetPropertyValue(nameof (SystemName)).ToString();
      this.Status = device.GetPropertyValue(nameof (Status)).ToString();
    }

    public string Name { get; private set; }

    public string PNPDeviceID { get; set; }

    public string Caption { get; private set; }

    public ErrorCode ConfigManagerErrorCode { get; private set; }

    public string DeviceID { get; private set; }

    public string SystemName { get; private set; }

    public string Status { get; private set; }
  }
}
