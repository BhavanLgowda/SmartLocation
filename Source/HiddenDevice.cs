
namespace SmartLocationApp.Source
{
  public class HiddenDevice
  {
    public bool isSingleDevice { get; set; }

    public char deviceLabel { get; set; }

    public string hexValue { get; set; }

    public int hiddenDeviceCount { get; set; }

    public HiddenDevice(
      bool _isSingleDevice,
      char _deviceLabel,
      string _hexValue,
      int _hiddenDeviceCount)
    {
      this.isSingleDevice = _isSingleDevice;
      this.deviceLabel = _deviceLabel;
      this.hexValue = _hexValue;
      this.hiddenDeviceCount = _hiddenDeviceCount;
    }
  }
}
