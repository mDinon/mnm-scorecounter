using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MnMScoreCounter.Models;

namespace MnMScoreCounter.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetPlayersAsync();
        Task SavePlayersAsync(List<Player> players);
        Task AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(Guid id);
    }

    public class PlayerService : IPlayerService
    {
        private readonly string _filePath;

        public PlayerService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "players.json");
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    // Return a default set of players if none exist to make it easier for the user
                    var defaults = new List<Player>
                    {
                        new Player { Id = Guid.NewGuid(), Name = "Marko", Color = "#FF1744" },
                        new Player { Id = Guid.NewGuid(), Name = "Matea", Color = "#2979FF" },
                        new Player { Id = Guid.NewGuid(), Name = "Nikola", Color = "#00E676" },
                        new Player { Id = Guid.NewGuid(), Name = "Ana", Color = "#FFEA00" }
                    };
                    await SavePlayersAsync(defaults);
                    return defaults;
                }

                using var stream = File.OpenRead(_filePath);
                var players = await JsonSerializer.DeserializeAsync<List<Player>>(stream);
                return players ?? new List<Player>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading players: {ex.Message}");
                return new List<Player>();
            }
        }

        public async Task SavePlayersAsync(List<Player> players)
        {
            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using var stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, players, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving players: {ex.Message}");
            }
        }

        public async Task AddPlayerAsync(Player player)
        {
            var players = await GetPlayersAsync();
            players.Add(player);
            await SavePlayersAsync(players);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            var players = await GetPlayersAsync();
            var index = players.FindIndex(p => p.Id == player.Id);
            if (index != -1)
            {
                players[index] = player;
                await SavePlayersAsync(players);
            }
        }

        public async Task DeletePlayerAsync(Guid id)
        {
            var players = await GetPlayersAsync();
            var index = players.FindIndex(p => p.Id == id);
            if (index != -1)
            {
                players.RemoveAt(index);
                await SavePlayersAsync(players);
            }
        }
    }
}
