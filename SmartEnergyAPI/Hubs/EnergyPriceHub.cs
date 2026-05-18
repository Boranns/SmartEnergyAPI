using Microsoft.AspNetCore.SignalR;

namespace SmartEnergyAPI.Hubs
{
    public class EnergyPriceHub : Hub
    {
        public async Task JoinPriceUpdates()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PriceUpdates");
        }

        public async Task LeavePriceUpdates()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "PriceUpdates");
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PriceUpdates");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "PriceUpdates");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
