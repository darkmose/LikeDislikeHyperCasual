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

    private Vector3 localSpawnPoint = Vector3.zero;
    private float spawnRadiusScaler = 1f;
    private List<DislikeController> _dislikes;
    public int DislikesCount => _dislikes.Count;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Constants.CoroutineUltraFastProcess);
    private OnEnemyGroupDieEvent _onEnemyGroupDieEvent = new OnEnemyGroupDieEvent();
    private void Awake()
    {
        EventsAgregator.Subscribe<OnLikeEnterEnemyEvent>(OnLikeEnterEnemyHandler);
        PrepareDislikeList();
        CreateDislike(_dislikeCount);
    }

    private void Start()
    {

    }

    private void OnLikeEnterEnemyHandler(object sender, OnLikeEnterEnemyEvent data)
    {
        RemoveDislike(data.dislikeController);
    }


    private void PrepareDislikeList() 
    {
        _dislikes = new List<DislikeController>();
    }

    private void CreateDislike(int count) 
    {
        for (int i = 0; i < count; i++)
        {
            var dislike = Factory.AbstractFactory.CreateDislikeController(Factory.LikeSkin.Common);
            _dislikes.Add(dislike);
            dislike.transform.SetParent(transform);
            spawnRadiusScaler = NormalSpawnRadius + DislikesCount * SpawnRadiusScale;
            localSpawnPoint = Random.insideUnitSphere * spawnRadiusScaler;
            localSpawnPoint.z = 0;
            dislike.transform.localPosition = localSpawnPoint;
            _dislikeCountText.text = DislikesCount.ToString();
        }
    }

    public void RemoveDislike(DislikeController dislikeController)
    {
        if (!System.Object.ReferenceEquals(dislikeController, null))
        {
            if (_dislikes.Contains(dislikeController))
            {
                _dislikes.Remove(dislikeController);
                dislikeController.transform.SetParent(PooledSkinManager.PooledObjectRoot);
                dislikeController.gameObject.SetActive(false);
                _dislikeCountText.text = DislikesCount.ToString();
                if (DislikesCount == 0)
                {
                    EventsAgregator.Post<OnEnemyGroupDieEvent>(this, _onEnemyGroupDieEvent);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SendToFight(Vector3 fightPoint)
    {
        for (int i = 0; i < DislikesCount; i++)
        {
            _dislikes[i].transform.DOMove(fightPoint, SecondsToGoFight);
        }
    }


}
