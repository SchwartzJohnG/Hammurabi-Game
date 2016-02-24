using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hammurabi.Domain.Models {
    public class GameState {

        [Key]
        public string Id { get; set; }

        public int Year { get; set; }
        public int Bushels { get; set; }
        public int Peasants { get; set; }
        public int Acres { get; set; }
        public int LandPrice { get; set; }
        public int Deaths { get; set; }
        public int Migrants { get; set; }
        public int Harvest { get; set; }
    }
}