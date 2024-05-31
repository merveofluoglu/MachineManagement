using Microsoft.EntityFrameworkCore;
using Models.dto;
using Models.entities;
using Services.Context;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MessagesService : IMessagesService
    {
        private MachineManagementDbContext _context;

        public MessagesService(MachineManagementDbContext _db)
        {
            _context = _db;
        }

        public List<Messages> GetAll()
        {
            return _context.Messages.ToList();
        }

        public Messages GetMessageById(long id)
        {
            return _context.Messages.FirstOrDefault(x => x.Id == id);
        }

        public List<Messages> GetMessagesByClientId(long id)
        {
            return _context.Messages.Where(x => x.Client_Id == id).ToList();
        }

        public async Task CreateMessageAsync(Messages message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            MessageReceived(message.Id);
        }

        public async Task DeleteMessageAsync(Messages message)
        {
            _context.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessagesByClientIdAsync(long id)
        {
            var messages = _context.Messages.Where(x => x.Client_Id == id).ToList();
            foreach (var message in messages)
            {
                await DeleteMessageAsync(message);
            }
        }

        public bool IsRead(long id)
        {
            return _context.Messages.FirstOrDefault(x =>x.Id == id).IsRead;
        }

        public async Task<bool> MessageIsReadAsync(long id)
        {
            var message = _context.Messages.FirstOrDefault(x => x.Id == id);
            if (message != null)
            {
                message.IsRead = true;
                _context.Entry(message).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool MessageReceived(long id)
        {
            var message = _context.Messages.FirstOrDefault(x => x.Id == id);
            if (message != null)
            {
                message.IsReceived = true;
                _context.Entry(message).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Messages GetLastMessage()
        {
            return _context.Messages.OrderByDescending(x => x.Id).First();
        }


    }
}
