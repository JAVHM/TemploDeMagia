using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Skillshot", menuName = "Scriptables/Abilities/Skillshot", order = 5)]
public class ModifyTerrain : Ability
{
    GameObject rangeInst;
    public float legth;
    public float width;

    public override void SetInstance(GameObject parent)
    {
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
        return true;
    }
    public override void Execute(GameObject parent, int damage)
    {
        
    }
    public override void OnCooldown(GameObject parent)
    {
        
    }
    public override void Reset(GameObject parent)
    {
        
    }
}
