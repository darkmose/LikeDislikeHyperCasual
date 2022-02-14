using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class LikeController : MonoBehaviour
{
    private OnLikeEnterEnemyEvent _onLikeEnterEnemyEvent = new OnLikeEnterEnemyEvent();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.DislikeTagName))
        {
            if(other.TryGetComponent(out DislikeController dislikeController))
            {
                _onLikeEnterEnemyEvent.likeController = this;
                _onLikeEnterEnemyEvent.dislikeController = dislikeController;
                EventsAgregator.Post<OnLikeEnterEnemyEvent>(this, _onLikeEnterEnemyEvent);
            }

        }
    }
}
