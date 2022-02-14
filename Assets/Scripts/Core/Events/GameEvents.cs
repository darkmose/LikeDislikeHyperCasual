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
        public int likeControllerCount;
        public int dislikeControllerCount;
    }

    public sealed class OnEnemyGroupDieEvent 
    {
        public DislikeContainer diedDislikeContainer;
    }

    public sealed class OnEndGameEvent 
    {
        
    }


}
