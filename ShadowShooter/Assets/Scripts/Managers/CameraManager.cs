using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : LevelObject
{
    private Transform _playerTransform;

    public void SetPlayerToFollow(Transform p_player)
    {
        _playerTransform = p_player;

    }
    public override void J_Update()
    {
        base.J_Update();
        transform.position = new Vector3(_playerTransform.transform.position.x, _playerTransform.transform.position.y, -10);
    }

}
