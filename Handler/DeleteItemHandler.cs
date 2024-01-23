using MediatR;
using ToDoListApp.Models;
using ToDoListApp.Services;

namespace ToDoListApp.Handler
{
    public class DeleteItemHandler
    {
        public class Query : IRequest
        {
            public ToDoItemModel Parameter { get; set; }
        }

        public class Handler : IRequestHandler<Query>
        {
            private readonly IToDoService _toDoService;

            public Handler(IToDoService toDoService)
            {
                _toDoService = toDoService;
            }

            public async Task Handle(Query request, CancellationToken cancellationToken)
            {
                await _toDoService.DeleteItem(request.Parameter);
            }
        }
    }
}
