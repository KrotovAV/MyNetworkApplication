using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp05S
{
    internal class Message
    {
        public int MessageId { get; set; }
        public string? Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool IsSent { get; set; }
        public int? UserToId { get; set; }
        public int? UserFromId { get; set; }
        public virtual User? UserTo {get; set;}
        public virtual User? UserFrom { get; set; }

    }
}
