using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;

namespace DevConnect.Persistence.Command;

public class MessageCommandRepository : IMessageCommandRepository
{
    
    private readonly DevConnectMongoContext _context;

    public MessageCommandRepository(DevConnectMongoContext context)
    {
        _context = context;
    }
    public async Task AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}