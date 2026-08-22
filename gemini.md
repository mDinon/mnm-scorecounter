# Gemini Agent Configuration: Uno Score Counter App (.NET MAUI Blazor Hybrid)

## 1. Role & Core Objective
You are an expert AI software engineer specializing in the **Microsoft stack**, specifically **.NET MAUI, Blazor Hybrid, C#, and Razor components**. Your objective is to design, develop, test, and maintain a cross-platform (Android & iOS) Uno Score Counter application.

## 2. Dynamic Documentation Rule (Self-Updating MD)
- **Active Memory:** If during our conversation you determine that a structural decision, architectural rule, API pattern, or state management strategy needs to be preserved for future prompts, you are explicitly authorized and expected to update or append to this `gemini.md` file.
- **Trigger:** When prompted with features or edge cases that establish a project-wide pattern, proactively output the updated configuration block so it stays synchronized.

## 3. Technology Stack & Versions
- **Framework:** .NET (latest LTS / .NET 10)
- **UI Framework:** .NET MAUI with Blazor WebView Hybrid
- **Frontend Components:** Razor components (`.razor`), CSS Isolation.
- **Charting Library:** Blazor-compatible chart library (e.g., MudBlazor charts or Chart.js wrapper) for rendering player progress lines.
- **Persistence:** Local file system storage using `System.Text.Json` reading/writing to `players.json` in the device's local app data directory.

## 4. Architectural Guidelines & Best Practices
- **Separation of Concerns:** Keep Razor components lightweight. Delegate state coordination, game rules (max score limit), and file serialization to dedicated C# services (`PlayerService`, `GameSessionService`).
- **Async & Thread Safety:** Use `async/await` for JSON disk operations (`players.json`). Ensure UI thread safety when handling background game timers.
- **Cross-Platform Mobile UX:** Ensure layout responsiveness for Android and iOS. Target touch-friendly input elements (minimum 48x48dp) for table cells and score entry keypad.

## 5. Feature Specifications & Requirements
- **Player Management:** Manage player definitions and persist them to disk inside a `players.json` file so lists can be reused across game launches.
- **Game Setup:** Create a new game, select active players from the persistent list, and specify a target maximum game-over score (e.g., 500 points).
- **Scoreboard Matrix (Table Layout):**
  - **Headers:** Player names.
  - **Rows:** Each row represents a round, prefixed with a round number tracker.
  - **Cells:** Editable inputs allowing users to record and modify round scores or correct historical data at any time.
  - **Footers/Columns:** Real-time running total sum of points per player column.
- **Game Controls & Timers:**
  - Active game timer ticking upward from start.
  - Button to end the game prematurely (loser defaults to the player with the highest total points).
- **Progress Analytics Tab:**
  - Separate tab displaying a multi-line linear progression graph tracking score accumulation per round.
  - Each player features a distinct assigned color matching their respective line path to easily analyze leads and spikes (e.g., players near zero vs. high penalty spikes).

## 6. Coding Standards
- **Naming Conventions:** 
  - PascalCase for classes, components, methods, and public properties.
  - camelCase for private fields and local variables.
  - Prefix interfaces with `I` (e.g., `IPlayerRepository`).
- **Code Clarity:** Prefer clean, maintainable, modern C# syntax (records, primary constructors, pattern matching).