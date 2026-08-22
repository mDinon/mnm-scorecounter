# Specification: Uno Score Counter Application

## 1. Project Overview
A cross-platform .NET MAUI Blazor Hybrid application to track Uno game scores, manage persistent player lists, and visualize score progression over time.

## 2. Data Models
### 2.1 Player
- `Guid Id`
- `string Name`
- `string Color` (Hex code for graph lines)

### 2.2 GameSession
- `Guid Id`
- `DateTime StartTime`
- `int MaxScoreLimit` (e.g., 500)
- `List<Player> Participants`
- `List<Round> Rounds`
- `bool IsEnded`

### 2.3 Round
- `int RoundNumber`
- `Dictionary<Guid, int> Scores` (Key: PlayerId, Value: Score)

## 3. Storage Architecture
- **Location:** `FileSystem.AppDataDirectory`
- **File:** `players.json`
- **Format:** `List<Player>`
- **Logic:** `PlayerService` handles read/write operations using `System.Text.Json`.

## 4. UI/UX & Navigation
### 4.1 Home / Setup
- **Player Management:** CRUD interface for the `players.json` list.
- **Game Setup:** Selection interface (checkboxes) to select active players for the current game. Input field for `MaxScoreLimit`.

### 4.2 Active Scoreboard
- **Layout:** Table where:
  - Header: Player names.
  - Rows: `RoundNumber`, followed by score inputs per player.
  - Footer: Summed total per column.
- **Features:** 
  - Real-time Timer (Header).
  - "End Game Prematurely" button.
  - Validation: Cell inputs trigger total recalculation.

### 4.3 Analytics Tab
- **Visuals:** Linear chart displaying score progression.
- **Logic:** X-axis = `RoundNumber`, Y-axis = `CumulativeScore`, Lines = `Player`.

## 5. Business Logic Rules
- **Game End:** Triggered if `TotalScore >= MaxScoreLimit` OR `EndGamePrematurely` is clicked.
- **Winner:** The player with the lowest score at game end (Standard Uno scoring rule).

### 6. Design
- sidebar with options Players and Game. Game must be first and default option to setup game, Player config doesn't need to be always visible. Extract player setup to new page `Payers.razor`
- timer must be smaller so that it doesn't waste too much space.
- app should be stretched maximally, don't wrap it into multiple containers because that wastes space.
- grid must have horizontal scroll, i want to move left and right to inspect values
- support light and dark theme with dark theme being default theme
- game end is triggered if all fields are entered on last row and one column goes beyond game MaxScoreLimit.
- all values in table row must be empty, not 0.
- top menu is unnecessary, hamburger button should expand sidebar.
- Round column should have only # in header and number in row. Example 1. instead of Round 1
- running total replace with some sum icon that will represent column sum.
- generate home icon and app should be called Uno Counter

