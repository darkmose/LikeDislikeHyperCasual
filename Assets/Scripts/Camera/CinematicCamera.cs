using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameEvents;
using System;

[RequireComponent(typeof(Camera))]
public class CinematicCamera : MonoBehaviour
{
    [SerializeField] private GameObject _entryLocationPrefab;
    [SerializeField] private GameObject _runLocationPrefab;
    [SerializeField] private GameObject _finishLocationPrefab;
    [SerializeField] private GameObject _fightLocationPrefab;
    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = GetComponent<Camera>();
        EventsAgregator.Subscribe<OnGameModeChangedEvent>(OnGameModeChangedHandler);
    }

    private void OnGameModeChangedHandler(object sender, OnGameModeChangedEvent data)
    {
        switch (GameModeHandler.CurrentState)
        {
            case States.Entry:
                ToEntryLocation();
                break;
            case States.Run:
                ToRunLocation();
                break;
            case States.Fight:
                ToFightLocation();
                break;
            case States.Finish:
                ToFinishLocation();
                break;
            case States.Lose:
                //Do nothing
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        ToEntryLocation();
    }

    private void ToEntryLocation() 
    {
        _playerCamera.transform.DOMove(_entryLocationPrefab.transform.position, Constants.GamemodeSwitchDelaySeconds);
        _playerCamera.transform.DORotate(_entryLocationPrefab.transform.rotation.eulerAngles, Constants.GamemodeSwitchDelaySeconds);
    }

    private void ToRunLocation() 
    {
        _playerCamera.transform.DOMove(_runLocationPrefab.transform.position, Constants.GamemodeSwitchDelaySeconds);
        _playerCamera.transform.DORotate(_runLocationPrefab.transform.rotation.eulerAngles, Constants.GamemodeSwitchDelaySeconds);
    }

    private void ToFinishLocation() 
    {
        _playerCamera.transform.DOMove(_finishLocationPrefab.transform.position, Constants.GamemodeSwitchDelaySeconds);
        _playerCamera.transform.DORotate(_finishLocationPrefab.transform.rotation.eulerAngles, Constants.GamemodeSwitchDelaySeconds);
    }

    private void ToFightLocation() 
    {
        _playerCamera.transform.DOMove(_fightLocationPrefab.transform.position, Constants.GamemodeSwitchDelaySeconds);
        _playerCamera.transform.DORotate(_fightLocationPrefab.transform.rotation.eulerAngles, Constants.GamemodeSwitchDelaySeconds);
    }




}
