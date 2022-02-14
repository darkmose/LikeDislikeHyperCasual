using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameEvents;

public class LikesContainer : MonoBehaviour
{
    private const int NormalSpawnRadius = 1;
    private const float SpawnRadiusScale = 0.01f;
    private const int SecondsToGoFight = 1;
    private const int SecondsToGoBase = 1;
    private const float HalfWay = 0.5f;

    [SerializeField] private Text _likeCountText;

    private List<LikeController> _likes;
    public int LikesCount => _likes.Count;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEndGameEvent _onEndGameEvent = new OnEndGameEvent();

    #region TEST
    private LikeController _testLikeController;
    #endregion

    private void Awake()
    {
        PrepareLikesContainer();
        EventsAgregator.Subscribe<OnLikeEnterEnemyEvent>(OnLikeEnterEnemyHandler);
        EventsAgregator.Subscribe<OnEnemyGroupDieEvent>(OnEnemyGroupDieHandler);
    }

    private void OnEnemyGroupDieHandler(object sender, OnEnemyGroupDieEvent data)
    {
        MoveToBase();
    }

    private void OnLikeEnterEnemyHandler(object sender, OnLikeEnterEnemyEvent data)
    {
        RemoveLike(data.dislikeControllerCount);
    }

    private void PrepareLikesContainer() 
    {
        _likes = new List<LikeController>();
    }

    public void CreateLike(int count) 
    {
        StartCoroutine(CreateLikes(count)); 
    }

    public void CreateLike() 
    {
        _testLikeController = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
        _likes.Add(_testLikeController);
        _testLikeController.transform.SetParent(transform);
        _testLikeController.transform.localPosition = Vector3.zero;
        _testLikeController.transform.localScale = Vector3.one;
        _likeCountText.text = LikesCount.ToString();
        
    }

    private IEnumerator CreateLikes(int count) 
    {
        for (int i = 0; i < count; i++)
        {
            var like = new LikeController();
            _likes.Add(like);
            _testLikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (LikesCount * SpawnRadiusScale));
            _likeCountText.text = LikesCount.ToString();
            yield return _waitForSeconds;
        }
       
    }

    private IEnumerator RemoveLikes(int count) 
    {
        for (int i = 0; i < _likes.Count; i++)
        {
            if (LikesCount > 1)
            {
                _likes.RemoveAt(1);
                _testLikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (LikesCount * SpawnRadiusScale));
                _likeCountText.text = LikesCount.ToString();
            }
            else
            {
                _testLikeController.gameObject.SetActive(false);
                _testLikeController.transform.SetParent(PooledSkinManager.PooledObjectRoot);
                _likeCountText.text = LikesCount.ToString();

                EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                GameModeHandler.SetState(States.Lose);
                break;
            }

            yield return _waitForSeconds;
        }

        

    }


    //Do the coroutine
    public void MoveToBase() 
    {        
        var doMove = _testLikeController.transform.DOMove(transform.position, SecondsToGoBase);
        DOTween.Sequence().AppendInterval(SecondsToGoBase)
            .OnComplete(Method);
    }

    private void Method() 
    {
        GameModeHandler.SetState(States.Run);
        Debug.Log("Method");
    }

    public void RemoveLike(int count) 
    {
        StartCoroutine(RemoveLikes(count));
    }


    public void SendToFight(DislikeContainer dislikeContainer) 
    {
        var averagePoint = Vector3.Lerp(transform.position, dislikeContainer.transform.position, HalfWay);
        dislikeContainer.SendToFight(averagePoint);

        _testLikeController.transform.DOMove(averagePoint, SecondsToGoFight); 
    }

}