using System;
using System.Collections.Generic;

namespace MnMScoreCounter.Models
{
    public class Round
    {
        public int RoundNumber { get; set; }
        // Key is Player.Id, Value is the score they got in this specific round
        public Dictionary<Guid, int> Scores { get; set; } = new();
    }
}
