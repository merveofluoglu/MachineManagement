using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class MachinesController : ControllerBase
    {
        private IMachinesService _MachinesService;
        private DtoConverter _DtoConverter;

        public MachinesController(
            IMachinesService _machinesService, 
            DtoConverter _dtoConverter)
        {
            _MachinesService = _machinesService;
            _DtoConverter = _dtoConverter;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _machinesDto = new List<MachinesDTO>();
            try
            {
                var machines = _MachinesService.GetAll();

                if(machines.IsNullOrEmpty())
                {
                    return NotFound();
                }

                foreach (var machine in machines)
                {
                    _machinesDto.Add(_DtoConverter.EntityToDto(machine));
                }
                return Ok(_machinesDto);
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpGet("MachineId/{id}")]
        public IActionResult GetMachineById(long id)
        {
            try
            {
                var machine = _MachinesService.GetMachineById(id);
                if(machine == null)
                {
                    return NotFound();
                }
                return Ok(_DtoConverter.EntityToDto(machine));
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpGet("MachineName/{machineName}")]
        public IActionResult GetMachineByName(string machineName)
        {
            try
            {
                var machine = _MachinesService.GetMachineByName(machineName);
                if(machine == null)
                {
                    return NotFound();
                }
                return Ok(_DtoConverter.EntityToDto(machine));
            }
            catch(Exception _ex) 
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddMachines([FromBody] MachinesDTO machineDto)
        {
            try
            {
                if(machineDto == null)
                {
                    return BadRequest();
                }
                _MachinesService.Add(_DtoConverter.DtoToEntity(machineDto));
                return Ok();
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMachine(long id, [FromBody] MachinesDTO machineDto)
        {
            try
            {
                if(machineDto == null || id != machineDto.Id)
                {
                    return BadRequest();
                }
                var machine = _MachinesService.GetMachineById(id);
                if(machine == null)
                {
                    return NotFound();
                }
                _MachinesService.Update(_DtoConverter.DtoToEntity(machineDto));
                return Ok();
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteMachine(long id)
        {
            try
            {
                var machine = _MachinesService.GetMachineById(id);
                if(machine == null)
                {
                    return NotFound();
                }
                _MachinesService.Delete(id);
                return NoContent();
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }

        [HttpGet("ClientId/{id:long}")]
        public IActionResult GetMessageCount(long id)
        {
            try
            {
                var machine = _MachinesService.GetMachineById(id);
                if(machine == null)
                {
                    return NotFound();
                }
                var messages = _MachinesService.GetMessageCount(id);
                if (messages.Equals(0))
                {
                    return NoContent();
                }
                return Ok(messages);
            }
            catch (Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
        }
    }
}
