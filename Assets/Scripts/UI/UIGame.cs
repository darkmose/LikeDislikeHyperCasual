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
    private LevelController _levelController;

    private void Awake()
    {
        EventsAgregator.Subscribe<OnLevelProgressChangedEvent>(OnLevelProgressChangedHandler);
        EventsAgregator.Subscribe<OnNextLevelEvent>(OnNextLevelHandler);
        _tutorialStartButton.onClick.AddListener(OnTutorialStartButtonClickHandler);
        _levelController = ServiceLocator.Resolve<LevelController>();
    }

    private void OnNextLevelHandler(object sender, OnNextLevelEvent data)
    {
        _currentLevelText.text = (data.CurrentLevelIndex + 1).ToString();
    }

    private void OnTutorialStartButtonClickHandler()
    {
        _tutorialScreen.SetActive(false);
        GameStatesHandler.SetState(States.Run);
    }


    private void Start()
    {
        _currentLevelText.text = $"{ServiceLocator.Resolve<LevelController>().CurrentLevel +1}";
    }

    private void OnLevelProgressChangedHandler(object sender, OnLevelProgressChangedEvent data)
    {
        _progressBarValue.fillAmount = data.LevelProgress;
    }

}
