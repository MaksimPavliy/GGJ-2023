using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cat : Player
{
    void Start()
    {
        move = playerInput.actions["MoveFirstPlayer"];
        interact = playerInput.actions["InteractFirstPlayer"];
    }

    protected override void Interract(InputAction.CallbackContext ctx)
    {
        base.Interract(ctx);

    }
}
