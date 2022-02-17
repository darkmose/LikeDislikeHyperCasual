using DG.Tweening;
using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DislikeContainer : MonoBehaviour
{
    #region CONST
    private const int NormalSpawnRadius = 1;
    private const float SpawnRadiusScale = 0.01f;
    #endregion

    #region FIELDS
    [Tooltip("Count of enemies")]
    [SerializeField] private int _startDislikeCount;
    [SerializeField] private Text _dislikeCountText;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEnemyGroupDieEvent _onEnemyGroupDieEvent = new OnEnemyGroupDieEvent();
    private DislikeController _mainDislike;
    private int _dislikesCount;
    public int DislikesCount
    {
        get
        {
            return _dislikesCount;
        }
        private set
        {
            _dislikesCount = value;
            _dislikeCountText.text = _dislikesCount.ToString();
        }
    }

    #endregion


    [ContextMenu("TEST")]
    public void Test() {
        Debug.Log($"DislikeCount: {DislikesCount}");
    }


    #region Awake/Start
    private void Start()
    {
        CreateDislike(_startDislikeCount);
    }
    #endregion

    #region CREATE DISLIKE
    private void CreateDislike(int count) 
    {
        _mainDislike = Factory.AbstractFactory.CreateDislikeController(Factory.LikeSkin.Common);
        DislikesCount++;
        _mainDislike.transform.SetParent(transform);
        _mainDislike.transform.localPosition = Vector3.zero;
        _mainDislike.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));

        for (int i = 1; i < count; i++)
        {
            DislikesCount++;            
            _mainDislike.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
        }

    }
    #endregion

    #region REMOVE DISLIKE
    private IEnumerator RemoveDislike(int count) 
    {
        if (count >= DislikesCount)
        {
            var cnt = DislikesCount - 1;
            for (int i = 0; i < cnt; i++)
            {
                DislikesCount--;
                _mainDislike.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
                yield return _waitForSeconds;
            }
            DislikesCount--;
            _mainDislike.transform.SetParent(PooledSkinManager.PooledObjectRoot);
            _mainDislike.gameObject.SetActive(false);
            EventsAgregator.Post<OnEnemyGroupDieEvent>(this, _onEnemyGroupDieEvent);
            this.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                DislikesCount--;
                _mainDislike.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
                yield return _waitForSeconds;
            }
        }
    }

    public void RemoveDislikes(int count)
    {
        StartCoroutine(RemoveDislike(count));                
    }
    #endregion

    #region DISLIKE MOVEMENT
    public void SendToFight(Vector3 fightPoint)
    {
        _mainDislike.transform.DOMove(fightPoint, Constants.SecondsToGoFight);
    }
    #endregion

    #region UNITY EVENTS
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion

}
