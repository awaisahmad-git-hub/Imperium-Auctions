namespace ImperiumAuctions.Repository.IRepository
{
    public interface IMainRepository
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IBidRepository BidRepository { get; }
        ICountDownTargetRepository CountDownTargetRepository { get; }
        IAuctionOrderRepository AuctionOrderRepository { get; }
        IChatMessageRepository ChatMessageRepository { get; }
        void Save();
        Task SaveA();

    }
}
