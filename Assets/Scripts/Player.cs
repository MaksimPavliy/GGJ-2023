using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public abstract class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToDig;

    protected Vector2 moveInput;
    protected Root pickedRoot;
    protected PlayerInput playerInput;
    protected InputAction move;
    protected InputAction interract;
    protected BoxCollider2D collider;

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
        rb.velocity = new Vector2(moveInput.x, moveInput.y) * speed;
    } 

    public void OnPlayerMove() => moveInput = move.ReadValue<Vector2>();

    public void OnInterract() => interract.started += Interract;

    protected virtual void Interract(InputAction.CallbackContext ctx)
    {
        /*if (!pickedRoot)
        {*/
            if (closestRidge && closestRidge.root && Vector2.Distance(transform.position, closestRidge.transform.position) <= minDistanceToDig)
            {
                state = PlayerState.Digging;
                pickedRoot = closestRidge.root;
                /*closestRidge.root = null;*/
                Debug.Log(pickedRoot);
            }
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ridge")
        {
            closestRidge = other.gameObject.GetComponent<Ridge>();
            Debug.Log(Vector2.Distance(transform.position, closestRidge.transform.position));
        }
        
    }

    public enum PlayerState
    {
        FreeMove,
        Digging
    }
}
