using Globomantics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data.Repositories;

public class TodoTaskRepository : ToDoRepository<TodoTask>
{

    public TodoTaskRepository(GlobomanticsDbContext context) : base(context)
    {
    }

    public override async Task AddAsync(TodoTask task)
    {
        var todoTask = DomainToDataMapping.MapToDoFromDomain<Domain.TodoTask, Data.Models.TodoTask>(task);

        await Context.AddAsync(todoTask);
    }

    public override async Task<TodoTask> GetAsync(Guid id)
    {
        var data = await Context.TodoTasks.SingleAsync(t => t.Id == id);

        return DataToDomainMapping.MapToDoFromData<Data.Models.TodoTask, Domain.TodoTask>(data);
    }
}
