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
        void CreateMessage(Messages message);
        void DeleteMessage(long id);
        bool MessageReceived(long id);
        bool MessageIsRead(long id);
        bool IsRead(long id);
        Messages GetLastMessage();
    }
}
