using Models.dto;
using Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DtoConverter
    {
        public MachinesDTO EntityToDto(Machines entity) =>
            new MachinesDTO
            {
                Id = entity.Id,
                MachineName = entity.MachineName,
                Status = entity.Status,
                Description = entity.Description,
                MessageCount = entity.MessageCount,
            };

        public Machines DtoToEntity(MachinesDTO dto) =>
            new Machines
            {
                Id = dto.Id,
                MachineName = dto.MachineName,
                Status = dto.Status,
                Description = dto.Description,
                MessageCount = dto.MessageCount,
            };

        public MessagesDTO EntityToDto(Messages entity) =>
            new MessagesDTO
            {
                Id = entity.Id,
                Client_Id = entity.Client_Id,
                IsReceived = entity.IsReceived,
                IsRead = entity.IsRead,
                Message = entity.Message,
                StatusCode = entity.StatusCode,
                ErrorCode = entity.ErrorCode,
                ErrorMessage = entity.ErrorMessage,
                ErrorType = entity.ErrorType,
                Topic = entity.Topic,
                Date = entity.Date,
            };

        public Messages DtoToEntity(MessagesDTO dto) =>
            new Messages
            {
                Id = dto.Id,
                Client_Id = dto.Client_Id,
                IsReceived = dto.IsReceived,
                IsRead = dto.IsRead,
                Message = dto.Message,
                StatusCode = dto.StatusCode,
                ErrorCode = dto.ErrorCode,
                ErrorMessage = dto.ErrorMessage,
                ErrorType = dto.ErrorType,
                Topic = dto.Topic,
                Date = dto.Date,
            };
    }
}
