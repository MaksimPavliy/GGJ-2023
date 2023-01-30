using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cat : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(moveInput.x, moveInput.y) * speed * Time.deltaTime);
    }

    public void OnCatMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
}
