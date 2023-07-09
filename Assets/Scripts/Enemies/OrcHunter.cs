using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHunter : BaseEnemyControls
{
    [SerializeField] float rangeAttackDist;

    [SerializeField] GameObject spearPrefab;


    bool isAttacking;

    Vector3 spearSpawnOffset = new Vector3(0.002f, 0.125f);

    int animId_IsAttacking;

    public void OnStartAttack()
    {
        isAttacking = true;
    }

    public void OnEndAttack() 
    {
        isAttacking = false;
    }

    public void OnThrow()
    {
        Vector2 direction = (HeroAI.instance.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // TODO: Put in pool
        GameObject spear = Instantiate(spearPrefab, transform.position + spearSpawnOffset,
                                        Quaternion.Euler(0f, 0f, angle));
    }

    protected override void Start()
    {
        base.Start();

        isAttacking = false;

        animId_IsAttacking = Animator.StringToHash("IsAttacking");
    }

    protected override void Update()
    {
        base.Update();

        Vector2 offset = HeroAI.instance.transform.position - transform.position;

        if (offset.sqrMagnitude < rangeAttackDist * rangeAttackDist)
        {
            animator.SetTrigger(animId_IsAttacking);
        }

        if (isAttacking)
            SetMoveDir(Vector2.zero);
        else
            SetMoveDir(offset); // Simple follow
    }

    private void OnDrawGizmosSelected()
    {
        Color originalColor = Gizmos.color;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttackDist);

        Gizmos.color = originalColor;
    }
}
