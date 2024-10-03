using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
