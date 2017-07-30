using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : LevelObject
{
    [SerializeField] protected Sprite[] _wallSprites;

    public override void J_Start(params object[] p_args)
    {
        base.J_Start(p_args);

        SpriteRenderer __spriteRenderer = GetComponent<SpriteRenderer>();

        __spriteRenderer.sprite = _wallSprites[UnityEngine.Random.Range(0, _wallSprites.Length - 1)];

        __spriteRenderer.flipX = (bool)p_args[0];
        __spriteRenderer.flipY = (bool)p_args[1];

    }
    public override void J_Update()
    {
        base.J_Update();
    }

}
