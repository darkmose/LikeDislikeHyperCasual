using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinScreen : MonoBehaviour
{
    [SerializeField] private Button _nextLevelButton;
    private LevelController _levelController;


    private void Awake()
    {
        _levelController = ServiceLocator.Resolve<LevelController>();
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClickHandler);
    }

    private void OnNextLevelButtonClickHandler()
    {
        _levelController.LoadNextLevel();
    }
}
