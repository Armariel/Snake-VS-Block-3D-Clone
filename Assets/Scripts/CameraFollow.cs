using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player Player;

    void LateUpdate()
    {
        if (Player != null)
            transform.position = new Vector3(0.0f, 5.5f, Player.transform.position.z - 3f);
    }
}
