using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _moveOffset = Vector3.zero;
    private void FixedUpdate()
    {
        if (GameModeHandler.CurrentState == States.Run)
        {
            _moveOffset.z = Time.fixedDeltaTime * Constants.PlayerSpeed;
            transform.position += _moveOffset;
        }
    }
}
