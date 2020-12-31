using Pokedex7.Controllers;
using Pokedex7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex7.ViewModels
{
    public class PokedexListViewModel
    {
        public string PageGenerated { get; set; }
        public IEnumerable<Pokemon> Pokemons { get; set; }
        public SortField SortField { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
