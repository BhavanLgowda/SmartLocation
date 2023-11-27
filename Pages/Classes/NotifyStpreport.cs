using SmartLocationApp.Models;
using SmartLocationApp.Router;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SmartLocationApp.Pages.Classes
{
  internal class NotifyStpreport
  {
    public void send(NotifyData notify)
    {
      FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new KeyValuePair<string, string>[4]
      {
        new KeyValuePair<string, string>("Subject", notify.Subject),
        new KeyValuePair<string, string>("Message", notify.Message),
        new KeyValuePair<string, string>("Parameters", notify.Parameters),
        new KeyValuePair<string, string>("Environment", string.Format("\r\n                    Date: " + notify.Date + ", \r\n                    Application: " + notify.Application + ", \r\n                    Version: " + notify.Version + ", \r\n                    " + notify.Environment + "\r\n                "))
      });
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri(Animation.Url);
        HttpResponseMessage result = httpClient.PostAsync("app/notify", (HttpContent) content).Result;
      }
    }
  }
}
