using System;

namespace MnMScoreCounter.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#512BD4"; // Default color or hex code
    }
}
