using Newtonsoft.Json;

namespace Application.ViewModels.Auth
{
  public class RecapchaResponseDTO
  {
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("challenge_ts")]
    public DateTimeOffset ChallengeTs { get; set; }
    [JsonProperty("hostname")]
    public string HostName { get; set; }
  }
}
