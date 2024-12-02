using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;

public record CompleteTodoItemCommand : IRequest
{
    public int Id { get; init; }
}

public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public CompleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id);

        if (todoItem == null)
        {
            throw new NotFoundException(nameof(todoItem), request.Id.ToString());
        }

        todoItem.Done = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
