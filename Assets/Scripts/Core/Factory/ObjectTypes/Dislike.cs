using System;
using UnityEngine;

namespace Factory
{
    public class Dislike : LikeBase
    {
        private DislikeController _dislikeController;
        public DislikeController Controller => _dislikeController;

        public Dislike(LikeType type, LikeSkin skin) : base(type, skin)
        {
        }

        public override LikeType type => LikeType.Dislike;

        public override void Init(LikeType type, LikeSkin skin)
        {
            gameObject = PooledSkinManager.GetLikeSkinPrefab(type, skin);

            if (gameObject.TryGetComponent(out DislikeController dislikeController))
            {
                GameObject.Destroy(dislikeController);
                _dislikeController = gameObject.AddComponent<DislikeController>();
            }
            else
            {
                _dislikeController = gameObject.AddComponent<DislikeController>();
            }
        }
    }


}