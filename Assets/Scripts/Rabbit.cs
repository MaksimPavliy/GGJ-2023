using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rabbit : Player
{
    private void Start()
    {
        if (GameManager.instance.networkMode == GameManager.NetworkMode.Local)
        {
            move = playerInput.actions["MoveSecondPlayer"];
            interact = playerInput.actions["InteractSecondPlayer"];
        }
        else if (GameManager.instance.networkMode == GameManager.NetworkMode.Multiplayer)
        {
            move = playerInput.actions["MoveFirstPlayer"];
            interact = playerInput.actions["InteractFirstPlayer"];
        }
    }

    protected override void Interract(InputAction.CallbackContext ctx)
    {
        base.Interract(ctx);

    }
}