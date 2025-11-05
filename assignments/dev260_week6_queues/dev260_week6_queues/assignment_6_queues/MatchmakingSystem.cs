namespace Assignment6
{
    /// <summary>
    /// Main matchmaking system managing queues and matches
    /// Students implement the core methods in this class
    /// </summary>
    public class MatchmakingSystem
    {
        // Data structures for managing the matchmaking system
        private Queue<Player> casualQueue = new Queue<Player>();
        private Queue<Player> rankedQueue = new Queue<Player>();
        private Queue<Player> quickPlayQueue = new Queue<Player>();
        private List<Player> allPlayers = new List<Player>();
        private List<Match> matchHistory = new List<Match>();

        // Statistics tracking
        private int totalMatches = 0;
        private DateTime systemStartTime = DateTime.Now;

        /// <summary>
        /// Create a new player and add to the system
        /// </summary>
        public Player CreatePlayer(string username, int skillRating, GameMode preferredMode = GameMode.Casual)
        {
            // Check for duplicate usernames
            if (allPlayers.Any(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Player with username '{username}' already exists");
            }

            var player = new Player(username, skillRating, preferredMode);
            allPlayers.Add(player);
            return player;
        }

        /// <summary>
        /// Get all players in the system
        /// </summary>
        public List<Player> GetAllPlayers() => allPlayers.ToList();

        /// <summary>
        /// Get match history
        /// </summary>
        public List<Match> GetMatchHistory() => matchHistory.ToList();

        /// <summary>
        /// Get system statistics
        /// </summary>
        public string GetSystemStats()
        {
            var uptime = DateTime.Now - systemStartTime;
            var avgMatchQuality = matchHistory.Count > 0
                ? matchHistory.Average(m => m.SkillDifference)
                : 0;

            return $"""
                🎮 Matchmaking System Statistics
                ================================
                Total Players: {allPlayers.Count}
                Total Matches: {totalMatches}
                System Uptime: {uptime.ToString("hh\\:mm\\:ss")}
                
                Queue Status:
                - Casual: {casualQueue.Count} players
                - Ranked: {rankedQueue.Count} players  
                - QuickPlay: {quickPlayQueue.Count} players
                
                Match Quality:
                - Average Skill Difference: {avgMatchQuality:F1}
                - Recent Matches: {Math.Min(5, matchHistory.Count)}
                """;
        }

        // ============================================
        // STUDENT IMPLEMENTATION METHODS (TO DO)
        // ============================================

        /// <summary>
        /// TODO: Add a player to the appropriate queue based on game mode
        /// 
        /// Requirements:
        /// - Add player to correct queue (casualQueue, rankedQueue, or quickPlayQueue)
        /// - Call player.JoinQueue() to track queue time
        /// - Handle any validation needed
        /// </summary>
        public void AddToQueue(Player player, GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Use switch statement on mode to select correct queue
            // Don't forget to call player.JoinQueue()!
            player.JoinQueue();
            switch (mode)
            {
                case GameMode.Casual:
                    casualQueue.Enqueue(player);
                    break;
                case GameMode.Ranked:
                    rankedQueue.Enqueue(player);
                    break;
                case GameMode.QuickPlay:
                    quickPlayQueue.Enqueue(player);
                    break;
                default:
                    throw new ArgumentException($"Unknown game mode: {mode}");
            }
        }

        /// <summary>
        /// TODO: Try to create a match from the specified queue
        /// 
        /// Requirements:
        /// - Return null if not enough players (need at least 2)
        /// - For Casual: Any two players can match (simple FIFO)
        /// - For Ranked: Only players within ±2 skill levels can match
        /// - For QuickPlay: Prefer skill matching, but allow any match if queue > 4 players
        /// - Remove matched players from queue and call LeaveQueue() on them
        /// - Return new Match object if successful
        /// </summary>
        public Match? TryCreateMatch(GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Different logic needed for each mode
            // Remember to check queue count first!
            var queue = GetQueueByMode(mode);
            if (queue.Count < 2)
            {
                return null;
            }
            Player p1 = queue.Peek(); // look at first player

            switch (mode)
            {
                case GameMode.Casual:
                    {
                        // Simple FIFO match
                        p1 = queue.Dequeue(); // remove first player
                        Player p2 = queue.Dequeue(); // remove second player
                        p1.LeaveQueue();
                        p2.LeaveQueue();
                        return new Match(p1, p2, mode);
                    }
                case GameMode.Ranked:
                    {
                        // First player tries to find someone within ±2 skill levels
                        var players = queue.ToList();
                        foreach (var p2 in queue)
                        {
                            if (CanMatchInRanked(p1, p2) && p1 != p2)
                            {
                                // Found a match
                                var tempQueue = new Queue<Player>(queue.Where(p => p != p1 && p != p2));
                                queue.Clear();
                                foreach (var p in tempQueue)
                                    queue.Enqueue(p);
                                p1.LeaveQueue();
                                p2.LeaveQueue();
                                return new Match(p1, p2, mode);
                            }
                        }
                        return null; // No match found
                    }
                case GameMode.QuickPlay:
                    {
                        foreach (var p2 in queue)
                        {
                            if (CanMatchInRanked(p1, p2) && p1 != p2)
                            {
                                // Found a skill match
                                var tempQueue = new Queue<Player>(queue.Where(p => p != p1 && p != p2));
                                queue.Clear();
                                foreach (var p in tempQueue)
                                    queue.Enqueue(p);
                                p1.LeaveQueue();
                                p2.LeaveQueue();
                                return new Match(p1, p2, mode);
                            }
                        }
                        // No skill match found, check if we can match anyone
                        if (queue.Count > 4)
                        {
                            Player p2 = queue.ElementAt(1); // second player
                            queue.Dequeue(); // remove first player
                            queue.Dequeue(); // remove second player
                            p1.LeaveQueue();
                            p2.LeaveQueue();
                            return new Match(p1, p2, mode);
                        }
                        return null; // No match found
                    }
                default:
                    throw new ArgumentException($"Unknown game mode: {mode}");
            }

        }

        /// <summary>
        /// TODO: Process a match by simulating outcome and updating statistics
        /// 
        /// Requirements:
        /// - Call match.SimulateOutcome() to determine winner
        /// - Add match to matchHistory
        /// - Increment totalMatches counter
        /// - Display match results to console
        /// </summary>
        public void ProcessMatch(Match match)
        {
            // TODO: Implement this method
            // Hint: Very straightforward - simulate, record, display
            match.SimulateOutcome();
            matchHistory.Add(match);
            totalMatches++;
            Console.WriteLine($"🏆 Match Result: {match.Player1.Username} vs {match.Player2.Username} - Winner: {match.Winner?.Username} (Prob: {match.WinProbability:P1})");
        }

        /// <summary>
        /// TODO: Display current status of all queues with formatting
        /// 
        /// Requirements:
        /// - Show header "Current Queue Status"
        /// - For each queue (Casual, Ranked, QuickPlay):
        ///   - Show queue name and player count
        ///   - List players with position numbers and queue times
        ///   - Handle empty queues gracefully
        /// - Use proper formatting and emojis for readability
        /// </summary>
        public void DisplayQueueStatus()
        {
            // TODO: Implement this method
            // Hint: Loop through each queue and display formatted information
            Console.WriteLine("🎮 Current Queue Status");
            foreach (var mode in Enum.GetValues<GameMode>())
            {
                var queue = GetQueueByMode(mode);
                Console.WriteLine($"\n--- {mode} Queue ({queue.Count} players) ---");
                if (queue.Count == 0)
                {
                    Console.WriteLine("   (empty)");
                }
                else
                {
                    int position = 1;
                    foreach (var player in queue)
                    {
                        var waitTime = DateTime.Now - player.JoinedQueue;
                        Console.WriteLine($"   {position}. {player.Username} - Waiting {waitTime.Minutes}m {waitTime.Seconds}s");
                        position++;
                    }
                }
            }
        }

        /// <summary>
        /// TODO: Display detailed statistics for a specific player
        /// 
        /// Requirements:
        /// - Use player.ToDetailedString() for basic info
        /// - Add queue status (in queue, estimated wait time)
        /// - Show recent match history for this player (last 3 matches)
        /// - Handle case where player has no matches
        /// </summary>
        public void DisplayPlayerStats(Player player)
        {
            // TODO: Implement this method
            // Hint: Combine player info with match history filtering
            Console.WriteLine(player.ToDetailedString());
            var queue = GetQueueByMode(player.PreferredMode);
            if (queue.Contains(player))
            {
                Console.WriteLine($"Status: In {player.PreferredMode} Queue");
                var estimate = GetQueueEstimate(player.PreferredMode);
                Console.WriteLine($"Estimated Wait Time: {estimate}");
            }
            else
            {
                Console.WriteLine("Status: Not in Queue");
            }
            var recentMatches = matchHistory
                .Where(m => m.Player1 == player || m.Player2 == player)
                .TakeLast(3)
                .ToList();
            if (recentMatches.Count == 0)
            {
                Console.WriteLine("No recent matches.");
            }
            else
            {
                Console.WriteLine("Recent Matches:");
                foreach (var match in recentMatches)
                {
                    var result = match.Winner == player ? "Won" : "Lost";
                    Console.WriteLine($" - {match.Player1.Username} vs {match.Player2.Username} on {match.MatchTime}: {result}");
                }
            }
        }

        /// <summary>
        /// TODO: Calculate estimated wait time for a queue
        /// 
        /// Requirements:
        /// - Return "No wait" if queue has 2+ players
        /// - Return "Short wait" if queue has 1 player
        /// - Return "Long wait" if queue is empty
        /// - For Ranked: Consider skill distribution (harder to match = longer wait)
        /// </summary>
        public string GetQueueEstimate(GameMode mode)
        {
            // TODO: Implement this method
            // Hint: Check queue counts and apply mode-specific logic
            var queue = GetQueueByMode(mode);
            if (queue.Count >= 2)
            {
                return "No wait";
            }
            else if (queue.Count == 1)
            {
                return "Short wait";
            }
            else
            {
                return "Long wait";
            }
        }

        // ============================================
        // HELPER METHODS (PROVIDED)
        // ============================================

        /// <summary>
        /// Helper: Check if two players can match in Ranked mode (±2 skill levels)
        /// </summary>
        private bool CanMatchInRanked(Player player1, Player player2)
        {
            return Math.Abs(player1.SkillRating - player2.SkillRating) <= 2;
        }

        /// <summary>
        /// Helper: Remove player from all queues (useful for cleanup)
        /// </summary>
        private void RemoveFromAllQueues(Player player)
        {
            // Create temporary lists to avoid modifying collections during iteration
            var casualPlayers = casualQueue.ToList();
            var rankedPlayers = rankedQueue.ToList();
            var quickPlayPlayers = quickPlayQueue.ToList();

            // Clear and rebuild queues without the specified player
            casualQueue.Clear();
            foreach (var p in casualPlayers.Where(p => p != player))
                casualQueue.Enqueue(p);

            rankedQueue.Clear();
            foreach (var p in rankedPlayers.Where(p => p != player))
                rankedQueue.Enqueue(p);

            quickPlayQueue.Clear();
            foreach (var p in quickPlayPlayers.Where(p => p != player))
                quickPlayQueue.Enqueue(p);

            player.LeaveQueue();
        }

        /// <summary>
        /// Helper: Get queue by mode (useful for generic operations)
        /// </summary>
        private Queue<Player> GetQueueByMode(GameMode mode)
        {
            return mode switch
            {
                GameMode.Casual => casualQueue,
                GameMode.Ranked => rankedQueue,
                GameMode.QuickPlay => quickPlayQueue,
                _ => throw new ArgumentException($"Unknown game mode: {mode}")
            };
        }
    }
}