using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public new string name;
    public Sprite image;
    public float cooldown;
    public float range;
    public GameObject rangeGO;
    public Element element;
    public int baseEffect;
    public float damageMult;
    public float wisdomMult;
    public float defenseMult;
    public int cost;
    public bool hasPrevImage;
    public Sprite prevImage;
    public bool hasNextImage;
    public Sprite nextImage;
    public bool canDamageSelf;

    public enum Type { damage, heal, none, especial };
    public Type type;

    public virtual bool Check(Unit u)
    {
        if(u.act_spd >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void SetInstance(GameObject parent)
    {
        Debug.Log("Aiming");
    }
    public virtual bool Aiming(GameObject parent)
    {
        Debug.Log("activate");
        return true;
    }
    public virtual void preImg(GameObject parent)
    {
        Debug.Log("activate");
    }
    public virtual void Execute(GameObject parent, int damage)
    {
        Debug.Log("execute");
    }
    public virtual void posImg(GameObject parent)
    {
        Debug.Log("activate");
    }
    public virtual void OnCooldown(GameObject parent)
    {
        Debug.Log("OnCooldown");
    }
    public virtual void Reset(GameObject parent)
    {
        Debug.Log("OnCooldown");
    }
    public virtual Vector2 SaveAction(GameObject parent)
    {
        Debug.Log("OnCooldown");
        return new Vector2(0,0);
    }
    public virtual void LoadAction(GameObject parent, int damage, Vector2 LoadedPositionAttack)
    {
        Debug.Log("OnCooldown");
    }
}
