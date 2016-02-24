using CoderCamps.Data.Repository;
using Hammurabi.Domain.Models;
using Hammurabi.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hammurabi.Presentation.Controllers {
    public class HammurabiController : ApiController {

        private IRepository _repo;

        public HammurabiController(IRepository repo) {
            _repo = repo;
        }

        public string Get() {
            Random rand = new Random();
            GameState game = new GameState {
                Id = Guid.NewGuid().ToString(),
                Peasants = 100,
                Bushels = 2800,
                Acres = 1000,
                LandPrice = rand.Next(17, 26),
                Year = 1,
                Deaths = 0,
                Migrants = 0,
                Harvest = 0
            };
            _repo.Add(game);
            _repo.SaveChanges();

            return game.Id;
        }

        public GameState Get(string id) {
            return _repo.Find<GameState>(id);
        }

        public GameState Post(string id, [FromBody]GameTurn turn) {

            Random rdm = new Random();

            var dbGame = _repo.Find<GameState>(id);

            // Calculate Peasant shift
            dbGame.Deaths = Math.Max(dbGame.Peasants - turn.Feed / 20, 0);
            dbGame.Migrants = rdm.Next(1, 10);
            dbGame.Peasants += dbGame.Migrants - dbGame.Deaths;

            // Calculate Acre shift
            dbGame.Acres += turn.AcreExchange;
            dbGame.Bushels -= turn.AcreExchange * dbGame.LandPrice;

            // Calculate remaining food shift
            dbGame.Harvest = rdm.Next(1, 6);
            dbGame.Bushels += turn.Plant * dbGame.Harvest;
            dbGame.Bushels -= turn.Feed;

            dbGame.LandPrice = rdm.Next(17, 26);

            dbGame.Year++;

            _repo.SaveChanges();

            return dbGame;
        }
    }
}
