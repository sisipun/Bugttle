using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiController", menuName = "Scriptable Objects/Controllers/Ai Controller")]
public class AiController : BaseController
{
    public override void handleInput()
    {
        game.EndTurn();
    }
}
