using ImperiumAuctions.Models;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        void Update(ChatMessage chatMessage);
        IOrderedQueryable<ChatMessage> GetAllCurrentProductMessages(int? productId);
    }
}
