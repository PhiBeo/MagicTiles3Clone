# Magic Tiles 3 Clone

## Overview
This project is a rhythm-based piano tiles game built in Unity.  
Players tap tiles in sync with the music to score points.  
It demonstrates tile spawning, beat synchronization, and responsive UI mechanics.

## How to Run the Project

### Requirements
- Unity version: `2021.3.45f2`

### Steps to Run
1. Clone or download this repository.
2. Open the project in Unity Hub.
3. Open the scene named `Gameplay`.
4. Press `Play` in the Unity Editor to start the game.

## Design Choices
- **Core Mechanic:** Using the BPM and song length to determine how many beat the song have. From that calculate the speed and distance the tile need to spawn in order to reach the tap point perfect in the exact beat.
- **Level Setup:** Tile spawn time/beat are manually added
- **Tile Movement:** Tiles move based on BPM instead of a tile movement speed to synchronize gameplay with music.
- **UI System:** A very simple prototype design with a centralize script control the UI elements. (This can be seperate to scale up the project if needed)
- **Data-Driven Songs:** Song data (BPM and note timings) is stored in a ScriptableObject. A custom editor allows easy viewing and editing of notes and tiles directly in the Inspector.

## External Assets & Attribution
- Music track: `"Fly Away"` by `TheFatRat`
- Sprites: `Music Notation` and `Star icon`

## Notes
- Some minor optimizations may be needed for first-time tile spawn lag.  
- UI and particle effects may require adjustment based on screen resolution.
- `Manager` Game Object in the scene will have the GameManager script which allow the game to auto play.
- The beat is map out manually not auto generate on runtime so it will not sync 100%
