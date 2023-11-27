
using System.Text.RegularExpressions;

namespace SmartLocationApp.Models
{
  internal class VideoFile
  {
    public string Name { get; set; }

    public string Ext { get; set; }

    public string Ticket { get; set; }

    public string FullPath { get; set; }

    public VideoFile.Type VideoType { get; set; }

    public static VideoFile Parse(string file, VideoFile.Type videoType)
    {
      VideoFile videoFile = (VideoFile) null;
      MatchCollection matchCollection = Regex.Matches(file, "\\\\(([a-z0-9]+)(_[0-9]+)*\\.(mov|mpeg4|avi|wmv|mpegps|flv|3gpp|webm|mp4))$", RegexOptions.IgnoreCase);
      if (matchCollection.Count == 1)
      {
        GroupCollection groups = matchCollection[0].Groups;
        videoFile = new VideoFile()
        {
          Name = groups[1].Value,
          Ext = groups[4].Value,
          Ticket = groups[2].Value,
          FullPath = file,
          VideoType = videoType
        };
      }
      return videoFile;
    }

    public enum Type
    {
      Normal,
      Zoomselfie,
      Podcam,
    }
  }
}
