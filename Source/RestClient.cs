using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SmartLocationApp.Source
{
  public class RestClient
  {
    public string EndPoint { get; set; }

    public HttpVerb Method { get; set; }

    public string ContentType { get; set; }

    public string PostData { get; set; }

    public RestClient()
    {
      this.EndPoint = "";
      this.Method = HttpVerb.GET;
      this.ContentType = "text/xml";
      this.PostData = "";
    }

    public RestClient(string endpoint)
    {
      this.EndPoint = endpoint;
      this.Method = HttpVerb.GET;
      this.ContentType = "text/xml";
      this.PostData = "";
    }

    public RestClient(string endpoint, HttpVerb method)
    {
      this.EndPoint = endpoint;
      this.Method = method;
      this.ContentType = "text/xml";
      this.PostData = "";
    }

    public RestClient(string endpoint, HttpVerb method, string postData)
    {
      this.EndPoint = endpoint;
      this.Method = method;
      this.ContentType = "text/xml";
      this.PostData = postData;
    }

    public string MakeRequest() => this.MakeRequest("");

    public string MakeRequest(string parameters)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.EndPoint + parameters);
      httpWebRequest.Method = this.Method.ToString();
      httpWebRequest.ContentLength = 0L;
      httpWebRequest.ContentType = this.ContentType;
      if (!string.IsNullOrEmpty(this.PostData) && this.Method == HttpVerb.POST)
      {
        UTF8Encoding utF8Encoding = new UTF8Encoding();
        byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(this.PostData);
        httpWebRequest.ContentLength = (long) bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
          requestStream.Write(bytes, 0, bytes.Length);
      }
      try
      {
        using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
        {
          string str = string.Empty;
          if (response.StatusCode != HttpStatusCode.OK)
            throw new ApplicationException(string.Format("Request failed. Received HTTP {0}", (object) response.StatusCode));
          using (Stream responseStream = response.GetResponseStream())
          {
            if (responseStream != null)
            {
              using (StreamReader streamReader = new StreamReader(responseStream))
                str = streamReader.ReadToEnd();
            }
          }
          return str;
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Could not connect to internet.");
        Application.Exit();
        return string.Empty;
      }
    }
  }
}
