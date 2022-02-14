using System;
using UnityEngine;

namespace Factory
{
    public class Like : LikeBase
    {
        private LikeController _likeController;
        public LikeController Controller => _likeController;
        
        public Like(LikeType type, LikeSkin skin) : base(type, skin)
        {
        }

        public override LikeType type => LikeType.Like;

        public override void Init(LikeType type, LikeSkin skin)
        {
            gameObject = PooledSkinManager.GetLikeSkinPrefab(type, skin);

            if (gameObject.TryGetComponent(out LikeController likeController))
            {
                GameObject.Destroy(likeController);
                _likeController = gameObject.AddComponent<LikeController>();
            }
            else
            {
                _likeController = gameObject.AddComponent<LikeController>();
            }
        }
    }


}