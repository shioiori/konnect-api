using Konnect.ChatHub.Helper;
using Konnect.ChatHub.Models;
using Konnect.ChatHub.Repositories;
using Konnect.ChatHub.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Konnect.ChatHub.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IUnitOfWork _unitOfWork;

		public ChatHub(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(string chatId, string obj)
		{
			var message = JsonConvert.DeserializeObject<AddMessageRequest>(obj);
			var chat = await _unitOfWork.Chats.GetAsync(chatId);
			var mess = new Message()
			{
				Text = message.Text,
				IsImage = message.IsImage,
				ImageUrl = message.ImageUrl,
				CreatedDate = DateTime.Now,
				CreatedBy = await _unitOfWork.Users.GetAsync(message.CreatedBy),
				IsDeleted = false
			};
			_ = _unitOfWork.Messages.CreateAsync(mess);
			if (chat.Messages == null)
			{
				chat.Messages = new List<Message>();
			}
			chat.Messages.Add(mess);
			_ = _unitOfWork.Chats.UpdateAsync(chatId, chat);
			await Clients.Group(chatId).SendAsync(ChatAction.ReceiveMessage,
			  JsonHelper.SerializeObjectCamelCase(mess));
		}
		public async Task AddToChat(string chatId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
			await Clients.Group(chatId).SendAsync(ChatAction.AddToChat, "online");
		}

		public async Task RemoveFromChat(string chatId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
			await Clients.Group(chatId).SendAsync(ChatAction.RemoveFromChat);
		}
	}

	public class ChatAction
	{
		public const string AddToChat = "AddToChat";
		public const string RemoveFromChat = "RemoveFromChat";
		public const string ReceiveMessage = "ReceiveMessage";
	}
}
