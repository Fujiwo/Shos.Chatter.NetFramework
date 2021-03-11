using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shos.Chatter.Wpf.Models
{
    using Shos.Chatter.Models;
    using System.Diagnostics;

    public class ChatterModel : BindableBase
    {
        static HttpClient httpClient = new HttpClient();

        public Server Server { get; set; } = new Server();
        
        int userId = 1;

        public int UserId {
            get => userId;
            set => SetProperty(ref userId, value);
        }

        IEnumerable<UserBase>? users = null;
        IEnumerable<ChatBase>? chats = null;

        public IEnumerable<UserBase> Users {
            get {
                if (users is null)
                    users = GetUsers().Result;
                return users;
            }
        }

        public IEnumerable<ChatBase> Chats {
            get {
                if (chats is null)
                    chats = GetChats().Result;
                return chats;
            }
        }

        public async Task Start()
        {
            Server.UpdateUsers += async () => await UpdateUsers(false);
            Server.UpdateChats += async () => await UpdateChats(false);
            await Server.Start();
        }

        public async Task<bool> Add(UserBase user)
        {
            try {
                var uri = new Uri($"{Server.Url}api/Users");
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "Name", user.Name }
                    }
                );
                request.Content = content;
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateUsers(true);
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> Delete(UserBase user)
        {
            try {
                var uri = new Uri($"{Server.Url}api/Users/{user.Id}");
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateUsers(true);
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> Update(UserBase user)
        {
            try {
                var uri = new Uri($"{Server.Url}api/Users/{user.Id}");
                var request = new HttpRequestMessage(HttpMethod.Put, uri);

                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "Id", user.Id.ToString() },
                        { "Name", user.Name }
                    }
                );
                request.Content = content;

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateUsers(true);
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> Add(ChatBase chat)
        {
            try {
                var uri     = new Uri($"{Server.Url}api/Chats");
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "Content", chat.Content      },
                        { "UserId" , UserId.ToString() }
                    }
                );
                request.Content = content;
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateChats(true);
                    chat.UserId = UserId;
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> Delete(ChatBase chat)
        {
            try {
                var uri = new Uri($"{Server.Url}api/Chats/{chat.Id}");
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateChats(true);
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        public async Task<bool> Update(ChatBase chat)
        {
            try {
                var uri     = new Uri($"{Server.Url}api/Chats/{chat.Id}");
                var request = new HttpRequestMessage(HttpMethod.Put, uri);
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
                        { "Id"     , chat.Id.ToString()     },
                        { "Content", chat.Content           },
                        { "UserId" , chat.UserId.ToString() }
                    }
                );
                request.Content = content;

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    await UpdateChats(true);
                    return true;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return false;
        }

        async Task<IEnumerable<UserBase>> GetUsers()
        {
            try {
                var uri = new Uri($"{Server.Url}api/Users");
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("ContentType", "application/json");
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<IEnumerable<UserBase>>();
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return new UserBase[] { };
        }

        async Task<IEnumerable<ChatBase>> GetChats()
        {
            try {
                var uri = new Uri($"{Server.Url}api/Chats");
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("ContentType", "application/json");
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<IEnumerable<ChatBase>>();
            } catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            return new ChatBase[] { };
        }

        async Task UpdateUsers(bool notifyEnabled = false)
        {
            users = GetUsers().Result;
            RaisePropertyChanged(nameof(Users));
            if (notifyEnabled)
                await Server.NotifyUpdateUsers();
        }

        async Task UpdateChats(bool notifyEnabled = false)
        {
            chats = GetChats().Result;
            RaisePropertyChanged(nameof(Chats));
            if (notifyEnabled)
                await Server.NotifyUpdateChats();
        }
    }

    public class Server : BindableBase
    {
        public event Action? UpdateUsers;
        public event Action? UpdateChats;

        const string serverUrlKey = "ServerUrl";
        public string Url => System.Configuration.ConfigurationManager.AppSettings[serverUrlKey];

        HubConnection? hubConnection = null;
        IHubProxy?     hubProxy      = null;

        public async Task NotifyUpdateUsers() => await hubProxy?.Invoke("UpdateUsers");
        public async Task NotifyUpdateChats() => await hubProxy?.Invoke("UpdateChats");

        public async Task Start()
        {
            hubConnection = new HubConnection($"{Url}signalr");
            hubProxy      = hubConnection.CreateHubProxy("chatter");

            hubProxy.On(nameof(UpdateUsers), () => UpdateUsers?.Invoke());
            hubProxy.On(nameof(UpdateChats), () => UpdateChats?.Invoke());
            try {
                await hubConnection.Start();
                Debug.WriteLine("hubConnection.State: {hubConnection.State}");
            } catch (Exception ex) {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
