using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand : IRequest
{
    public int Id { get; init; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id);

        if (todoItem == null)
        {
            throw new NotFoundException(nameof(todoItem), request.Id.ToString());
        }

        _context.TodoItems.Remove(todoItem);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
