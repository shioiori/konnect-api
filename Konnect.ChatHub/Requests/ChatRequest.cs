﻿using Konnect.ChatHub.Data;

namespace Konnect.ChatHub.Requests
{
  public class CreateChatRequest
  {
    public GroupData GroupData { get; set; }
    public List<UserData> Users { get; set; }
    public string CreatedBy { get; set; }
  }

  public class AddUsersToChatParam
  {
    public string Id { get; set; }
    public List<UserData> Users { get; set; }
  }
}
