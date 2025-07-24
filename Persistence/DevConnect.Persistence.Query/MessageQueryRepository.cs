using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using MongoDB.Driver.Linq;

namespace DevConnect.Persistence.Query;

public class MessageQueryRepository : IMessageQueryRepository
{
    private readonly DevConnectMongoContext _mongoContext;

    public MessageQueryRepository(DevConnectMongoContext context)
    {
        _mongoContext = context;
    }
    public async Task<IEnumerable<Message>> GetMessagesForUserAsync(Guid userId)
    {
        IQueryable<Message> query = _mongoContext.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId);

        var messages = await query
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return messages;
    }
}