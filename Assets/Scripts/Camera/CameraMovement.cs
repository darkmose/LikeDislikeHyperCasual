using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    private float _speedAcceleration = 1f;
    private Vector3 _moveOffset = Vector3.zero;
    private Dictionary<States, System.Action> _actions = new Dictionary<States, System.Action>();
    private System.Action _currentAction;

    #region Awake/Start

    private void Awake()
    {
        EventsAgregator.Subscribe<OnGameStateChangedEvent>(OnGameModeChangedHandler);
        _actions.Add(States.Run, RunAction);
        _actions.Add(States.Finish, FinishAction);
    }
    #endregion

    #region EVENT BASED

    private void OnGameModeChangedHandler(object sender, OnGameStateChangedEvent data)
    {
        switch (GameStatesHandler.CurrentState) //Entry Point
        {
            case States.Separate:
                var pos = this.transform.position;
                pos.y += 1f;
                pos.z -= 2f;
                var rotate = new Vector3(15, 0, 0);
                transform.DOMove(pos, Constants.SecondsToSeparateLike);
                transform.DORotate(rotate, Constants.SecondsToSeparateLike);
                break;
        }
        if (_actions.ContainsKey(GameStatesHandler.CurrentState))
        {
            _currentAction = _actions[GameStatesHandler.CurrentState];
        }
        else
        {
            _currentAction = NullAction;
        }
    }
    #endregion

    #region UNITY EVENTS


    private void OnDestroy()
    {
        EventsAgregator.Unsubscribe<OnGameStateChangedEvent>(OnGameModeChangedHandler);
    }

    #endregion

    #region ACTIONS

    private void NullAction() 
    { }

    private void FinishAction() 
    {
        _moveOffset.z = Time.fixedDeltaTime * Constants.PlayerFinishSpeed * _speedAcceleration;
        transform.position += _moveOffset;
        _speedAcceleration += Constants.PlayerSpeedAcceleration;

    }

    private void RunAction() 
    {
        _moveOffset.z = Time.fixedDeltaTime * Constants.PlayerSpeed;
        transform.position += _moveOffset;
    }

    #endregion

    #region UPDATES
    private void FixedUpdate()
    {
        _currentAction?.Invoke();
    }
    #endregion
}
