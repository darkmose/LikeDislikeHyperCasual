using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using System;

public class UIGame : MonoBehaviour
{
    [SerializeField] private Image _progressBarValue;
    [SerializeField] private Text _currentLevelText;
    [SerializeField] private GameObject _tutorialScreen;
    [SerializeField] private Button _tutorialStartButton;

    private void Awake()
    {
        EventsAgregator.Subscribe<OnLevelProgressChangedEvent>(OnLevelProgressChangedHandler);
        _tutorialStartButton.onClick.AddListener(OnTutorialStartButtonClickHandler);
    }

    private void OnTutorialStartButtonClickHandler()
    {
        Debug.Log("[OnTutorialStartButtonClickHandler]");
        _tutorialScreen.SetActive(false);
        GameModeHandler.SetState(States.Run);
    }


    private void Start()
    {
        _currentLevelText.text = $"Level {ServiceLocator.Resolve<LevelController>().CurrentLevel +1}";
    }

    private void OnLevelProgressChangedHandler(object sender, OnLevelProgressChangedEvent data)
    {
        _progressBarValue.fillAmount = data.LevelProgress;
    }

}
