using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levelList;
    public int CurrentLevel{ get; private set; }
    private float _startPosZ;
    private float _finishPosZ;
    private Transform _player;
    private float _distance;
    private bool _isFinished = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private OnLevelProgressChangedEvent _onLevelProgressChangedEvent = new OnLevelProgressChangedEvent();
    private OnLevelFinishEvent _onLevelFinish = new OnLevelFinishEvent();
    private void Awake()
    {
        ServiceLocator.Register<LevelController>(this);
        LoadStartLevel();
    }

    private void LoadStartLevel()
    {
        if (!PlayerPrefs.HasKey(Constants.PrefsLevelProgressKey))
        {
            PlayerPrefs.SetInt(Constants.PrefsLevelProgressKey, 0);
        }
        CurrentLevel = PlayerPrefs.GetInt(Constants.PrefsLevelProgressKey);

        var level = Instantiate(_levelList[CurrentLevel]);
        PrepareLevelProgressCheck();
        StartCoroutine(CheckLevelProgress());

        _onLevelProgressChangedEvent.LevelProgress = 0f;
        EventsAgregator.Post<OnLevelProgressChangedEvent>(this, _onLevelProgressChangedEvent);
    }

    private void PrepareLevelProgressCheck() 
    {
        var playerObject = GameObject.FindGameObjectWithTag(Constants.PlayerTagName);
        if (!System.Object.ReferenceEquals(playerObject, null))
        {
            _startPosZ = playerObject.transform.position.z;
            _player = playerObject.transform;
        }

        var finishObj = GameObject.FindGameObjectWithTag(Constants.FinishTag);
        if (!System.Object.ReferenceEquals(finishObj, null))
        {
            _finishPosZ = finishObj.transform.position.z;
        }
        _distance = _finishPosZ - _startPosZ;
    }

    private IEnumerator CheckLevelProgress()
    {
        while (!_isFinished)
        {
            yield return _waitForFixedUpdate;
            _onLevelProgressChangedEvent.LevelProgress = 1f - (_finishPosZ - _player.position.z) / _distance;
            EventsAgregator.Post<OnLevelProgressChangedEvent>(this, _onLevelProgressChangedEvent);
            if (_finishPosZ < _player.position.z)
            {
                _onLevelProgressChangedEvent.LevelProgress = 1f;
                EventsAgregator.Post<OnLevelFinishEvent>(this, _onLevelFinish);
                _isFinished = true;
                GameStatesHandler.SetState(States.Finish);
            }
        }
    }


    private void OnDestroy()
    {
        ServiceLocator.Ungerister<LevelController>();
    }

}
