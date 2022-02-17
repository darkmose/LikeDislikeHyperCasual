using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;

public class PooledSkinManager : MonoBehaviour
{
    private static PooledSkinManager _innerInstance;
    private ObjectPooler _objectPooler;
    public static Transform PooledObjectRoot => _innerInstance.transform;

    [SerializeField] private List<SkinDescriptor> _skins;

    private void Awake()
    {
        if (_innerInstance == null)
        {
            _innerInstance = this;
        }
        PrepareObjectPooler();
    }

    private void PrepareObjectPooler()
    {
        _objectPooler = new ObjectPooler(this.transform, true, Constants.DefaultDynamicExtendCount);

        for (int i = 0; i < _skins.Count; i++)
        {
            var name = _skins[i].LikeSkin.ToString() + _skins[i].LikeType.ToString();
            _objectPooler.CreatePool(name, _skins[i].Prefab, _skins[i].count);
        }
    }

    public static void ReturnAllObjectsInPool()
    {
        _innerInstance._objectPooler.ReturnObjects();
    }

    public static GameObject GetLikeSkinPrefab(LikeType type, LikeSkin skin)
    {
        var skinTag = skin.ToString() + type.ToString();
        var newObject = _innerInstance._objectPooler.GetPooledGameObject(skinTag);
        return newObject;
    }


    [System.Serializable]
    private class SkinDescriptor 
    {
        public string nameDescriptor;

        public LikeType LikeType;

        public LikeSkin LikeSkin;

        public GameObject Prefab;

        [Tooltip("Initial count of pooled objects")]
        public int count;
    }
}