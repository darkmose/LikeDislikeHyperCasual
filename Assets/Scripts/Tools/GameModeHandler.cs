using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public enum States { Entry, Run, Fight, Finish, Lose }

public static class GameModeHandler 
{
    public static States CurrentState { get; private set; }
    private static OnGameModeChangedEvent _onGameModeChangedEvent = new OnGameModeChangedEvent();

    public static void SetState(States state) 
    {
        Debug.Log($"State switch to: {state}");
        CurrentState = state;
        EventsAgregator.Post<OnGameModeChangedEvent>(null, _onGameModeChangedEvent);  
    }

}
