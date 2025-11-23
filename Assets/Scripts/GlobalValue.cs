using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValue
{
    // Global game settings and constants
    public static int LandCount = 4;
    public static float SpawnY = 12f;
    public static float UnitPerBeat = 3f;
    public static Vector2 TileSize = new Vector2(1.4f, 2.6f);
    public static Vector2 LaneSize = new Vector2(1.4f, 10f);
    public static float[] LaneXPosition = {-2.1f, -0.7f, 0.7f, 2.1f};

    // Score and Tile Timing
    public static float perfectDistance = UnitPerBeat * 0.15f;
    public static float greatDistance = UnitPerBeat * 0.25f;
    public static float goodDistance = UnitPerBeat * 0.35f;
    public static float perfectTick = 1f;
    public static float greatTick = 0.8f;
    public static float goodTick = 0.5f;

    // Animation
    public static float tileFadeTime = 0.15f;

    public static bool IsWithinRange(Vector2 input, Vector2 pos, Vector2 bound)
    {
        return (input.x >= pos.x - bound.x / 2f && input.x <= pos.x + bound.x / 2f) &&
               (input.y >= pos.y - bound.y / 2f && input.y <= pos.y + bound.y / 2f);
    }
}
