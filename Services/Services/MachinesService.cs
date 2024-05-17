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

        public async void Add(Machines machine)
        {
            await _context.Machines.AddAsync(machine);
            await _context.SaveChangesAsync();
        }

        public async void Update(Machines machine)
        {
            _context.Entry(machine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async void Delete(long id)
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
            return _context.Machines.OrderBy(x => x.Id).Last();
        }
    }
}
