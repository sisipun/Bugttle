using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    protected GameManager game;

    public void Init(GameManager game)
    {
        this.game = game;
    }

    public abstract void handleInput();
}
