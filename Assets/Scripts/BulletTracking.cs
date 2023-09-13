using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracking : MonoBehaviour
{
    public Vector3 startPosition;
    public float distance;
    public int damage;
    public GameObject user;

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > distance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Unit") && col.gameObject != user)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit != null)
            {
                unit.TakeEffect(damage);
                Destroy(gameObject);
            }
        }
    }
}
