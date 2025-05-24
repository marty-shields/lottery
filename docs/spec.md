## Lottery Game Specification

Build a simplified lottery game as a console application in **C#** following these requirements:

### 1. Ticket Purchase

- The user (Player 1) is prompted via the console to purchase their desired number of tickets.
- Remaining participants are computer-generated players (CPU), labelled Player 2, Player 3, etc. Their ticket purchases are determined randomly.

### 2. Player Limits and Ticket Cost

- Total number of players: **10 to 15**.
- Each player can purchase **1 to 10 tickets**.
- Each player starts with a balance of **$10**.
- **Each ticket costs $1**.
- No player can purchase more tickets than their balance allows. If a player attempts to purchase more, only the affordable number is purchased.

### 3. Prize Distribution

- **Grand Prize:** One ticket wins **50%** of the total ticket revenue.
- **Second Tier:** **10%** of total tickets (rounded to the nearest whole number) share **30%** of the revenue equally.
- **Third Tier:** **20%** of total tickets (rounded to the nearest whole number) share **10%** of the revenue equally.

**Notes:**
- Tickets that have already won a prize are excluded from further prize tiers.
- Any remaining revenue after all prizes are distributed is the **house profit**.
- If the prize amount cannot be split exactly among winners, the closest equal split is used and any remainder goes to the house profit.

### Output Requirements

The program should display:

1. A list of all players and the number of tickets each purchased.
2. The list of winning players, including:
    - The prize tier won (Grand, Second, or Third).
    - The amount won.
3. The total casino (house) profit.