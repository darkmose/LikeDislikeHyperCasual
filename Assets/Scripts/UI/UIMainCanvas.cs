using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;
using DG.Tweening;

public class UIMainCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private GameObject _loseGamePanel;
    [SerializeField] private GameObject _winGamePanel;

    private void Awake()
    {
        EventsAgregator.Subscribe<OnGameStateChangedEvent>(OnGamemodeChangedHandler);
    }

    private void OnGamemodeChangedHandler(object sender, OnGameStateChangedEvent data)
    {
        switch (GameStatesHandler.CurrentState)
        {
            case States.Entry:
                EntryAction();
                break;
            case States.Run:
                RunAction();
                break;
            case States.Fight:
                FightAction();
                break;
            case States.Finish:
                //Do nothing
                break;
            case States.Lose:
                LoseAction();
                break;
            case States.Win:
                WinAction();
                break;
            default:
                //Do nothing
                break;
        }
    }

    private void WinAction()
    {
        DOTween.Sequence().AppendInterval(.5f).OnComplete(() =>
        {
            _tutorialPanel.SetActive(false);
            _controlPanel.SetActive(false);
            _loseGamePanel.SetActive(false);
            _winGamePanel.SetActive(true);
        });

    }

    private void RunAction() 
    {
        _tutorialPanel.SetActive(false);
        _controlPanel.SetActive(true);
        _loseGamePanel.SetActive(false);
        _winGamePanel.SetActive(false);
    }

    private void LoseAction() 
    {
        _winGamePanel.SetActive(false);
        _tutorialPanel.SetActive(false);
        _controlPanel.SetActive(false);
        _loseGamePanel.SetActive(true);
    }
    private void FightAction() 
    {
        //Nothing yet
    }
    private void EntryAction() 
    {
        _tutorialPanel.SetActive(true);
        _controlPanel.SetActive(false);
        _loseGamePanel.SetActive(false);
        _winGamePanel.SetActive(false);
    }
}
