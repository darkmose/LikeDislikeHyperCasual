using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraLinker : MonoBehaviour
{
    [SerializeField] private Canvas _targetCanvas;
    private void Awake()
    {
        _targetCanvas.worldCamera = Camera.main;
    }
}
