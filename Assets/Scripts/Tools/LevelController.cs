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
    private OnNextLevelEvent _onNextLevelEvent = new OnNextLevelEvent();
    private void Awake()
    {
        ServiceLocator.Register<LevelController>(this);
        LoadStartLevel();
    }

    public void LoadNextLevel() 
    {
        if (CurrentLevel < _levelList.Count - 1)
        {
            PooledSkinManager.ReturnAllObjectsInPool();
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            CurrentLevel++;
            PlayerPrefs.SetInt(Constants.PrefsLevelProgressKey, CurrentLevel);
            var level = Instantiate(_levelList[CurrentLevel]);
            level.transform.SetParent(transform);
            var playerObject = level.transform.Find("LikesGroup");
            Debug.Log($"[LoadNextLevel] playerObject is: {playerObject.transform.position}");

            PrepareLevelProgressCheck(level);
            StartCoroutine(CheckLevelProgress());

            _onNextLevelEvent.CurrentLevelIndex = CurrentLevel;
            EventsAgregator.Post<OnNextLevelEvent>(this, _onNextLevelEvent);
            GameStatesHandler.SetState(States.Entry);
        }
        else
        {
            Debug.LogWarning($"Level {CurrentLevel + 1} doesn't exist");
        }

    }

    public void LoadStartLevel()
    {
        if (!PlayerPrefs.HasKey(Constants.PrefsLevelProgressKey))
        {
            PlayerPrefs.SetInt(Constants.PrefsLevelProgressKey, 0);
        }
        CurrentLevel = PlayerPrefs.GetInt(Constants.PrefsLevelProgressKey);

        var level = Instantiate(_levelList[CurrentLevel]);
        level.transform.SetParent(transform);

        PrepareLevelProgressCheck(level);
        StartCoroutine(CheckLevelProgress());

        _onLevelProgressChangedEvent.LevelProgress = 0f;
        EventsAgregator.Post<OnLevelProgressChangedEvent>(this, _onLevelProgressChangedEvent);
    }

    private void PrepareLevelProgressCheck(GameObject level) 
    {
        var startFinishHandler = level.GetComponent<StartFinishHandler>();

        var playerTransform = startFinishHandler._playerPoint;

        Debug.Log($"playerObject pos is: {playerTransform.transform.position}");

        if (!System.Object.ReferenceEquals(playerTransform, null))
        {
            _startPosZ = playerTransform.transform.position.z;
            _player = playerTransform;
            Debug.Log($"[PrepareLevelProgressCheck] StartPosZ: {_startPosZ}");
        }
        var finishTransform = startFinishHandler._finishPoint;
        if (!System.Object.ReferenceEquals(finishTransform, null))
        {
            _finishPosZ = finishTransform.position.z;
            Debug.Log($"[PrepareLevelProgressCheck] FinishPosZ: {_finishPosZ}");
        }
        _distance = _finishPosZ - _startPosZ;
        Debug.Log($"Distance: {_distance}");
        _isFinished = false;
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
                GameStatesHandler.SetState(States.Separate);
            }
        }
    }


    private void OnDestroy()
    {
        ServiceLocator.Ungerister<LevelController>();
    }

}
