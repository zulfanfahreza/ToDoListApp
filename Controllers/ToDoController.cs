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

namespace ToDoListApp.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ActionResult<ItemCollectionResponseModel>> GetAllItems()
        {
            try
            {
                var items = _toDoService.GetAllItems();
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
                var toDoItems = _toDoService.GetById(id);
                if (toDoItems == null)
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
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(ToDoItemModel toDoItem)
        {
            try
            {
                _toDoService.AddItem(toDoItem);

                return CreatedAtAction(nameof(GetAllItems), new { id = toDoItem.Id }, toDoItem);
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
        public async Task<ActionResult<UpdateItemResponseModel>> PutToDoItem(int id,  UpdateItemRequestModel request)
        {
            try
            {
                var toDoItem = _toDoService.UpdateItem(id, request);
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
                var response = new UpdateItemResponseModel
                {
                    Item = toDoItem,
                    UpdatedAt = DateTime.Now
                };
                return response;
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
                var toDoItem = _toDoService.GetById(id);
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

                _toDoService.DeleteItem(toDoItem);
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
