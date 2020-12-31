using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pokedex7.Models;
using Pokedex7.ViewModels;

namespace Pokedex7.Controllers
{
    public enum SortField
    {
        Id, Name, Type
    }

    public enum SortDirection
    {
        ASC, DESC
    }
    public class PokedexController : Controller
    {
        private IPokedex pokedex;
        private IConfiguration configuration;
        private ILogger<PokedexController> logger;
        public PokedexController(IConfiguration configuration, ILogger<PokedexController> logger, IPokedex pokedex)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.pokedex = pokedex;
        }
        
        public IActionResult Index([FromQuery] SortField sort = SortField.Id, [FromQuery] SortDirection sortDirection = SortDirection.ASC)
        {
            logger.LogInformation("A user has used the pokedex");
            IEnumerable<Pokemon> pokemons = pokedex.GetAll();
            switch (sort)
            {
                case SortField.Id:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Id) : pokemons.OrderByDescending(p => p.Id);
                    break;
                case SortField.Name:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Name.English) : pokemons.OrderByDescending(p => p.Name.English);
                    break;
                case SortField.Type:
                    pokemons = (sortDirection == SortDirection.ASC) ? pokemons.OrderBy(p => p.Type[0]) : pokemons.OrderByDescending(p => p.Type[0]);
                    break;
            }
            PokedexListViewModel pokedexListViewModel = new PokedexListViewModel
            {
                PageGenerated = pokedex.GeneratedAt.ToString(),
                Pokemons = pokemons,
                SortField = sort,
                SortDirection = sortDirection
            };

            return View(pokedexListViewModel);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Pokemon pokemon = pokedex.Get(id);
            if (pokemon != null)
            {
                return View(pokemon);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Details(int id, Pokemon pokemon)
        {
            Pokemon p = pokedex.Get(id);
            
            if (ModelState.IsValid)
            {
                p.Base = pokemon.Base;
                TempData["Message"] = $"{p.Name.English} was updated!";
                this.pokedex.Save(p);
            }
            return View(p);
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Pokemon p = pokedex.Get(Id);
            if (p != null)
            {
                pokedex.Delete(p);
                TempData["Message"] = $"{p.Name.English} was deleted succesfully!";
                return RedirectToAction("Index", "Pokedex");
            }
            return NotFound();
        }
        private List<int> GetPoketeamFromSession()
        {
            string poketeamString = HttpContext.Session.GetString("Poketeam");
            List<int> poketeam = new List<int>();
            if (poketeamString != null)
            {
                poketeam = JsonConvert.DeserializeObject<List<int>>(poketeamString);
            }
            return poketeam;
        }
        private void SavePoketeamToSession(List<int> poketeam)
        {
            HttpContext.Session.SetString("Poketeam", JsonConvert.SerializeObject(poketeam));
        }

        [HttpPost]
        public IActionResult AddToTeam(int id)
        {
            List<int> poketeam = GetPoketeamFromSession().TakeLast(5).ToList();
            poketeam.Add(id);
            SavePoketeamToSession(poketeam);
            return RedirectToAction(nameof(Team));
        }
        [HttpGet]
        public IActionResult Team()
        {
            List<int> poketeam = GetPoketeamFromSession();
            List<Pokemon> pokemon = poketeam.Select(id => pokedex.Get(id)).ToList();
            return View(pokemon);
        }
    }
}
