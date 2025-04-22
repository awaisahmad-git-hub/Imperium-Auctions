namespace PrestigeAuction.Repository.IRepository
{
    public interface IMainRepository
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IBidRepository BidRepository { get; }
        ICountDownTargetRepository CountDownTargetRepository { get; }
        void Save();
    }
}
