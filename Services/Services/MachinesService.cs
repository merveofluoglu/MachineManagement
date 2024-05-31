using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    public class MachinesService : IMachinesService
    {
        private MachineManagementDbContext _context;

        public MachinesService(MachineManagementDbContext _db)
        { 
            _context = _db;
        }

        public List<Machines> GetAll()
        {
            return _context.Machines.ToList();
        }

        public Machines GetMachineById(long id)
        {
            return _context.Machines.FirstOrDefault(x => x.Id == id);
        }

        public Machines GetMachineByName(string name)
        {
            return _context.Machines.FirstOrDefault(x => x.MachineName == name);
        }

        public async Task<Machines> AddAsync(Machines machine)
        {
            machine.Status = "idle";
            machine.MessageCount = 0;
            await _context.Machines.AddAsync(machine);
            await _context.SaveChangesAsync();
            return _context.Machines.OrderByDescending(x => x.Id).First();
        }

        public async Task UpdateAsync(Machines machine)
        {
            _context.Entry(machine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Machines id)
        {
            _context.Remove(id);
            await _context.SaveChangesAsync();
        }

        public long GetMessageCount(long id)
        {
            return _context.Machines.FirstOrDefault(x => x.Id == id).MessageCount;
        }

        public Machines GetLastMachine()
        {
            return _context.Machines.OrderByDescending(x => x.Id).First();
        }

        public async Task IncrementMessageCountAsync(long id)
        {
            var machine = _context.Machines.FirstOrDefault(x => x.Id == id);
            if(machine != null)
            {
                machine.MessageCount++;
                _context.Entry(machine).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecrementMessageCountAsync(long id)
        {
            var machine = _context.Machines.FirstOrDefault(x => x.Id == id);
            if (machine != null)
            {
                machine.MessageCount--;
                _context.Entry(machine).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
