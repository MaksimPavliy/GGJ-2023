using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rabbit : Player
{
    void Start()
    {
        if (GameManager.instance.isLocalGame)
        {
            move = playerInput.actions["MoveLocal"];
            interact = playerInput.actions["InteractLocal"];
        }
        else
        {
            move = playerInput.actions["MoveMultiplayer"];
            interact = playerInput.actions["InteractMultiplayer"];
        }
    }

    protected override void Interract(InputAction.CallbackContext ctx)
    {
        base.Interract(ctx);


    }
}