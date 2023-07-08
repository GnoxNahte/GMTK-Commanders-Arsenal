using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControls : MonoBehaviour
{
    public enum AttackType
    {
        NotAttacking,
        Attack1, // Combo attack
        Spin,
        JumpFwd,
        Cast1,
        Cast2,
    }

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private bool overrideControls;

    [SerializeField]
    [ReadOnly]
    private Vector2 moveDir;

    [SerializeField]
    [ReadOnly]
    private AttackType currAttackType;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    // Animation id / hash
    int animId_MoveSpd;
    int animId_AttackType;
    int animId_IsDead;

    #region Public Methods
    public void SetMoveDir(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    public void SetAttack(AttackType type)
    {
        currAttackType = type;
        animator.SetInteger(animId_AttackType, (int)currAttackType);
    }

    public void OnHeroDead()
    {
        animator.SetBool(animId_IsDead, true);
    }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        currAttackType = AttackType.NotAttacking;

        // Caching
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        animId_MoveSpd = Animator.StringToHash("MoveSpd");
        animId_AttackType = Animator.StringToHash("AttackType");
        animId_IsDead = Animator.StringToHash("IsDead");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (overrideControls && input.sqrMagnitude > 0)
        {
            moveDir = input;
        }

        sr.flipX = moveDir.x < 0;

        // Update animation
        animator.SetFloat(animId_MoveSpd, moveDir.magnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion
}
