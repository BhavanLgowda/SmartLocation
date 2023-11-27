using CameraControl.Devices;
using CameraControl.Devices.Classes;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;

namespace SmartLocationApp.Pages.Classes
{
  internal class CameraLiveView
  {
    private System.Timers.Timer _liveViewTimer = new System.Timers.Timer();

    public ICameraDevice CameraDevice { get; set; }

    public event EventHandler<Bitmap> OnLiveImageReceived;

    public event EventHandler<string> OnError;

    public CameraLiveView(ICameraDevice cameraDevice)
    {
      this._liveViewTimer.Interval = 66.0;
      this._liveViewTimer.Stop();
      this._liveViewTimer.Elapsed += new ElapsedEventHandler(this._liveViewTimer_Tick);
      this.CameraDevice = cameraDevice;
      this.CameraDevice.CameraDisconnected += new CameraDisconnectedEventHandler(this.CameraDevice_CameraDisconnected);
    }

    private void CameraDevice_CameraDisconnected(object sender, DisconnectCameraEventArgs eventArgs)
    {
      this.CameraDevice.CameraDisconnected -= new CameraDisconnectedEventHandler(this.CameraDevice_CameraDisconnected);
      this.Stop();
    }

    public void Start() => new Thread(new ThreadStart(this.StartLiveView)).Start();

    public void Stop() => new Thread(new ThreadStart(this.StopLiveView)).Start();

    public void CameraDisconnected() => this.Stop();

    private void _liveViewTimer_Tick(object sender, EventArgs e)
    {
      LiveViewData liveViewImage;
      try
      {
        liveViewImage = this.CameraDevice.GetLiveViewImage();
      }
      catch (Exception ex)
      {
        return;
      }
      if (liveViewImage == null)
        return;
      if (liveViewImage.ImageData == null)
        return;
      try
      {
        if (this.OnLiveImageReceived == null)
          return;
        this.OnLiveImageReceived((object) this, new Bitmap((Stream) new MemoryStream(liveViewImage.ImageData, liveViewImage.ImageDataPosition, liveViewImage.ImageData.Length - liveViewImage.ImageDataPosition)));
      }
      catch (Exception ex)
      {
      }
    }

    private void StartLiveView()
    {
      bool flag;
      do
      {
        flag = false;
        try
        {
          this.CameraDevice.StartLiveView();
        }
        catch (DeviceException ex)
        {
          if (ex.ErrorCode == 8217U || ex.ErrorCode == 2147942570U)
          {
            Thread.Sleep(100);
            flag = true;
          }
          else if (this.OnError != null)
            this.OnError((object) this, "CameraLiveView.StartLiveView.DeviceException = Error occurred :" + ex.Message);
        }
        catch (Exception ex)
        {
          Console.WriteLine("CameraLiveView.StartLiveView.Exception = Error occurred :" + ex.Message);
        }
      }
      while (flag);
      this._liveViewTimer.Start();
    }

    private void StopLiveView()
    {
      bool flag;
      do
      {
        flag = false;
        try
        {
          this._liveViewTimer.Stop();
          Thread.Sleep(500);
          this.CameraDevice.StopLiveView();
        }
        catch (DeviceException ex)
        {
          if (ex.ErrorCode == 8217U || ex.ErrorCode == 2147942570U)
          {
            Thread.Sleep(100);
            flag = true;
          }
          else if (this.OnError != null)
            this.OnError((object) this, "CameraLiveView.StopLiveView.DeviceException = Error occurred :" + ex.Message);
        }
        catch (Exception ex)
        {
          Console.WriteLine("CameraLiveView.StopLiveView.Exception = Error occurred :" + ex.Message);
        }
      }
      while (flag);
    }
  }
}
