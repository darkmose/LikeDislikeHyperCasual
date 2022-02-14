using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;

public class UIMainCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private GameObject _loseGamePanel;
    [SerializeField] private GameObject _winGamePanel;

    private void Awake()
    {
        EventsAgregator.Subscribe<OnGameModeChangedEvent>(OnGamemodeChangedHandler);
    }

    private void OnGamemodeChangedHandler(object sender, OnGameModeChangedEvent data)
    {
        switch (GameModeHandler.CurrentState)
        {
            case States.Entry:
                _tutorialPanel.SetActive(true);
                _controlPanel.SetActive(false);
                _loseGamePanel.SetActive(false);
                _winGamePanel.SetActive(false);
                break;
            case States.Run:
                _tutorialPanel.SetActive(false);
                _controlPanel.SetActive(true);
                _loseGamePanel.SetActive(false);
                _winGamePanel.SetActive(false);
                break;
            case States.Fight:
                //Do nothing
                break;
            case States.Finish:
                _winGamePanel.SetActive(true);
                _tutorialPanel.SetActive(false);
                _controlPanel.SetActive(false);
                _loseGamePanel.SetActive(false);
                break;
            case States.Lose:
                _winGamePanel.SetActive(false);
                _tutorialPanel.SetActive(false);
                _controlPanel.SetActive(false);
                _loseGamePanel.SetActive(true);
                break;
            default:
                //Do nothing
                break;
        }
    }

}
