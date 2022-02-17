using System.Collections.Generic;
using UnityEngine;
using System;
using GameEvents;
using DG.Tweening;

public class LikeGroupController : MonoBehaviour
{
    #region CONST FIELDS
    private const float PositionBorderOffset = 3.5f;
    #endregion

    #region FIELDS
    [SerializeField] private LikesContainer _likesContainer;
    private UIControl _uIControl;

    private Dictionary<States, System.Action> _actions = new Dictionary<States, System.Action>();
    private System.Action _currentAction;

    private Vector3 _movementOffset = Vector3.zero;
    private Vector3 _newPosition = Vector3.zero;
    private float _startPosX;
    private float _rightMovementBorder;
    private float _leftMovementBorder;
    private float _speedAcceleration = 1f;
    private DislikeContainer _dislikeContainer;
    #endregion

    #region Awake/Start

    private void Awake()
    {
        _actions.Add(States.Run, RunAction);
        _actions.Add(States.Finish, FinishAction);
    }

    private void Start()
    {
        _uIControl = ServiceLocator.Resolve<UIControl>();
        _startPosX = transform.position.x; 
        _leftMovementBorder = transform.position.x - PositionBorderOffset;
        _rightMovementBorder = transform.position.x + PositionBorderOffset;
        _likesContainer.CreateFirstLike();
        EventsAgregator.Subscribe<OnGameStateChangedEvent>(OnGameModeChangedHandler);
        GameStatesHandler.SetState(States.Entry);
    }
    #endregion

    #region ACTIONS

    private void DefaultNullAction()
    {
        //Do nothing
    }

    private void FinishAction() 
    {
        _movementOffset.z = Time.deltaTime * Constants.PlayerFinishSpeed * _speedAcceleration;
        _newPosition = transform.position + _movementOffset;
        _newPosition.x = Mathf.Clamp(_newPosition.x, _leftMovementBorder, _rightMovementBorder);
        transform.position = _newPosition;
        _speedAcceleration += Constants.PlayerSpeedAcceleration;
    }

    private void RunAction()
    {
        _movementOffset.z = Time.deltaTime * Constants.PlayerSpeed;
        _movementOffset.x = _uIControl.DeltaX;
        _newPosition = transform.position + _movementOffset;
        _newPosition.x = Mathf.Clamp(_newPosition.x, _leftMovementBorder, _rightMovementBorder);
        transform.position = _newPosition;
    }

    #endregion

    #region EVENT BASED
    private void OnGameModeChangedHandler(object sender, OnGameStateChangedEvent data)
    {
        switch (GameStatesHandler.CurrentState) //Entry Point
        {
            case States.Separate:
                var pos = transform.position;
                pos.x = _startPosX;
                transform.position = pos;
                pos = Camera.main.transform.position;
                pos.x = _startPosX;
                Camera.main.transform.position = pos;
                _likesContainer.SeparateMainLike();
                break;
            case States.Finish:

                break;
        }

        if (_actions.ContainsKey(GameStatesHandler.CurrentState))
        {
            _currentAction = _actions[GameStatesHandler.CurrentState];
        }
        else
        {
            _currentAction = DefaultNullAction;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.WinTagName))
        {
            StopAllCoroutines();
            GameStatesHandler.SetState(States.Win);
        }
        if (other.CompareTag(Constants.DislikeTagName))
        {
            var likeCount = _likesContainer.LikesCount;
            var dislikeCount = _dislikeContainer.DislikesCount;
            _dislikeContainer.RemoveDislikes(likeCount);
            _likesContainer.RemoveLike(dislikeCount);
        }
        if (other.CompareTag(Constants.EnemyTagName))
        {
            GameStatesHandler.SetState(States.Fight);
            if (other.TryGetComponent(out DislikeContainer dislikeContainer))
            {
                var point = other.ClosestPoint(transform.position);
                _dislikeContainer = dislikeContainer;

                _dislikeContainer.SendToFight(point);
                _likesContainer.MoveToFight(point);
            }
        }
        if (other.CompareTag(Constants.GateTagName))
        {
            if (other.TryGetComponent(out Gate gate))
            {
                gate.RemoveField();

                switch (gate.booster.gateType)
                {
                    case Booster.GateType.LikeGate:
                        switch (gate.booster.likeBooster)
                        {
                            case Booster.LikeBoosters.Plus10:
                                { 
                                    _likesContainer.CreateLikes(10);
                                }
                                break;
                            case Booster.LikeBoosters.Plus20:
                                {
                                    _likesContainer.CreateLikes(20);
                                }
                                break;
                            case Booster.LikeBoosters.Plus30:
                                {
                                    _likesContainer.CreateLikes(30);
                                }
                                break;
                            case Booster.LikeBoosters.Plus40:
                                {
                                    _likesContainer.CreateLikes(40);
                                }
                                break;
                            case Booster.LikeBoosters.x1:
                                {
                                    //Do nothing
                                }
                                break;
                            case Booster.LikeBoosters.x2:
                                {
                                    var count = _likesContainer.LikesCount;
                                    _likesContainer.CreateLikes(count);
                                }
                                break;
                            case Booster.LikeBoosters.x3:
                                {
                                    var count = _likesContainer.LikesCount * 2;
                                    _likesContainer.CreateLikes(count);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case Booster.GateType.DislikeGate:
                        switch (gate.booster.dislikeBooster)
                        {
                            case Booster.DislikeBoosters.Minus10:
                                {
                                    _likesContainer.RemoveLike(10);
                                }
                                break;
                            case Booster.DislikeBoosters.Minus20:
                                {
                                    _likesContainer.RemoveLike(20);
                                }
                                break;
                            case Booster.DislikeBoosters.Minus30:
                                {
                                    _likesContainer.RemoveLike(30);
                                }
                                break;
                            case Booster.DislikeBoosters.d1:
                                {
                                    //Do nothing
                                }
                                break;
                            case Booster.DislikeBoosters.d2:
                                {
                                    var count = _likesContainer.LikesCount / 2;
                                    _likesContainer.RemoveLike(count);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion

    #region UPDATES
    private void FixedUpdate()
    {
        _currentAction?.Invoke();
    }
    #endregion
}