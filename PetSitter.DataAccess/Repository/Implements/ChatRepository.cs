using Microsoft.EntityFrameworkCore;
using PetSitter.DataAccess.Repository.Interfaces;
using PetSitter.Models;
using PetSitter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetSitter.DataAccess.Repository.Implements
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync(Guid petOwnerId, Guid shopId)
        {
            // Kiểm tra xem cuộc trò chuyện đã tồn tại chưa để tránh tạo trùng lặp
            var existingConversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.PetOwnerId == petOwnerId && c.ShopId == shopId);

            if (existingConversation != null)
            {
                return existingConversation;
            }

            // Nếu chưa có, tạo mới
            var newConversation = new Conversation
            {
                PetOwnerId = petOwnerId,
                ShopId = shopId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Conversations.AddAsync(newConversation);
            await _context.SaveChangesAsync();

            return newConversation;
        }

        public async Task<Message> CreateMessageAsync(Guid conversationId, Guid senderId, string content)
        {
            var message = new Message
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Content = content
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<Conversation>> GetConversationsByUserIdAsync(Guid userId)
        {
            // Tìm các conversation IDs mà người dùng có liên quan
            var conversationIds = await _context.Conversations
                .Where(c => c.PetOwnerId == userId || (c.Shop != null && c.Shop.UserId == userId))
                .Select(c => c.ConversationId)
                .ToListAsync();

            if (!conversationIds.Any())
            {
                return new List<Conversation>(); 
            }

            // Dựa trên các IDs đó, truy vấn lại để lấy đầy đủ thông tin
            return await _context.Conversations
                .Where(c => conversationIds.Contains(c.ConversationId))
                .Include(c => c.PetOwner)
                .Include(c => c.Shop)
                .ThenInclude(s => s.User)
                .Include(c => c.Messages) //To get messages
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt) // Sắp xếp tin nhắn theo thời gian
                .ToListAsync();
        }

        public async Task<Conversation> GetConversationByIdAsync(Guid conversationId)
        {
            return await _context.Conversations
                .Include(c => c.PetOwner)
                .Include(c => c.Shop)
                .ThenInclude(s => s.User)
                .Include(c => c.Messages) //To get messages
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }
    }
}