﻿using ImperiumAuctions.Data;
using ImperiumAuctions.Repository.IRepository;

namespace ImperiumAuctions.Repository
{
    public class MainRepository : IMainRepository
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository {  get; private set; }

        public IProductRepository ProductRepository { get; private set; }
        public IProductImageRepository ProductImageRepository { get; private set; }
        public IBidRepository BidRepository { get; private set; }
        public ICountDownTargetRepository CountDownTargetRepository { get; private set; }
        public IAuctionOrderRepository AuctionOrderRepository { get; private set; }

        public IChatMessageRepository ChatMessageRepository { get; private set; }

        public MainRepository(ApplicationDbContext db)
        {
            _context = db;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ProductImageRepository = new ProductImageRepository(_context);
            BidRepository = new BidRepository(_context);
            CountDownTargetRepository= new CountDownTargetRepository(_context);
            AuctionOrderRepository = new AuctionOrderRepository(_context);
            ChatMessageRepository = new ChatMessageRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveA()
        {
            await _context.SaveChangesAsync();
        }
    }
}
