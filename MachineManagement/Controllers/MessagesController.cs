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
        private readonly IMessagesService _MessagesService;
        private readonly DtoConverter _DtoConverter;

        public MessagesController(
            IMessagesService _messagesService,
            DtoConverter _dtoConverter)
        {
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
        public IActionResult AddMessage([FromBody]MessagesDTO messageDto)
        {
            try
            {
                if(messageDto == null)
                {
                    return BadRequest();
                }
                _MessagesService.CreateMessage(_DtoConverter.DtoToEntity(messageDto));
                return Ok(_DtoConverter.EntityToDto(_MessagesService.GetLastMessage()));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteMessage(long messageId)
        {
            try
            {
                var message = _MessagesService.GetMessageById(messageId);
                if(message == null)
                {
                    return NotFound();
                }
                _MessagesService.DeleteMessage(messageId);
                return NoContent();
            }
            catch (Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpPost("{id}")]
        public IActionResult ReadMessage(long id)
        {
            try
            {
                var message = _MessagesService.GetMessageById(id);
                if(message == null)
                {
                    return NotFound(id);
                }
                _MessagesService.MessageIsRead(id);
                return Ok(_DtoConverter.EntityToDto(_MessagesService.GetMessageById(id)));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }
    }
}
