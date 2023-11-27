
namespace SmartLocationApp.Models
{
  public class PodcamError
  {
    public int Id { get; set; }

    public string LocationId { get; set; }

    public string Ticket { get; set; }

    public string ArchivePath { get; set; }

    public PodcamError.Type ErrorType { get; set; }

    public string ErrorMessage { get; set; }

    public string Date { get; set; }

    public enum Type
    {
      Archive,
      Stpreport,
      CloudStorage,
    }
  }
}
