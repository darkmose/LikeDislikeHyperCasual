using System.Collections.Generic;
using UnityEngine;
using System;
using GameEvents;
using DG.Tweening;

public class LikeGroupController : MonoBehaviour
{
    #region CONST FIELDS
    private const float PositionBorderOffset = 4.8f;
    #endregion

    #region FIELDS
    [SerializeField] private LikesContainer _likesContainer;
    private UIControl _uIControl;

    private Dictionary<States, System.Action> _actions = new Dictionary<States, Action>();
    private System.Action _currentAction;

    private Vector3 _movementOffset = Vector3.zero;
    private Vector3 _newPosition = Vector3.zero;
    private float _rightMovementBorder;
    private float _leftMovementBorder;
    #endregion

    #region Awake/Start

    private void Awake()
    {
        _actions.Add(States.Run, MoveAction);
    }

    private void Start()
    {
        _uIControl = ServiceLocator.Resolve<UIControl>();
        _leftMovementBorder = transform.position.x - PositionBorderOffset;
        _rightMovementBorder = transform.position.x + PositionBorderOffset;
        _likesContainer.CreateLike();
        EventsAgregator.Subscribe<OnGameModeChangedEvent>(OnGameModeChangedHandler);
        GameModeHandler.SetState(States.Entry);
    }
    #endregion

    #region ACTIONS

    private void DefaultNullAction()
    {
        //Do nothing
    }

    private void MoveAction()
    {
        _movementOffset.z = Time.deltaTime * Constants.PlayerSpeed;
        _movementOffset.x = _uIControl.DeltaX;
        _newPosition = transform.position + _movementOffset;
        _newPosition.x = Mathf.Clamp(_newPosition.x, _leftMovementBorder, _rightMovementBorder);
        transform.position = _newPosition;
    }

    #endregion

    #region EVENT BASED
    private void OnGameModeChangedHandler(object sender, OnGameModeChangedEvent data)
    {
        if (_actions.ContainsKey(GameModeHandler.CurrentState))
        {
            _currentAction = _actions[GameModeHandler.CurrentState];
        }
        else
        {
            _currentAction = DefaultNullAction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.EnemyTagName))
        {
            GameModeHandler.SetState(States.Fight);
            if (other.TryGetComponent(out DislikeContainer dislikeContainer))
            {
                _likesContainer.SendToFight(dislikeContainer);
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
                                    _likesContainer.CreateLike(10);
                                }
                                break;
                            case Booster.LikeBoosters.Plus20:
                                {
                                    _likesContainer.CreateLike(20);
                                }
                                break;
                            case Booster.LikeBoosters.Plus30:
                                {
                                    _likesContainer.CreateLike(30);
                                }
                                break;
                            case Booster.LikeBoosters.Plus40:
                                {
                                    _likesContainer.CreateLike(40);
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
                                    _likesContainer.CreateLike(count);
                                }
                                break;
                            case Booster.LikeBoosters.x3:
                                {
                                    var count = _likesContainer.LikesCount * 2;
                                    _likesContainer.CreateLike(count);
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