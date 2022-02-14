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
    private Vector3 localSpawnPoint = Vector3.zero;
    private float spawnRadiusScaler = 1f;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEndGameEvent _onEndGameEvent = new OnEndGameEvent();

    #region TEST
    [SerializeField] private bool _testLike;
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
        RemoveLike(data.likeController);
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
        if (_testLike)
        {
            _testLikeController = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
            _likes.Add(_testLikeController);
            _testLikeController.transform.SetParent(transform);
            _testLikeController.transform.localPosition = Vector3.zero;
            _testLikeController.transform.localScale = Vector3.one;
            _likeCountText.text = LikesCount.ToString();
        }
        else
        {
            var like = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
            _likes.Add(like);
            like.transform.SetParent(transform);
            like.transform.localPosition = Vector3.zero;
            _likeCountText.text = LikesCount.ToString();
        }

    }

    private IEnumerator CreateLikes(int count) 
    {
        if (_testLike)
        {
            for (int i = 0; i < count; i++)
            {
                var like = new LikeController();
                _likes.Add(like);
                _testLikeController.transform.localScale = Vector3.one * (1 + (LikesCount * SpawnRadiusScale));
                _likeCountText.text = LikesCount.ToString();
                yield return _waitForSeconds;
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                var like = Factory.AbstractFactory.CreateLikeController(Factory.LikeSkin.Common);
                _likes.Add(like);
                like.transform.SetParent(transform);
                spawnRadiusScaler = NormalSpawnRadius + LikesCount * SpawnRadiusScale;
                localSpawnPoint = Random.insideUnitSphere * spawnRadiusScaler;
                localSpawnPoint.z = LikesCount * SpawnRadiusScale;
                like.transform.localPosition = localSpawnPoint;
                _likeCountText.text = LikesCount.ToString();
                yield return _waitForSeconds;
            }
        }
    }

    private IEnumerator RemoveLikes(int count) 
    {
        if (_testLike)
        {
            for (int i = 0; i < _likes.Count; i++)
            {
                _likes.RemoveAt(1);
                _testLikeController.transform.localScale = Vector3.one * (1 + (LikesCount * SpawnRadiusScale));
                _likeCountText.text = LikesCount.ToString();
                yield return _waitForSeconds;
            }
            if (LikesCount == 0)
            {
                EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                GameModeHandler.SetState(States.Lose);
            }
        }
        else
        {
            if (count <= _likes.Count)
            {
                for (int i = 0; i < count; i++)
                {
                    _likes[0].gameObject.SetActive(false);
                    _likes[0].transform.SetParent(PooledSkinManager.PooledObjectRoot);
                    _likes.RemoveAt(0);
                    yield return _waitForSeconds;
                    _likeCountText.text = LikesCount.ToString();
                }
                if (LikesCount == 0)
                {
                    EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                    GameModeHandler.SetState(States.Lose);
                }
            }
            else
            {
                for (int i = 0; i < _likes.Count; i++)
                {
                    _likes[0].gameObject.SetActive(false);
                    _likes[0].transform.SetParent(PooledSkinManager.PooledObjectRoot);
                    _likes.RemoveAt(0);
                    _likeCountText.text = LikesCount.ToString();
                    yield return _waitForSeconds;
                }
                EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                GameModeHandler.SetState(States.Lose);
            }
        }

    }

    public void RemoveLike(LikeController likeController) 
    {
        if (!System.Object.ReferenceEquals(likeController, null))
        {
            if (_testLike)
            {
                if (_likes.Contains(likeController))
                {
                    _likes.RemoveAt(1);
                    _testLikeController.transform.localScale = Vector3.one * (1 + (LikesCount * SpawnRadiusScale));
                    _likeCountText.text = LikesCount.ToString();
                    _likeCountText.text = LikesCount.ToString();
                    if (LikesCount == 0)
                    {
                        EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                        GameModeHandler.SetState(States.Lose);
                    }
                }
            }
            else
            {
                if (_likes.Contains(likeController))
                {
                    _likes.Remove(likeController);
                    likeController.transform.SetParent(PooledSkinManager.PooledObjectRoot);
                    likeController.gameObject.SetActive(false);
                    _likeCountText.text = LikesCount.ToString();
                    if (LikesCount == 0)
                    {
                        EventsAgregator.Post<OnEndGameEvent>(this, _onEndGameEvent);
                        GameModeHandler.SetState(States.Lose);
                    }
                }
            }
        }
    }


    public void MoveToBase() 
    {
        if (_testLike)
        {
            _testLikeController.transform.DOMove(transform.position, SecondsToGoBase);
        }
        else
        {
            for (int i = 0; i < _likes.Count; i++)
            {
                spawnRadiusScaler = NormalSpawnRadius + LikesCount * SpawnRadiusScale;
                localSpawnPoint = transform.position + Random.insideUnitSphere * spawnRadiusScaler;
                localSpawnPoint.z = transform.position.z;
                _likes[i].transform.DOMove(localSpawnPoint, SecondsToGoBase);
            }
        }

        DOTween.Sequence().AppendInterval(SecondsToGoBase).OnComplete(() => { GameModeHandler.SetState(States.Run); });
    }

    public void RemoveLike(int count) 
    {
        StartCoroutine(RemoveLikes(count));
    }


    public void SendToFight(DislikeContainer dislikeContainer) 
    {
        var averagePoint = Vector3.Lerp(transform.position, dislikeContainer.transform.position, HalfWay);
        dislikeContainer.SendToFight(averagePoint);

        if (_testLike)
        {
            _testLikeController.transform.DOMove(averagePoint, SecondsToGoFight);
        }
        else
        {
            for (int i = 0; i < LikesCount; i++)
            {
                _likes[i].transform.DOMove(averagePoint, SecondsToGoFight);
            }
        }

    }

}