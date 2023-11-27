using Microsoft.Win32;
using System;
using System.Timers;

namespace SmartLocationApp.Source
{
  internal static class MidnightNotifier
  {
    private static readonly Timer timer = new Timer(MidnightNotifier.GetSleepTime());

    static MidnightNotifier()
    {
      MidnightNotifier.timer.Elapsed += (ElapsedEventHandler) ((s, e) =>
      {
        MidnightNotifier.OnDayChanged();
        MidnightNotifier.timer.Interval = MidnightNotifier.GetSleepTime();
      });
      MidnightNotifier.timer.Start();
      SystemEvents.TimeChanged += new EventHandler(MidnightNotifier.OnSystemTimeChanged);
    }

    private static double GetSleepTime() => (DateTime.Today.AddDays(1.0) - DateTime.Now).TotalMilliseconds;

    private static void OnDayChanged()
    {
      EventHandler<EventArgs> dayChanged = MidnightNotifier.DayChanged;
      if (dayChanged == null)
        return;
      dayChanged((object) null, (EventArgs) null);
    }

    private static void OnSystemTimeChanged(object sender, EventArgs e) => MidnightNotifier.timer.Interval = MidnightNotifier.GetSleepTime();

    public static event EventHandler<EventArgs> DayChanged;
  }
}
