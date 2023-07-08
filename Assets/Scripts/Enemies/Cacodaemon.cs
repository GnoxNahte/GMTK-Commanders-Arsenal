using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cacodaemon : BaseEnemyControls
{
    // Enemy state goes from follow -> chase -> attack. Move speed is constant
    [SerializeField] float chaseRange;
    [SerializeField] float attackRange;

    int animId_IsChasing;
    int animId_IsAttacking;

    protected override void Start()
    {
        base.Start();

        animId_IsChasing = Animator.StringToHash("IsChasing");
        animId_IsAttacking = Animator.StringToHash("IsAttacking");
    }

    protected override void Update()
    {
        base.Update();

        Vector2 offset = PlayerAI.instance.transform.position - transform.position;

        // Simple follow 
        SetMoveDir(offset);

        animator.SetBool(animId_IsAttacking, offset.sqrMagnitude < attackRange);
        animator.SetBool(animId_IsChasing, offset.sqrMagnitude < chaseRange);
    }

    private void OnDrawGizmosSelected()
    {
        Color originalColor = Gizmos.color;

        Gizmos.color = new Color(1f, 0.5f, 0f); // Orange
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = originalColor;
    }
}
