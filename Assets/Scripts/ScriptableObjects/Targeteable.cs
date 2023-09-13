using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Targeteable", menuName = "Scriptables/Abilities/Targeteable", order = 2)]
public class Targeteable : Ability
{
    GameObject rangeInst;
    public GameObject area;
    GameObject areaInst;

    // Start is called before the first frame update
    public override void SetInstance(GameObject parent)
    {
        rangeInst = Instantiate(rangeGO, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        rangeInst.transform.parent = parent.transform;
        rangeInst.transform.localScale = new Vector3(range * 2, range * 2, 1);
        areaInst = Instantiate(area, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        areaInst.transform.localScale = new Vector3(0.1f, 0.1f, 1);
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
        switch (type)
        {
            case Type.damage:
                areaInst.GetComponent<AoE_Attack>().damage = (int)(baseEffect + (damage * damageMult) + (damage * wisdomMult) + (damage * defenseMult));
                break;
            case Type.heal:
                areaInst.GetComponent<AoE_Attack>().damage = -(int)(baseEffect + (damage * damageMult) + (damage * wisdomMult) + (damage * defenseMult));
                break;
            case Type.none:
                break;
        }
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
}
