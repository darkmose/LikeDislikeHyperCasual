using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public sealed class OnLevelProgressChangedEvent
    {
        public float LevelProgress;
    }

    public sealed class OnLevelFinishEvent
    {
    }

    public sealed class OnGameModeChangedEvent
    {
    }


    public sealed class OnLikeEnterEnemyEvent 
    {
        public LikeController likeController;
        public DislikeController dislikeController;
    }

    public sealed class OnEnemyGroupDieEvent 
    {
        public DislikeContainer diedDislikeContainer;
    }

    public sealed class OnEndGameEvent 
    {
        
    }


}
