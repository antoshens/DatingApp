using System.Collections.Concurrent;

namespace DatingApp.WebAPI.SignalR
{
    public class PresenceTracker
    {
        private static readonly IDictionary<string, List<string>> OnlineUsers = new ConcurrentDictionary<string, List<string>>();

        public Task UserConnected(string userName, string connectionId)
        {
            if (OnlineUsers.ContainsKey(userName))
            {
                OnlineUsers[userName].Add(connectionId);
            }
            else
            {
                OnlineUsers.Add(userName, new List<string> { connectionId });
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnected(string userName, string connectionId)
        {
            if (!OnlineUsers.ContainsKey(userName)) return Task.CompletedTask;
            
                OnlineUsers[userName].Remove(connectionId);
            
            if (OnlineUsers[userName].Count == 0)
            {
                OnlineUsers.Remove(userName);
            }

            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            onlineUsers = OnlineUsers
                .OrderBy(k => k.Key)
                .Select(k => k.Key)
                .ToArray();

            return Task.FromResult(onlineUsers);
        }

        public Task<IEnumerable<string>> GetConnectionsForUser(string username)
        {
            if (OnlineUsers.TryGetValue(username, out var connectionIds))
            {
                return Task.FromResult(connectionIds.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<string>());
        }
    }
}
