using System;
using System.Threading;

namespace SmartLocationApp.Pages.Classes
{
  public class BWorker
  {
    public BWorker.StartMethod StartMethodFunc;
    private Thread thread;
    private string resultCode = "";
    private string resultMessage = "";
    private object startThreadParameter;
    private Thread endControlThread;

    public event BWorker.ExitEventHandler Exit;

    public bool IsAlive => this.thread != null && this.thread.IsAlive;

    public ThreadState State => this.thread.ThreadState;

    public string ResultCode
    {
      get => this.resultCode;
      set => this.resultCode = value;
    }

    public string ResultMessage
    {
      get => this.resultMessage;
      set => this.resultMessage = value;
    }

    public object ResultObject { get; set; }

    public void Start(object parameter)
    {
      this.thread = new Thread(new ParameterizedThreadStart(this.startThread));
      this.thread.IsBackground = true;
      this.thread.Start(parameter);
      this.endControlThread = new Thread(new ParameterizedThreadStart(this.endControl));
      this.endControlThread.IsBackground = true;
      this.endControlThread.Start((object) this.thread);
    }

    public void Stop()
    {
      if (this.endControlThread != null && this.endControlThread.IsAlive)
        this.endControlThread.Abort();
      if (this.thread == null || !this.thread.IsAlive)
        return;
      this.thread.Abort();
    }

    private void startThread(object parameter)
    {
      try
      {
        this.startThreadParameter = parameter;
        this.StartMethodFunc(parameter, this);
      }
      catch (Exception ex)
      {
      }
    }

    private void endControl(object parameter)
    {
      this.thread = (Thread) parameter;
      while (this.thread.IsAlive)
        Thread.Sleep(500);
      if (this.Exit == null)
        return;
      this.Exit(this.startThreadParameter, this);
    }

    public delegate void StartMethod(object parameter, BWorker worker);

    public delegate void ExitEventHandler(object parameter, BWorker worker);
  }
}
