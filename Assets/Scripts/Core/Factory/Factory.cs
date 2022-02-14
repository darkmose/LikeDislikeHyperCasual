using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public enum LikeType { Like, Dislike }
    public enum LikeSkin { Common }

    public class CommonFactory : ILikeFactory
    {
        public ILike CreateLike()
        {
            var like = new Like(LikeType.Like, LikeSkin.Common);
            return like;
        }

        public ILike CreateDislike()
        {
            var dislike = new Dislike(LikeType.Dislike, LikeSkin.Common);
            return dislike;
        }

    }
    
    public static class AbstractFactory 
    {
        private static CommonFactory _commonFactory = new CommonFactory();

        public static LikeController CreateLikeController(LikeSkin skin) 
        {
            Like like = default;

            switch (skin)
            {
                case LikeSkin.Common:
                    like = (Like)_commonFactory?.CreateLike();
                    break;
                default:
                    break;
            }
            return like.Controller;
        }

        public static DislikeController CreateDislikeController(LikeSkin skin)
        {
            Dislike dislike = default;
            switch (skin)
            {
                case LikeSkin.Common:
                    dislike = (Dislike)_commonFactory?.CreateDislike();
                    break;
                default:
                    break;
            }
            return dislike.Controller;
        }



    }
}