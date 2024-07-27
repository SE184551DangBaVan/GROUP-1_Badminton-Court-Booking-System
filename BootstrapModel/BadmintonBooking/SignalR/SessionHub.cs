using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonBooking.SignalR
{
    [Authorize]
    public class SessionHub : Hub
    {
        public async Task NotifyLogout(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveLogoutNotification");
        }
    }
}
