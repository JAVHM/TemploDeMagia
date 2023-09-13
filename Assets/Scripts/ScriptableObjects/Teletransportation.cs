using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Teletransportation", menuName = "Scriptables/Abilities/Teletransportation", order = 3)]
public class Teletransportation : Ability
{
    GameObject rangeInst;
    public GameObject area;
    GameObject areaInst;
    public float areaOfImpact;
    public bool canTeleport;
    Vector3 teletransp;
    
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
        areaInst.transform.localScale = new Vector3(parent.transform.localScale.x, parent.transform.localScale.y, 1);
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(areaInst.transform.position, parent.transform.localScale.x);
            canTeleport = true;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Unit"))
                {
                    canTeleport = false;
                    areaInst.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
            }
            if (canTeleport == true)
            {
                areaInst.GetComponent<SpriteRenderer>().color = Color.green;
                teletransp = areaInst.transform.position;
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("Fuera de rango");
            canTeleport = false;
            areaInst.GetComponent<SpriteRenderer>().color = Color.red;
            return false;
        }
    }


    public override void Execute(GameObject parent, int damage)
    {
        if (canTeleport == true)
        {
            parent.gameObject.transform.position = areaInst.transform.position;
            parent.gameObject.GetComponent<AgentMovement>().SetTargetPositionAuto(teletransp);
        }
    }


    public override void OnCooldown(GameObject parent)
    {
        Destroy(rangeInst);
        Destroy(areaInst);
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
        GameObject aInst = Instantiate(area, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        aInst.transform.localScale = new Vector3(areaOfImpact * 2, areaOfImpact * 2, 1);
        aInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, aInst.GetComponent<SpriteRenderer>().color.a);
        aInst.transform.parent = parent.transform;
        aInst.transform.position = LoadedPositionAttack;

        Vector2 centerPosition = parent.gameObject.transform.position;
        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), centerPosition);
        if (distance < range)
        {
            areaInst.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); //*BlackCenter* + all that Math
            Collider2D[] colliders = Physics2D.OverlapCircleAll(areaInst.transform.position, parent.transform.localScale.x);
            canTeleport = true;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Unit"))
                {
                    Debug.Log("coll");
                    canTeleport = false;
                    areaInst.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
            }
            if (canTeleport == true)
            {
                Debug.Log("no coll");
                areaInst.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            Debug.Log("Fuera de rango");
            canTeleport = false;
            areaInst.GetComponent<SpriteRenderer>().color = Color.red;
        }
        //Destroy(aInst);
    }
}
