using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public interface ILike
    {
        LikeType type { get; }
        void Init(LikeType type, LikeSkin skin);
    }

    public interface ILikeFactory
    {
        ILike CreateLike();
        ILike CreateDislike();
    }
}

