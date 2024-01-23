using MediatR;
using ToDoListApp.Models;
using ToDoListApp.Services;

namespace ToDoListApp.Handler
{
    public class GetItemByIdHandler
    {
        public class Query : IRequest<ToDoItemModel> 
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ToDoItemModel>
        {
            private readonly IToDoService _toDoService;
            public Handler(IToDoService toDoService)
            {
                _toDoService = toDoService;
            }

            public async Task<ToDoItemModel> Handle(Query request, CancellationToken cancellationToken) 
            {
                return await _toDoService.GetById(request.Id);
            }
        }
    }
}
