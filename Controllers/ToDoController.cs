using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using ToDoListApp.Models;
using ToDoListApp.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using ToDoListApp.Services;
using MediatR;
using ToDoListApp.Handler;

namespace ToDoListApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ToDo")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly IMediator _mediator;

        public ToDoController(IToDoService toDoService,
            IMediator mediator)
        {
            _toDoService = toDoService;
            _mediator = mediator;
        }

        [HttpGet("version")]
        public IActionResult VersionTrial()
        {
            return new OkObjectResult("ToDo from v1 controller");
        }

        [HttpGet]
        public async Task<ActionResult<ItemCollectionResponseModel>> GetAllItems()
        {
            try
            {
                var items = await _mediator.Send(new GetAllItemsHandler.Query());
                if (items.Count() == 0)
                {
                    var errorResponse = new ErrorResponseModel
                    {
                        Error = $"{nameof(GetItemById)} Exception",
                        Message = $"Item cannot be found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                    return NotFound(errorResponse);
                }

                var response = new ItemCollectionResponseModel
                {
                    Items = items,
                    Pagination = new Pagination
                    {
                        Total = items.Count,
                        Page = 1,
                        Sort = 1,
                        SortBy = "Id",
                        Limit = items.Count
                    }
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(GetAllItems)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };
                
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemModel>> GetItemById(int id)
        {
            try
            {
                var toDoItems = await _mediator.Send(new GetItemByIdHandler.Query
                {
                    Id = id
                });
                if (toDoItems is null)
                {
                    var errorResponse = new ErrorResponseModel
                    {
                        Error = $"{nameof(GetItemById)} Exception",
                        Message = $"Item with id {id} cannot be found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                    return NotFound(errorResponse);
                }

                return toDoItems;
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(GetItemById)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(AddUpdateItemRequestModel request)
        {
            try
            {
                var toDoItem = await _mediator.Send(new AddItemHandler.Query 
                {
                    Parameter = request
                });

                return CreatedAtAction(nameof(PostToDoItem), toDoItem);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(PostToDoItem)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoItemModel>> PutToDoItem(int id,  AddUpdateItemRequestModel request)
        {
            try
            {
                var toDoItem = await _mediator.Send(new UpdateItemHandler.Query
                {
                    Id = id,
                    Parameter = request
                });

                if (toDoItem == null)
                {
                    var errorResponse = new ErrorResponseModel
                    {
                        Error = $"{nameof(PutToDoItem)} Exception",
                        Message = $"Item with id {id} cannot be found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                    return NotFound(errorResponse);
                }

                return toDoItem;
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(PutToDoItem)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                return BadRequest(errorResponse);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var toDoItem = await _mediator.Send(new GetItemByIdHandler.Query
                {
                    Id= id
                });

                if (toDoItem == null)
                {
                    var errorResponse = new ErrorResponseModel
                    {
                        Error = $"{nameof(DeleteItem)} Exception",
                        Message = $"Item with id {id} cannot be found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                    return NotFound(errorResponse);
                }

                await _mediator.Send(new DeleteItemHandler.Query
                {
                    Parameter = toDoItem
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(DeleteItem)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                return BadRequest(errorResponse);
            }
            
        }
    }
}
