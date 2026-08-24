using System;
using System.Collections.Generic;
using System.Linq;
using MnMScoreCounter.Models;

namespace MnMScoreCounter.Services
{
    public interface IGameSessionService
    {
        GameSession? CurrentSession { get; set; }
        void StartNewGame(List<Player> participants, int maxScoreLimit);
        void AddRound();
        void UpdateRoundScore(int roundNumber, Guid playerId, int score);
        void DeleteRound(int roundNumber);
        void EndGame();
        int GetTotalScore(Guid playerId);
        List<int> GetCumulativeScores(Guid playerId);
        bool CheckGameOverCondition();
        Player? GetWinner();
        Player? GetLoser();
    }

    public class GameSessionService : IGameSessionService
    {
        public GameSession? CurrentSession { get; set; }

        public void StartNewGame(List<Player> participants, int maxScoreLimit)
        {
            CurrentSession = new GameSession
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.Now,
                MaxScoreLimit = maxScoreLimit,
                Participants = participants,
                Rounds = new List<Round>(),
                IsEnded = false
            };
            // Add a default first round to start with
            AddRound();
        }

        public void AddRound()
        {
            if (CurrentSession == null) return;

            int nextRoundNum = CurrentSession.Rounds.Count > 0 
                ? CurrentSession.Rounds.Max(r => r.RoundNumber) + 1 
                : 1;

            var newRound = new Round
            {
                RoundNumber = nextRoundNum,
                Scores = CurrentSession.Participants.ToDictionary(p => p.Id, p => (int?)null)
            };

            CurrentSession.Rounds.Add(newRound);
        }

        public void UpdateRoundScore(int roundNumber, Guid playerId, int score)
        {
            if (CurrentSession == null) return;

            var round = CurrentSession.Rounds.FirstOrDefault(r => r.RoundNumber == roundNumber);
            if (round != null)
            {
                round.Scores[playerId] = score;

                // Check game over dynamically if totals exceed limit
                if (CheckGameOverCondition())
                {
                    EndGame();
                }
            }
        }

        public void DeleteRound(int roundNumber)
        {
            if (CurrentSession == null) return;

            var round = CurrentSession.Rounds.FirstOrDefault(r => r.RoundNumber == roundNumber);
            if (round != null)
            {
                CurrentSession.Rounds.Remove(round);
                
                // Recalculate round numbers sequentially
                for (int i = 0; i < CurrentSession.Rounds.Count; i++)
                {
                    CurrentSession.Rounds[i].RoundNumber = i + 1;
                }
            }
        }

        public void EndGame()
        {
            if (CurrentSession != null)
            {
                CurrentSession.IsEnded = true;
            }
        }

        public int GetTotalScore(Guid playerId)
        {
            if (CurrentSession == null) return 0;

            return CurrentSession.Rounds.Sum(r => r.Scores.TryGetValue(playerId, out int? s) ? s.GetValueOrDefault() : 0);
        }

        public List<int> GetCumulativeScores(Guid playerId)
        {
            if (CurrentSession == null) return new List<int>();

            var cumulative = new List<int>();
            int sum = 0;
            foreach (var round in CurrentSession.Rounds)
            {
                sum += round.Scores.TryGetValue(playerId, out int? s) ? s.GetValueOrDefault() : 0;
                cumulative.Add(sum);
            }
            return cumulative;
        }

        public bool CheckGameOverCondition()
        {
            if (CurrentSession == null) return false;

            var currentRound = CurrentSession.Rounds.Last();

            if (currentRound.Scores.Count != CurrentSession.Participants.Count ||
                currentRound.Scores.Values.Any(score => score == null))
            {
                return false;
            }

            return CurrentSession.Participants.Any(p => GetTotalScore(p.Id) >= CurrentSession.MaxScoreLimit);
        }

        public Player? GetWinner()
        {
            if (CurrentSession == null || CurrentSession.Participants.Count == 0) return null;

            // Standard Uno: Player with the minimum score wins
            return CurrentSession.Participants
                .OrderBy(p => GetTotalScore(p.Id))
                .FirstOrDefault();
        }

        public Player? GetLoser()
        {
            if (CurrentSession == null || CurrentSession.Participants.Count == 0) return null;

            // Player with the maximum score loses
            return CurrentSession.Participants
                .OrderByDescending(p => GetTotalScore(p.Id))
                .FirstOrDefault();
        }
    }
}
