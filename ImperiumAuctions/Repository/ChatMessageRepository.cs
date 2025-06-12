using ImperiumAuctions.Data;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Models;
using Microsoft.EntityFrameworkCore;

namespace ImperiumAuctions.Repository
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public ChatMessageRepository(ApplicationDbContext db):base(db)
        {
            _context = db;
        }

        public IOrderedQueryable<ChatMessage> GetAllCurrentProductMessages(int? productId)
        {
            var chatMessages = GetAll()
                .Where(p => p.ProductID == productId)
                .OrderBy(t => t.Timestamp);
            return chatMessages;
        }

        public void Update(ChatMessage chatMessage)
        {
            _context.ChatMessages.Update(chatMessage);
        }

    }
}
