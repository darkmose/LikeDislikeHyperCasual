using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameEvents;

public class PhotoCard : MonoBehaviour
{
    [SerializeField] private Image _photoFrame;
    [SerializeField] private Sprite _photo;
    [SerializeField] private Text _likesCountText;
    [SerializeField] private GameObject _platform;
    [SerializeField] private Text _platformMultiplierTextLeft;
    [SerializeField] private Text _platformMultiplierTextRight;
    [SerializeField] private float _pointsMultiplier;
    private OnLikePhotoEvent _onLikePhotoEvent = new OnLikePhotoEvent();
    private int _likesCount = 0;
    private int _intenseLightLayer;
    private int _defaultLayer;
    private int LikesCount
    {
        get
        {
            return _likesCount;
        }
        set 
        {
            _likesCount = value;
            _likesCountText.text = value.ToString();
        }
    }
    private bool _isIntenseLight;

    private void Awake()
    {
        _intenseLightLayer = LayerMask.NameToLayer(Constants.IntenseLightLayerName);
        _defaultLayer = LayerMask.NameToLayer(Constants.DefaultLayerName);
        _photoFrame.sprite = _photo;
        _likesCountText.text = _likesCount.ToString();
        _platformMultiplierTextLeft.text = "x" + _pointsMultiplier.ToString("0.0");
        _platformMultiplierTextRight.text = "x" + _pointsMultiplier.ToString("0.0");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.LikeTagName))
        {
            LikesCount++;
            EventsAgregator.Post<OnLikePhotoEvent>(this, _onLikePhotoEvent);
            if (!_isIntenseLight)
            {
                PhotoEnter();
                DOTween.Sequence().AppendInterval(Constants.SecondsToLightPlatform).OnComplete(PhotoExit);
            }
        }
    }

    private void PhotoEnter() 
    {
        _platform.layer = _intenseLightLayer;
        _isIntenseLight = true;
    }

    private void PhotoExit() 
    {
        _platform.layer = _defaultLayer;
        _isIntenseLight = false;
        Debug.Log("PhotoExit");
    }


}
