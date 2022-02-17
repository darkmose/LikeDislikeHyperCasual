using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameEvents;

public class LikesContainer : MonoBehaviour
{
    #region CONST
    private const int NormalSpawnRadius = 1;
    private const float SpawnRadiusScale = 0.01f;
    private const float SeparateSpawnRadiusScale = 0.1f;
    private const int MinSeparateLikesCount = 100;
    private const float SeparateLikeSize = .4f;
    #endregion

    #region FIELDS
    [SerializeField] private Text _likeCountText;
    [SerializeField] private Canvas _likeCountCanvas;
    private int _likesCount;
    public int LikesCount
    {
        get
        {
            return _likesCount;
        }
        private set
        {
            _likesCount = value;
            _likeCountText.text = _likesCount.ToString();
        }
    }

    private List<LikeController> _instantiatedLikes;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEndGameEvent _onEndGameEvent = new OnEndGameEvent();
    private LikeController _mainLike;
    private Vector3 _separateOffset = Vector3.zero;


    #endregion

    #region Awake/Start

    private void Awake()
    {
        _instantiatedLikes = new List<LikeController>();
        EventsAgregator.Subscribe<OnEnemyGroupDieEvent>(OnEnemyGroupDieHandler);
        EventsAgregator.Subscribe<OnLikePhotoEvent>(OnLikePhotoHandler);
    }


    #endregion

    #region EVENT BASED
    private void OnEnemyGroupDieHandler(object sender, OnEnemyGroupDieEvent data)
    {
        MoveToBase();
    }

    private void OnLikePhotoHandler(object sender, OnLikePhotoEvent data)
    {
        var randVal = Random.Range(0, _instantiatedLikes.Count);
        _instantiatedLikes[randVal].transform.SetParent(PooledSkinManager.PooledObjectRoot);
        _instantiatedLikes[randVal].gameObject.SetActive(false);
        LikesCount--;
        _instantiatedLikes.RemoveAt(randVal);
        if (LikesCount == 0)
        {
            GameStatesHandler.SetState(States.Win);
        }
    }
    #endregion

    #region CREATE LIKES

    public void CreateLikes(int count) 
    {
        StartCoroutine(CreateLikesCoroutine(count)); 
    }

    public void CreateFirstLike() 
    {
        _mainLike = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
        LikesCount++;
        _mainLike.transform.SetParent(transform);
        _mainLike.transform.localPosition = Vector3.zero;
        _mainLike.transform.localScale = Vector3.one;        
    }

    private IEnumerator CreateLikesCoroutine(int count) 
    {
        for (int i = 0; i < count; i++)
        {
             LikesCount++;
            _mainLike.transform.localScale = Vector3.one * (NormalSpawnRadius + (LikesCount * SpawnRadiusScale));
            yield return _waitForSeconds;
        }       
    }

    #endregion

    #region REMOVE LIKES
    private IEnumerator RemoveLikes(int count) 
    {
        if (count >= LikesCount)
        {
            var cnt = LikesCount - 1;
            for (int i = 0; i < cnt; i++)
            {
                LikesCount--;
                _mainLike.transform.localScale = Vector3.one * (NormalSpawnRadius + (LikesCount * SpawnRadiusScale));
                yield return _waitForSeconds;
            }
            LikesCount--;
            _mainLike.gameObject.SetActive(false);
            _mainLike.transform.SetParent(PooledSkinManager.PooledObjectRoot);

            EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
            GameStatesHandler.SetState(States.Lose);            
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                LikesCount--;
                _mainLike.transform.localScale = Vector3.one * (NormalSpawnRadius + (LikesCount * SpawnRadiusScale));
                yield return _waitForSeconds;
            }
        }
    }

    public void RemoveLike(int count) 
    {
        StartCoroutine(RemoveLikes(count));
    }

    #endregion

    #region INSTANIATE SEPARATED LIKES

    public void SeparateMainLike() 
    {
        _likeCountCanvas.enabled = false;

        _mainLike.PlaySeparateAnimation();
        DOTween.Sequence().AppendInterval(Constants.SecondsToSeparateLike).OnComplete(() =>
        {
            _mainLike.gameObject.SetActive(false);
            _mainLike.transform.SetParent(PooledSkinManager.PooledObjectRoot);
            SeparateLikesSphere();               
        });  
    }

    private void SeparateLikesSphere() 
    {

        if (LikesCount > MinSeparateLikesCount)
        {
            for (int i = 0; i < LikesCount; i++)
            {
                var controller = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
                _instantiatedLikes.Add(controller);
                controller.transform.SetParent(transform);
                _separateOffset = Random.onUnitSphere * (NormalSpawnRadius + (LikesCount * SeparateSpawnRadiusScale));
                _separateOffset.z = 0f;
                _separateOffset.x *= 15f / (float)LikesCount;
                _separateOffset.y *= 15f / (float)LikesCount;
                _separateOffset.y += 1.5f;
                controller.transform.localPosition = _separateOffset;
                controller.transform.localScale = Vector3.one * SeparateLikeSize;
                controller.transform.DOMove(transform.position + _separateOffset, Constants.SecondsToGoBase);
                if (controller.TryGetComponent(out SphereCollider sphereCollider))
                {
                    sphereCollider.enabled = true;
                }                
            }
        }
        else
        {
            for (int i = 0; i < LikesCount; i++)
            {
                var controller = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
                _instantiatedLikes.Add(controller);
                controller.transform.SetParent(transform);
                _separateOffset = Random.onUnitSphere * (NormalSpawnRadius + (LikesCount * SeparateSpawnRadiusScale));
                _separateOffset.z = 0f;
                _separateOffset.x *= 15f / (float)LikesCount;
                _separateOffset.y *= 10f / (float)LikesCount;
                _separateOffset.y += 1f;
                controller.transform.localPosition = _separateOffset;
                controller.transform.localScale = Vector3.one * SeparateLikeSize;
                controller.transform.DOMove(transform.position + _separateOffset, Constants.SecondsToGoBase);
                if (controller.TryGetComponent(out SphereCollider sphereCollider))
                {
                    sphereCollider.enabled = true;
                }
            }
        }
        DOTween.Sequence().AppendInterval(Constants.SecondsToGoBase).OnComplete(()=> 
        {
            GameStatesHandler.SetState(States.Finish);
        });
    }

    #endregion

    #region MOVEMENT LOGIC

    public void MoveToFight(Vector3 point)
    {
        _mainLike.transform.DOMove(point, Constants.SecondsToGoFight);
    }

    public void MoveToBase()
    {
        _mainLike.transform.DOMove(transform.position, Constants.SecondsToGoBase)
        .OnComplete(() =>
        {
            GameStatesHandler.SetState(States.Run);
        });
    }

    public void MoveSeparateLikes(float value) 
    {
        
    }

    #endregion

    #region UNITY EVENTS

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    #endregion
}