using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Projectiles
{
    public float speed;
    public float rotationSpeed;

    private Vector3 startPosition;
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();

        startPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Vector3 dir = HeroAI.instance.transform.position - transform.position;
        transform.right = dir;

        transform.position += transform.right * speed * Time.deltaTime;
    }
}
