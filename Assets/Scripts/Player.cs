using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput), typeof(Animator), typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToDig;
    [SerializeField] private float minDistanceToCharacter;
    [SerializeField] private Pot pot;
    [SerializeField] private List<Character> characters;

    public Transform rootPickupAnchor;
    public static UnityAction<Character> OnRootGiven;

    protected Vector2 moveInput;
    protected Root pickedRoot;
    protected PlayerInput playerInput;
    protected InputAction move;
    protected InputAction interact;

    protected PlayerState state = PlayerState.FreeMove;
    protected Animator animator;

    [HideInInspector] public Ridge closestRidge;
    private Rigidbody2D rb;
    private Character activeCharacter;

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
            if (rb.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rb.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            rb.velocity = new Vector2(moveInput.x, moveInput.y) * speed;
        }
    }

    public void OnPlayerMove() => moveInput = move.ReadValue<Vector2>();

    public void OnInterract() => interact.started += Interract;

    protected virtual void Interract(InputAction.CallbackContext ctx)
    {
        if (!pickedRoot)
        {
            state = PlayerState.FreeMove;
        }
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
            if (IsCharacterClose())
            {
                if (activeCharacter.requiredRootType == pickedRoot.rootType || pickedRoot.rootType == Root.RootType.Buryak)
                {
                    OnRootGiven?.Invoke(activeCharacter);
                    pickedRoot.JumpToPot(pot.RootTargetTransform.position);
                    state = PlayerState.FreeMove;
                }
            }
        }
    }

    private Character IsCharacterClose()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.StoryMode)
        {
            foreach (var character in characters)
            {
                if (Vector2.Distance(transform.position, character.transform.position) <= minDistanceToCharacter)
                {
                    return activeCharacter = character;
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, characters[0].transform.position) <= minDistanceToCharacter)
            {
                return activeCharacter = characters[0];
            }
        }
        return null;
    }

    public void FinishDigging()
    {
        pickedRoot.JumpToPlayer(this);
    }

    public void SetState(PlayerState state)
    {
        this.state = state;
    }

    public enum PlayerState
    {
        FreeMove,
        Carrying,
        Digging
    }
}
