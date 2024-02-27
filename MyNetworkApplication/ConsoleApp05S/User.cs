using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp05S
{
    internal class User
    {
        public virtual List<Message>? MessagesTo { get; set; } = new List<Message>();
        public virtual List<Message>? MessagesFrom { get; set; } = new List<Message>();
        public int Id { get; set; }
        public string? FullName { get; set; }

    }
}
