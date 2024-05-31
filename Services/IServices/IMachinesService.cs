using Models.dto;
using Models.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IMachinesService
    {
        List<Machines> GetAll();
        Machines GetMachineById(long id);
        Machines GetMachineByName(string name);
        Task<Machines> AddAsync(Machines dto);
        Task UpdateAsync(Machines dto);
        Task DeleteAsync(Machines id);
        long GetMessageCount(long id);
        Machines GetLastMachine();
        Task IncrementMessageCountAsync(long id);
        Task DecrementMessageCountAsync(long id);
    }
}
