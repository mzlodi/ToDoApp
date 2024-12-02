using Microsoft.AspNetCore.Mvc;
using System.Text;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Application.TodoItems.Commands.DeleteTodoItem;
using ToDoApp.Application.TodoItems.Commands.CompleteTodoItem;
using ToDoApp.Application.Common.Exceptions;

namespace ToDoApp.Web.Controllers;

public class ToDoItemController : Controller
{
    private readonly ISender _sender;

    public ToDoItemController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            var errorBuilder = new StringBuilder();
            foreach (var error in ex.Errors.SelectMany(err => err.Value))
            {
                errorBuilder.AppendLine(error);
            }
            TempData["Errors"] = errorBuilder.ToString();
        }

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int[] id, CancellationToken cancellationToken)
    {
        if(id == null || id.Length == 0)
        {
            TempData["Errors"] = "No Todo items selected";
            return Redirect("/");
        }

        try
        {
            foreach (var i in id)
            {
                await _sender.Send(new DeleteTodoItemCommand { Id = i }, cancellationToken);
            }
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join(Environment.NewLine, ex.Errors.SelectMany(err => err.Value));
        }

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Complete(int[] id, CancellationToken cancellationToken)
    {
        if (id == null || id.Length == 0)
        {
            TempData["Errors"] = "No Todo items selected";
            return Redirect("/");
        }

        try
        {
            foreach (var i in id)
            {
                await _sender.Send(new CompleteTodoItemCommand { Id = i }, cancellationToken);
            }
        }
        catch (ValidationException ex)
        {
            TempData["Errors"] = string.Join(Environment.NewLine, ex.Errors.SelectMany(err => err.Value));
        }

        return Redirect("/");
    }
}
