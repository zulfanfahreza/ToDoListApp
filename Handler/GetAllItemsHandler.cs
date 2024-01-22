using MediatR;
using ToDoListApp.Models;
using ToDoListApp.Services;

namespace ToDoListApp.Handler
{
    public class GetAllItemsHandler
    {
        public class Query : IRequest<List<ToDoItemModel>> { }

        public class Handler : IRequestHandler<Query, List<ToDoItemModel>> 
        {
            private readonly IToDoService _toDoService;
            public Handler(IToDoService toDoService)
            {
                _toDoService = toDoService;
            }

            public async Task<List<ToDoItemModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _toDoService.GetAllItems();
            }
        }
    }
}
