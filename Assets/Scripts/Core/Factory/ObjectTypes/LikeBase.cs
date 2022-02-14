using UnityEngine;

namespace Factory
{
    public abstract class LikeBase : ILike
    {
        protected LikeBase(LikeType type, LikeSkin skin)
        {
            Init(type, skin);
        }
        public abstract LikeType type { get; }
        public GameObject gameObject;
        public abstract void Init(LikeType type, LikeSkin skin);
    }


}