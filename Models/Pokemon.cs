using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex7.Models
{
    public class Pokemon
    {
        public Base Base { get; set; }
        public int Id { get; set; }
        public Name Name { get; set; }
        public string[] Type { get; set; }
    }
}
