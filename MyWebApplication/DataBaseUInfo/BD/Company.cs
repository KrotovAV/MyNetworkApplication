using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseUInfo.BD
{
    internal class Company
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string Position { get; set; }
        public int AdressId { get; set; }

    }
}
