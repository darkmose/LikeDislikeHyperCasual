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
                var dContainer = dislikeController.transform.parent.GetComponent<DislikeContainer>();
                var lContainer = transform.parent.GetComponent<LikesContainer>();
                _onLikeEnterEnemyEvent.likeControllerCount = lContainer.LikesCount;
                _onLikeEnterEnemyEvent.dislikeControllerCount = dContainer.DislikesCount;
                EventsAgregator.Post<OnLikeEnterEnemyEvent>(this, _onLikeEnterEnemyEvent);
            }

        }
    }
}
