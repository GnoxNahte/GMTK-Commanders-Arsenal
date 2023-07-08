using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAI : MonoBehaviour
{
    [SerializeField] private int avoidanceRadius;
    [SerializeField] private ContactFilter2D contactFilter;

    private HeroControls controls;

    [SerializeField] [ReadOnly]
    private List<Collider2D> enemies;

    // Singleton
    // Use it so I can implement stuff fast since in game jam
    public static HeroAI instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than 1 SingletonClass. Destroying this. Name: " + name);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Collider2D>(50);

        // Caching
        controls = GetComponent<HeroControls>();
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
        }

        controls.SetMoveDir(moveDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
