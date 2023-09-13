using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
[CreateAssetMenu(fileName = "AoEDamage", menuName = "Scriptables/Abilities/AoEDamage", order = 1)]
public class AoEDamage : Ability
{
    GameObject rangeInst;
    public GameObject area;
    GameObject areaInst;
    public float areaOfImpact;
    // Start is called before the first frame update
    public override void SetInstance(GameObject parent)
    {
        rangeInst = Instantiate(rangeGO, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        rangeInst.transform.parent = parent.transform;
        rangeInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, rangeInst.GetComponent<SpriteRenderer>().color.a);
        rangeInst.transform.localScale = new Vector3(range * 2, range * 2, 1);

        areaInst = Instantiate(area, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        areaInst.transform.localScale = new Vector3(areaOfImpact * 2, areaOfImpact * 2, 1);
        areaInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, areaInst.GetComponent<SpriteRenderer>().color.a);
        areaInst.transform.parent = parent.transform;
    }
    public override bool Aiming(GameObject parent)
    {
        Vector2 centerPosition = parent.gameObject.transform.position;
        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), centerPosition);
        if (distance < range)
        {
            areaInst.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); //*BlackCenter* + all that Math
            return true;
        }
        else
            return false;
    }
    public override void Execute(GameObject parent, int damage)
    {
        areaInst.GetComponent<Collider2D>().enabled = true;
        if(canDamageSelf == false)
        {
            areaInst.GetComponent<AoE_Attack>().parent = parent;
        }
        areaInst.GetComponent<AoE_Attack>().damage = (int)(baseEffect + (damage * damageMult) + (damage * wisdomMult) + (damage * defenseMult));
        areaInst.transform.parent = null;
        areaInst.GetComponent<AoE_Attack>().DestroyFunc();
    }
    public override void OnCooldown(GameObject parent)
    {
        Destroy(rangeInst);
        //Destroy(areaInst); Destuir desde el script del AoE
    }
    public override void Reset(GameObject parent)
    {
        Destroy(rangeInst);
        Destroy(areaInst);
    }
    public override Vector2 SaveAction(GameObject parent)
    {
        Vector2 centerPosition = parent.gameObject.transform.position;
        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), centerPosition);
        if (distance < range)
        {
            areaInst.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); //*BlackCenter* + all that Math
        }
        return areaInst.transform.position;
    }
    public override void LoadAction(GameObject parent, int damage, Vector2 LoadedPositionAttack)
    {
        Debug.Log("LoadAction()");
        GameObject aInst = Instantiate(area, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        aInst.transform.localScale = new Vector3(areaOfImpact * 2, areaOfImpact * 2, 1);
        aInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, aInst.GetComponent<SpriteRenderer>().color.a);
        aInst.transform.parent = parent.transform;

        aInst.transform.position = LoadedPositionAttack;

        aInst.GetComponent<Collider2D>().enabled = true;
        aInst.GetComponent<AoE_Attack>().damage = (int)(baseEffect + (damage * damageMult) + (damage * wisdomMult) + (damage * defenseMult));
        aInst.transform.parent = null;
        aInst.GetComponent<AoE_Attack>().DestroyFunc();

        //Destroy(aInst);
    }
}
