using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.dto
{
    public class MachinesDTO
    {
        public long Id { get; set; }
        public required string MachineName { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public int MessageCount { get; set; }
    }
}
