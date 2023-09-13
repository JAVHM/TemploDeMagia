using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracking : MonoBehaviour
{
    public Vector3 startPosition;
    public float Distance;
    public int damage;
    public GameObject user;
    
    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > Distance)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Unit" && col.gameObject != user)
        {
            col.gameObject.GetComponent<Unit>().TakeEffect(damage);
            Destroy(gameObject);
        }
    }
}
