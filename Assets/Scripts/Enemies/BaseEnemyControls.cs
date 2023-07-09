using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to HeroControls.cs but customised for enemies
public class BaseEnemyControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField] [ReadOnly]
    private Vector2 moveDir;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer sr;

    // Animation id / hash
    int animId_IsDead;

    #region Public Methods
    public void SetMoveDir(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    public void OnEnemyDead()
    {
        animator.SetBool(animId_IsDead, true);
    }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    virtual protected void Start()
    {
        // Caching
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        animId_IsDead = Animator.StringToHash("IsDead");
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        sr.flipX = HeroAI.instance.transform.position.x < transform.position.x;
    }

    virtual protected void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion
}
