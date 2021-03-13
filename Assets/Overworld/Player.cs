using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Snowman snowman;
    public float speed = 5;
    public float radius = 1;
    public CircleCollider2D playerCollider;
    public ContactFilter2D contactFilter;

    // Start is called before the first frame update
    void Start()
    {
        snowman.SetRandomColors();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = new Vector3(
            Input.GetAxis("Horizontal") * Time.deltaTime * speed,
            Input.GetAxis("Vertical") * Time.deltaTime * speed, 0);

        transform.Translate(translation);

        List<Collider2D> results = new List<Collider2D>();

        Physics2D.OverlapCollider(playerCollider, contactFilter, results);

        foreach(Collider2D c in results)
        {
            Vector3 nrm = transform.position - (Vector3)c.ClosestPoint(transform.position);
            if (playerCollider.radius < nrm.magnitude) continue;
            transform.Translate(nrm.normalized * (playerCollider.radius - nrm.magnitude));
        }

    }
}
