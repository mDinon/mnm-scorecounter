using System;
using System.Collections.Generic;

namespace MnMScoreCounter.Models
{
    public class GameSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int MaxScoreLimit { get; set; } = 500;
        public List<Player> Participants { get; set; } = new();
        public List<Round> Rounds { get; set; } = new();
        public bool IsEnded { get; set; }
    }
}
