using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Token
    {

        public int Id { get; set; }
        public string TokenValue { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
 
    }
}
