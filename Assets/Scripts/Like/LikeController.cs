using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class LikeController : MonoBehaviour
{
    private const string SeparateTriggerName = "Separate";
    private Animator _animator;

    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }
    }

    public void PlaySeparateAnimation() 
    {
        _animator.SetTrigger(SeparateTriggerName);
    }
}
