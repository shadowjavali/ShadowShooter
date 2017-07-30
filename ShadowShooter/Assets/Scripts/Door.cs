using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum DoorAnimationEvents
    {
      
    }

    public override void J_Start(params object[] p_args)
    {
        base.J_Start(p_args);

        _spriteRendererFrame.sprite = _doorSpritesFrame[UnityEngine.Random.Range(0, _doorSpritesFrame.Length - 1)];
        _spriteRendererRight.sprite = _doorSpritesRight[UnityEngine.Random.Range(0, _doorSpritesRight.Length - 1)];
        _spriteRendererLeft.sprite = _doorSpritesLeft[UnityEngine.Random.Range(0, _doorSpritesLeft.Length - 1)];

        _spriteRendererFrame.flipX = (bool)p_args[0];
        _spriteRendererFrame.flipY = (bool)p_args[1];

        _spriteRendererRight.flipX = (bool)p_args[0];
        _spriteRendererRight.flipY = (bool)p_args[1];

        _spriteRendererLeft.flipX = (bool)p_args[0];
        _spriteRendererLeft.flipY = (bool)p_args[1];

        base.J_Start(p_args);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _animator.SetTrigger("Open");
            _collider.enabled = false;
        }       
    }


    public void AnimationCalledEvent(DoorAnimationEvents p_event)
    {

    }

}
