using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class GameManager
{

    public enum GameState
    {
        PREGAME,
        INGAME,
        ENDGAME
    }

    public static InGameUI UI;
    public static ObstacleSpawner Spawner;
    public static PlayerController Player;


    public static float Speed;


    /** The size of the platform in units. */
    public static int PlatformSize;


    public static float SpawnMin;


    public static float SpawnMax;


    public static GameState CurrentState;


    public static float Distance;


    public static int Ranking;




    


    public static void StartGame()
    {
        CurrentState = GameState.INGAME;
        UI.ShowState(CurrentState);
    }

    public static void EndGame()
    {
        CurrentState = GameState.ENDGAME;
        UI.ShowState(CurrentState);
    }


    /**
    * Reset all values to default.
    */
    public static void Reset()
    {
        Speed = 10f;
        PlatformSize = 4;
        SpawnMin = 10f;
        SpawnMax = 20f;
        CurrentState = GameState.PREGAME;
        Distance = 0.0f;
        Ranking = 0;

        UI.Reset();
        Spawner.Reset();
        Player.Reset();
    }



}