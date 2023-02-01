using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput), typeof(Animator), typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToDig;
    [SerializeField] private float minDistanceToPot;
    [SerializeField] private Pot pot;

    protected Vector2 moveInput;
    protected Root pickedRoot;
    protected PlayerInput playerInput;
    protected InputAction move;
    protected InputAction interact;

    protected PlayerState state = PlayerState.FreeMove;
    protected Animator animator;

    private Ridge closestRidge;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (state != PlayerState.Digging)
        {
            rb.MovePosition(rb.position + moveInput * speed * Time.deltaTime);
        }
    } 

    public void OnPlayerMove() => moveInput = move.ReadValue<Vector2>();

    public void OnInterract() => interact.started += Interract;

    protected virtual void Interract(InputAction.CallbackContext ctx)
    {
        if (state == PlayerState.FreeMove)
        {
            if (closestRidge && closestRidge.CanBeDigged() && Vector2.Distance(transform.position, closestRidge.transform.position) <= minDistanceToDig)
            {
                state = PlayerState.Digging;
                rb.velocity = Vector2.zero;
                //animator.Play("Dig");
                pickedRoot = closestRidge.root;
                closestRidge.root = null;
                closestRidge.isEmpty = true;

                FinishDigging();
            }
        }
        if (state == PlayerState.Carrying)
        {
            if(Vector2.Distance(transform.position, pot.transform.position) <= minDistanceToPot && pot.rootInRecipe(pickedRoot.rootType))
            {
                state = PlayerState.FreeMove;
                pickedRoot.JumpToPot(pot.RootTargetTransform.position);
            }
        }
    }

    public void FinishDigging()
    {
        pickedRoot.JumpToPlayer(this);
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
