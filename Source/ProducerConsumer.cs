using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SmartLocationApp.Source
{
  internal class ProducerConsumer
  {
    private readonly object _lock = new object();
    private readonly Queue<fileSourceDest> queue = new Queue<fileSourceDest>();

    public void Produce(fileSourceDest input)
    {
      lock (this._lock)
      {
        this.queue.Enqueue(input);
        Monitor.Pulse(this._lock);
      }
    }

    public fileSourceDest Consume()
    {
      lock (this._lock)
      {
        while (!this.queue.Any<fileSourceDest>())
          Monitor.Wait(this._lock);
        fileSourceDest fileSourceDest = this.queue.Dequeue();
        try
        {
          File.Move(fileSourceDest.sourceFileName, fileSourceDest.destFileName);
        }
        catch (Exception ex)
        {
        }
        return fileSourceDest;
      }
    }
  }
}
