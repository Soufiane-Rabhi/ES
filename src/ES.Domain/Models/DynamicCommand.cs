using System;

namespace ES.Domain.Models
{
    public class DynamicCommand
    {
        public Guid ObjectId { get; set; }
        public Permission[] Permissions { get; set; }
    }
}
