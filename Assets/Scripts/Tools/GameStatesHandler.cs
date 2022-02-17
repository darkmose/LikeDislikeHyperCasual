using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public enum States { Entry, Run, Fight, Finish, Lose, Win, Separate }

public static class GameStatesHandler 
{
    public static States CurrentState { get; private set; }
    private static OnGameStateChangedEvent _onGameModeChangedEvent = new OnGameStateChangedEvent();

    public static void SetState(States state) 
    {
        Debug.Log($"State switch to: {state}");
        CurrentState = state;
        EventsAgregator.Post<OnGameStateChangedEvent>(null, _onGameModeChangedEvent);  
    }

}
