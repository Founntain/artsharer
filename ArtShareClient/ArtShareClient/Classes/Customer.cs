using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtShareClient.Classes
{
    public sealed class Customer
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public DateTime CreationTime { get; set; }
        public string FurAffinity { get; set; }
        public string Twitter { get; set; }
        public string Discord { get; set; }
    }
}
