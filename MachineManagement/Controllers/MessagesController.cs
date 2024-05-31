using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.dto;
using Services.IServices;
using Services.Services;

namespace MachineManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMachinesService _MachinesService;
        private readonly IMessagesService _MessagesService;
        private readonly DtoConverter _DtoConverter;

        public MessagesController(
            IMessagesService _messagesService,
            IMachinesService _machinesService,
            DtoConverter _dtoConverter)
        {
            _MachinesService = _machinesService;
            _MessagesService = _messagesService;
            _DtoConverter = _dtoConverter;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var messagesDto = new List<MessagesDTO>();
            try
            {
                var messages = _MessagesService.GetAll();
                if(messages.IsNullOrEmpty())
                {
                    return NotFound();
                }
                foreach (var message in messages)
                {
                    messagesDto.Add(_DtoConverter.EntityToDto(message));
                }
                return Ok(messagesDto);
            }
            catch (Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpGet("MessageId/{id}")]
        public IActionResult GetMessageById(int id)
        {
            try
            {
                var message = _MessagesService.GetMessageById(id);
                if(message == null)
                {
                    return NotFound();
                }
                return Ok(_DtoConverter.EntityToDto(message));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpGet("ClientId/{id}")]
        public IActionResult GetMessagesByClientId(long id)
        {
            var messagesDto = new List<MessagesDTO>();
            try
            {
                var messages = _MessagesService.GetMessagesByClientId(id);
                if (messages.IsNullOrEmpty())
                {
                    return NoContent();
                }
                foreach (var message in messages)
                {
                    messagesDto.Add(_DtoConverter.EntityToDto(message));
                }
                return Ok(messagesDto);
            }
            catch(Exception _ex)
            {
                throw new Exception (_ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody]MessagesDTO messageDto)
        {
            try
            {
                if(messageDto == null)
                {
                    return BadRequest();
                }
                await _MessagesService.CreateMessageAsync(_DtoConverter.DtoToEntity(messageDto));
                await _MachinesService.IncrementMessageCountAsync(messageDto.Client_Id);
                return Ok(_DtoConverter.EntityToDto(_MessagesService.GetLastMessage()));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpDelete("MessageId/{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            try
            {
                var message = _MessagesService.GetMessageById(id);
                if(message == null)
                {
                    return NotFound();
                }
                await _MessagesService.DeleteMessageAsync(message);
                await _MachinesService.DecrementMessageCountAsync(message.Client_Id);
                return NoContent();
            }
            catch (Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ReadMessage(long id)
        {
            try
            {
                var message = _MessagesService.GetMessageById(id);
                if(message == null)
                {
                    return NotFound(id);
                }
                await _MessagesService.MessageIsReadAsync(id);
                return Ok(_DtoConverter.EntityToDto(_MessagesService.GetMessageById(id)));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }
    }
}
