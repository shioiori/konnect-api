namespace Konnect.ChatHub.Requests
{
  public class AddMessageRequest
  {
    public string Text { get; set; }
    public string IsImage { get; set; }
    public string? ImageUrl { get; set; }
    public string ChatId { get; set; }
    public string CreatedBy { get; set; }
  }
}
