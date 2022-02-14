using DG.Tweening;
using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DislikeContainer : MonoBehaviour
{
    private const int NormalSpawnRadius = 1;
    private const float SpawnRadiusScale = 0.01f;
    private const int SecondsToGoFight = 1;

    [Tooltip("Count of enemies")]
    [SerializeField] private int _dislikeCount;
    [SerializeField] private Text _dislikeCountText;


    private List<DislikeController> _dislikes;
    public int DislikesCount => _dislikes.Count;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEnemyGroupDieEvent _onEnemyGroupDieEvent = new OnEnemyGroupDieEvent();

    #region TEST
    private DislikeController _testDislikeController;    
    #endregion

    private void Awake()
    {
        EventsAgregator.Subscribe<OnLikeEnterEnemyEvent>(OnLikeEnterEnemyHandler);
        PrepareDislikeList();

        CreateDislike(_dislikeCount);
    }


    private void OnLikeEnterEnemyHandler(object sender, OnLikeEnterEnemyEvent data)
    {
        RemoveDislikes(data.likeControllerCount);
    }


    private void PrepareDislikeList() 
    {
        _dislikes = new List<DislikeController>();
    }



    private void CreateDislike(int count) 
    {
        _testDislikeController = Factory.AbstractFactory.CreateDislikeController(Factory.LikeSkin.Common);
        _dislikes.Add(_testDislikeController);
        _testDislikeController.transform.SetParent(transform);
        _testDislikeController.transform.localPosition = Vector3.zero;
        _testDislikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
        _dislikeCountText.text = DislikesCount.ToString();

        for (int i = 1; i < count; i++)
        {
            var dislike = new DislikeController();
            _dislikes.Add(dislike);
            _testDislikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
            _dislikeCountText.text = DislikesCount.ToString();
        }

    }

    private IEnumerator RemoveDislike(int count) 
    {
        for (int i = 0; i < count; i++)
        {
            if (DislikesCount > 1)
            {
                _dislikes.RemoveAt(1);
                _testDislikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
                _dislikeCountText.text = DislikesCount.ToString();
            }
            else
            {
                _dislikes.Remove(_testDislikeController);
                _testDislikeController.transform.SetParent(PooledSkinManager.PooledObjectRoot);
                _testDislikeController.gameObject.SetActive(false);
                _testDislikeController.transform.localScale = Vector3.one * (NormalSpawnRadius + (DislikesCount * SpawnRadiusScale));
                _dislikeCountText.text = DislikesCount.ToString();

                EventsAgregator.Post<OnEnemyGroupDieEvent>(this, _onEnemyGroupDieEvent);
                this.gameObject.SetActive(false);
            }
            yield return _waitForSeconds;
        }

    }

    public void RemoveDislikes(int count)
    {
        StartCoroutine(RemoveDislike(count));                
    }

    public void SendToFight(Vector3 fightPoint)
    {
        _testDislikeController.transform.DOMove(fightPoint, SecondsToGoFight);
    }


}
