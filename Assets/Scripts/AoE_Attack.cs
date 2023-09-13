using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_Attack : MonoBehaviour
{
    public int damage;
    public GameObject parent;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Unit" && col.gameObject != parent)
        {
            col.gameObject.GetComponent<Unit>().TakeEffect(damage);
        }
    }

    public IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    public void DestroyFunc()
    {
        StartCoroutine(DestroyCoroutine());
    }
}
