using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] private int avoidanceRadius;
    [SerializeField] private ContactFilter2D contactFilter;

    private PlayerControls playerMovement;

    [SerializeField] [ReadOnly]
    private List<Collider2D> enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Collider2D>(50);

        // Caching
        playerMovement = GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        int count = Physics2D.OverlapCircle(transform.position, avoidanceRadius, contactFilter, enemies);

        Vector2 moveDir = Vector2.zero;

        foreach (Collider2D enemy in enemies)
        {
            Vector2 offset = transform.position - enemy.transform.position;
            moveDir += offset / Mathf.Max(offset.sqrMagnitude, 1);
            print("OFfset: " + offset);
        }

        print("Move Dir: " + moveDir);

        playerMovement.SetMoveDir(moveDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
