using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]
public abstract class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToDig;

    public static UnityAction<Player, Root> OnRootDigged;

    protected Vector2 moveInput;
    protected Root pickedRoot;
    protected PlayerInput playerInput;
    protected InputAction move;
    protected InputAction interract;

    protected PlayerState state = PlayerState.FreeMove;
    protected Animator animator;

    private Ridge closestRidge;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Debug.Log(state);

        if (state != PlayerState.Digging)
        {
            rb.velocity = new Vector2(moveInput.x, moveInput.y) * speed;
        }
    } 

    public void OnPlayerMove() => moveInput = move.ReadValue<Vector2>();

    public void OnInterract() => interract.started += Interract;

    protected virtual void Interract(InputAction.CallbackContext ctx)
    {
        if (state == PlayerState.FreeMove)
        {
            if (closestRidge && closestRidge.root && Vector2.Distance(transform.position, closestRidge.transform.position) <= minDistanceToDig)
            {
                state = PlayerState.Digging;
                //animator.Play("Dig");
                pickedRoot = closestRidge.root;
                closestRidge.root = null;
                closestRidge.isEmpty = true;

                FinishDigging();
            }
        }
    }

    private void FinishDigging()
    {
        OnRootDigged?.Invoke(this, pickedRoot);
    }

    public void SetState(PlayerState state)
    {
        this.state = state;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ridge")
        {
            closestRidge = other.gameObject.GetComponent<Ridge>();
        }
    }

    public enum PlayerState
    {
        FreeMove,
        Carrying,
        Digging
    }
}
