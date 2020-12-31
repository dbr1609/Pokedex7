using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex7.Models
{
    public class Base
    {
        [Range(1, 255)]
        public int Attack { get; set; }
        [Range(1, 255)]
        public int Defense { get; set; }
        [Range(1, 255)]
        public int Hp { get; set; }
        [Range(1, 255)]
        public int SpAttack { get; set; }
        [Range(1, 255)]
        public int SpDefense { get; set; }
        [Range(1, 255)]
        public int Speed { get; set; }
    }
}
