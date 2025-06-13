# Dungeon Crawler Game with Enemy Pathfinding

This project is a 2D dungeon crawler game prototype developed in Unity. Its main focus is on implementing an efficient A* pathfinding system for enemy artificial intelligence (AI), as well as foundational player combat and interaction mechanics.

## Key Features

* **Player Controls:** Responsive 8-directional movement and a health system.

* **Weapon System:**
    * The player can pick up and drop weapons.
    * Two weapon types are implemented: a Sword for melee attacks and a Gun for ranged attacks.
    * The currently equipped weapon is displayed on the UI.

* **Intelligent Enemy AI:**
    * Utilizes the **A\* algorithm** to find the most efficient path to the player or a roaming target point.
    * A **Binary Heap** is implemented to optimize A\* performance by efficiently managing the open set.
    * The AI has multiple states: Roaming, Chasing, and Attacking.
    * Enemies will patrol a defined area randomly when not provoked.
 
* **Combat System:**
    * The player can attack enemies using the equipped weapon.
    * Enemies can detect and attack the player when in range.
    * Health and damage system for both player and enemies.

* **User Interface (UI):**
    * A Health Bar to monitor the player's health.
    * A Pause Menu with options to resume, restart, or return to the main menu.
    * A Game Over screen that displays the final score.

* **Visual Debugging:**
    * Options to render the pathfinding grid, obstacles, and the calculated path in real-time.
    * Visualization of enemy attack and detection ranges for easy debugging.

## How to Play

| Control | Action |
| :--- | :--- |
| **WASD / Arrow Keys** | Move the player character. |
| **Left Mouse Click** | Attack with the currently equipped weapon. |
| **Right Mouse Click** | Pick up a nearby weapon or drop the currently held weapon. |
| **Escape Key** | Pause or resume the game. |

## Core Scripts Overview

### Pathfinding
* `Pathfinding.cs`: Implements the A\* algorithm to find a path. It manages the open and closed sets to evaluate nodes and reconstruct the most efficient path.
* `Grid.cs`: Creates a 2D navigation grid of `Node` objects. It can convert world positions to grid coordinates and detects obstacles using `Physics2D.OverlapBox`.
* `Node.cs`: Represents a single point in the pathfinding grid, storing its costs (G, H, and F) and its parent for path reconstruction.
* `BinaryHeap.cs`: A binary heap data structure used to optimize the A\* algorithm's open set, ensuring the lowest-cost node can be found quickly.

### Characters & Interaction
* `PlayerController.cs`: Manages all player input, movement, health, and interactions with UI systems like the health bar and game over screen.
* `EnemyAIController.cs`: The brain for all enemy behavior. It uses a simple state machine to switch between roaming, chasing the player, and attacking. It calls the pathfinding system for navigation.
* `WeaponHolder.cs`: Attached to the player, this script manages the logic for picking up, equipping, and unequipping weapons.
* `Sword.cs` / `Gun.cs`: Individual scripts that define the attack behavior for each weapon type.

### User Interface (UI)
* `Pause.cs`: Manages the pause menu functionality, such as freezing game time and displaying menu options.
* `Health_Bar.cs`: Updates the slider UI element to reflect the player's current health.
* `GameOverScreen.cs`: Displays the player's final score when the game ends.
