using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Skillshot", menuName = "Scriptables/Abilities/Skillshot", order = 0)]
public class Skillshot : Ability
{
    public GameObject bullet;
    public GameObject skillshot;
    GameObject skillshotInst;
    GameObject rangeInst;
    public float legth;
    public float width;

    public override void SetInstance(GameObject parent)
    {
        skillshotInst = Instantiate(skillshot, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        skillshotInst.transform.parent = parent.transform;
        skillshotInst.transform.localScale = new Vector3(width, legth * 2, 1);
        skillshotInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, skillshotInst.GetComponent<SpriteRenderer>().color.a);
        rangeInst = Instantiate(rangeGO, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        rangeInst.transform.parent = parent.transform;
        rangeInst.transform.localScale = new Vector3(range * 2, range * 2, 1);
        rangeInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, rangeInst.GetComponent<SpriteRenderer>().color.a);
    }
    public override bool Aiming(GameObject parent)
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - parent.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        skillshotInst.transform.rotation = rotation;
        return true;
    }
    public override void Execute(GameObject parent, int damage)
    {
        GameObject b = Instantiate(bullet, parent.transform.position, parent.transform.rotation);
        BulletTracking bt = b.GetComponent<BulletTracking>();
        bt.user = parent;
        bt.damage = (int)(baseEffect + (damage * damageMult) + (damage * wisdomMult) + (damage * defenseMult));
        b.GetComponent<SpriteRenderer>().color = element.color;
        b.transform.localScale = new Vector3(width, width, 1);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.AddForce(skillshotInst.transform.up * 10, ForceMode2D.Impulse);
        bt.startPosition = parent.transform.position;
        bt.distance = legth;
    }
    public override void OnCooldown(GameObject parent)
    {
        Destroy(skillshotInst);
        Destroy(rangeInst);
    }
    public override void Reset(GameObject parent)
    {
        Destroy(skillshotInst);
        Destroy(rangeInst);
    }
}
