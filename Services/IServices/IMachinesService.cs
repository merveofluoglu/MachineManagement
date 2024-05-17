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
        void Add(Machines dto);
        void Update(Machines dto);
        void Delete(long id);
        long GetMessageCount(long id);
        Machines GetLastMachine();
    }
}
