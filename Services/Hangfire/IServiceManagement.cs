namespace OnlineShopAPIFull.Services.Hangfire
{
    public interface IServiceManagement
    {
        Task RefreshCacheAsync();
    }
}
