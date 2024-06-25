using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel11.Models
{
    public class PriceRange
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public PriceRange(decimal min, decimal max)
        {
            MinPrice = min;
            MaxPrice = max;
        }
    }
}