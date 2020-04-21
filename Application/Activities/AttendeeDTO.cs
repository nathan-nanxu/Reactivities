using System.Collections.Generic;

namespace Application.Activities
{
  public class AttendeeDTO
  {
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Image { get; set; }
    public bool isHost { get; set; }
    public bool Following { get; set; }
  }
}