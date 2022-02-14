using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.EventSystems;
using System;

public class UIControl : MonoBehaviour, IDragHandler
{
    private const float SpeedScale = 0.05f;
    private const float NullSpeed = 0f;

    public float DeltaX { get; private set; }

    private void Awake()
    {
        ServiceLocator.Register<UIControl>(this);       
    }


    public void OnDrag(PointerEventData eventData)
    {
        DeltaX = eventData.delta.x * SpeedScale;       
    }


    private void FixedUpdate()
    {
        DeltaX = NullSpeed;
    }

    private void OnDestroy()
    {
        ServiceLocator.Ungerister<UIControl>();
    }
}
