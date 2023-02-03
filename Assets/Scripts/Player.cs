using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Events;
using Spine.Unity;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
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

    protected PlayerState state = PlayerState.Idle;

    [HideInInspector] public Ridge closestRidge;
    private Rigidbody2D rb;
    private Character activeCharacter;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState skeletonAnimationState;
    [SerializeField] private AnimationReferenceAsset idle, walk, dig, attack;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimationState = skeletonAnimation.AnimationState;
    }

    private void Update()
    {
        if (state != PlayerState.Digging)
        {
            if (skeletonAnimation.AnimationName != walk.name && rb.velocity != Vector2.zero)
            {
                skeletonAnimationState.SetAnimation(0, walk, true);
            }
            else if (skeletonAnimation.AnimationName != idle.name && rb.velocity == Vector2.zero)
            {
                skeletonAnimationState.SetAnimation(0, idle, true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (state != PlayerState.Digging)
        {
            if (moveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (moveInput.x > 0)
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
        if (state == PlayerState.Moving || state == PlayerState.Idle)
        {
            if (!pickedRoot)
            {
                if (closestRidge && closestRidge.CanBeDigged() && Vector2.Distance(transform.position, closestRidge.transform.position) <= minDistanceToDig)
                {
                    state = PlayerState.Digging;
                    pickedRoot = closestRidge.root;
                    skeletonAnimationState.SetAnimation(0, dig, false);
                    closestRidge.root = null;
                    closestRidge.isEmpty = true;
                    rb.velocity = Vector2.zero;
                    FinishDigging();
                }
            }
            if (state != PlayerState.Digging && pickedRoot)
            {
                if (IsCharacterClose())
                {
                    if (activeCharacter.requiredRootType == pickedRoot.rootType || pickedRoot.rootType == Root.RootType.Buryak)
                    {
                        StartCoroutine(pickedRoot.JumpToPot(pot.RootTargetTransform.position, activeCharacter));
                        pickedRoot = null;
                        OnRootGiven?.Invoke(activeCharacter);
                    }
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
        StartCoroutine(pickedRoot.JumpToPlayer(this, dig.Animation.Duration));
    }

    public void SetState(PlayerState state)
    {
        this.state = state;
    }

    public enum PlayerState
    {
        Idle,
        Moving,
        Digging,
        Attacking
    }
}
