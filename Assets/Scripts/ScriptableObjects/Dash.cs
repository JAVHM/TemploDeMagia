using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
[CreateAssetMenu(fileName = "Dash", menuName = "Scriptables/Abilities/Dash", order = 4)]
public class Dash : Ability
{
    public GameObject bullet;
    public GameObject skillshot;
    public GameObject area;
    GameObject skillshotInst;
    GameObject rangeInst;
    GameObject areaInst;
    public float legth;
    public float width;
    public bool canDash;

    public override void SetInstance(GameObject parent)
    {
        skillshotInst = Instantiate(skillshot, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        skillshotInst.transform.parent = parent.transform;
        skillshotInst.transform.localScale = new Vector3(width, range * 2, 1);
        skillshotInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, skillshotInst.GetComponent<SpriteRenderer>().color.a);
        rangeInst = Instantiate(rangeGO, new Vector3(
            parent.transform.position.x,
            parent.transform.position.y,
            parent.transform.position.z), Quaternion.identity);
        rangeInst.transform.parent = parent.transform;
        rangeInst.transform.localScale = new Vector3(range * 2, range * 2, 1);
        rangeInst.GetComponent<SpriteRenderer>().color = new Color(element.color.r, element.color.g, element.color.b, rangeInst.GetComponent<SpriteRenderer>().color.a);
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
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - parent.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        skillshotInst.transform.rotation = rotation;
        if (distance < range)
        {
            areaInst.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); //*BlackCenter* + all that Math
            Collider2D[] colliders = Physics2D.OverlapCircleAll(areaInst.transform.position, parent.transform.localScale.x);
            canDash = true;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Unit"))
                {
                    canDash = false;
                    areaInst.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
            }
            if (canDash == true)
            {
                areaInst.GetComponent<SpriteRenderer>().color = Color.green;
                return true;
            }
            else
                return false;
        }
        else
        {
            canDash = false;
            areaInst.GetComponent<SpriteRenderer>().color = Color.red;
            return false;
        }
    }
    public override void Execute(GameObject parent, int damage)
    {
        parent.gameObject.GetComponent<AgentMovement>().SetTargetPositionAuto((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //parent.gameObject.GetComponent<NavMeshAgent>().speed *= 3;
    }

    public override void OnCooldown(GameObject parent)
    {
        //parent.gameObject.GetComponent<NavMeshAgent>().speed /= 3;
        Destroy(skillshotInst);
        Destroy(rangeInst);
        Destroy(areaInst);
    }
    public override void Reset(GameObject parent)
    {
        Destroy(skillshotInst);
        Destroy(rangeInst);
        Destroy(areaInst);
    }
}