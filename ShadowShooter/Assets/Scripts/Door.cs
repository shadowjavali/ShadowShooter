using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : LevelObject
{
    [SerializeField] protected Sprite[] _doorSpritesFrame;
    [SerializeField] protected Sprite[] _doorSpritesLeft;
    [SerializeField] protected Sprite[] _doorSpritesRight;

    [SerializeField] protected SpriteRenderer _spriteRendererFrame;
    [SerializeField] protected SpriteRenderer _spriteRendererLeft;
    [SerializeField] protected SpriteRenderer _spriteRendererRight;

    [SerializeField] protected Animator _animator;
    [SerializeField] protected Collider2D _collider;

    private SpawningAreaManager.DoorPosition _doorType;

    private SpawningAreaManager _myGrid;
    private Vector2 _gridItOpens;

    private bool _open = false;

    public Action<SpawningAreaManager.GameAreaType, Vector2, SpawningAreaManager.DoorPosition> onSpawnArea;

    public enum DoorAnimationEvents
    {
      
    }

    public bool GetDoorState()
    {
        return _open;
    }

    public override void J_Start(params object[] p_args)
    {
        base.J_Start(p_args);

        _open = false;

        _spriteRendererFrame.sprite = _doorSpritesFrame[UnityEngine.Random.Range(0, _doorSpritesFrame.Length - 1)];
        _spriteRendererRight.sprite = _doorSpritesRight[UnityEngine.Random.Range(0, _doorSpritesRight.Length - 1)];
        _spriteRendererLeft.sprite = _doorSpritesLeft[UnityEngine.Random.Range(0, _doorSpritesLeft.Length - 1)];

        _spriteRendererFrame.flipX = (bool)p_args[0];
        _spriteRendererFrame.flipY = (bool)p_args[1];

        _spriteRendererRight.flipX = (bool)p_args[0];
        _spriteRendererRight.flipY = (bool)p_args[1];

        _spriteRendererLeft.flipX = (bool)p_args[0];
        _spriteRendererLeft.flipY = (bool)p_args[1];


        _myGrid = (SpawningAreaManager)p_args[2];

        _doorType = (SpawningAreaManager.DoorPosition)p_args[3];

        switch(_doorType)
        {
            case SpawningAreaManager.DoorPosition.LEFT:
                _gridItOpens = new Vector2(_myGrid.GetGridPos(). x - 1, _myGrid.GetGridPos().y);
            break;
            case SpawningAreaManager.DoorPosition.RIGHT:
                _gridItOpens = new Vector2(_myGrid.GetGridPos().x + 1, _myGrid.GetGridPos().y);
                break;
            case SpawningAreaManager.DoorPosition.UP:
                _gridItOpens = new Vector2(_myGrid.GetGridPos().x, _myGrid.GetGridPos().y +1);
                break;
            case SpawningAreaManager.DoorPosition.DOWN:
                _gridItOpens = new Vector2(_myGrid.GetGridPos().x , _myGrid.GetGridPos().y -1);
                break;

        }


        base.J_Start(p_args);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_open == false)
        {
            if (collision.transform.tag == "Player")
            {
                J_OpenDoor();
                onSpawnArea(SpawningAreaManager.GameAreaType.AREA_TYPE_0, _gridItOpens, _doorType);
            }
        }
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().SetCurrentGrid(_myGrid);
        }
        else
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().SetCurrentGrid(_myGrid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       // if (_open == false)
      //  {
       

    }

    public void J_OpenDoor()
    {
        _animator.SetTrigger("Open");
        _open = true;
      //  _collider.enabled = false;      
    }

    private void CreateArea()
    {


    }

    public void AnimationCalledEvent(DoorAnimationEvents p_event)
    {

    }

}
