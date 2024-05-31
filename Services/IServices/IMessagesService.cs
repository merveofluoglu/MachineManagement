using Models.dto;
using Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IMessagesService
    {
        List<Messages> GetAll();
        Messages GetMessageById(long id);
        List<Messages> GetMessagesByClientId(long id);
        Task CreateMessageAsync(Messages message);
        Task DeleteMessageAsync(Messages message);
        Task DeleteMessagesByClientIdAsync(long id);
        bool MessageReceived(long id);
        Task<bool> MessageIsReadAsync(long id);
        bool IsRead(long id);
        Messages GetLastMessage();
    }
}
