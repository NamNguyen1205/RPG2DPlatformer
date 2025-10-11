using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    private UI ui;
    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager{ get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health{ get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }


    #region Stat Variable
    public Player_idleState idleState { get; private set; }
    public Player_moveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }

    #endregion

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;
    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;

    [Header("Movement Details")]
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;

    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;

    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;
    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        health = GetComponent<Entity_Health>();

        input = new PlayerInputSet();

        idleState = new Player_idleState(this, stateMachine, "idle");
        moveState = new Player_moveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        swordThrowState = new Player_SwordThrowState(this, stateMachine, "swordThrow");
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, "jumpFall");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;

        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        jumpAttackVelocity = jumpAttackVelocity * speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();

    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}
