using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;

public class Unit : MonoBehaviour
{
    public bool _isSelected = false;
    public GameObject _selectedGO;

    public SO_unit unit;

    [HideInInspector]public string nombre;
    [HideInInspector]public int act_heal;
    [HideInInspector]public int act_atk;
    [HideInInspector]public int act_def;
    [HideInInspector]public int act_spd;
    [HideInInspector]public int act_act;
    [HideInInspector]public int act_wis;

    public GameObject takeDamage;
    public int actualInitiaive;
    public List<GameObject> ranges = new List<GameObject>();
    public Vector2Int loadedAction = new Vector2Int(0, 0);
    public HealthBarBehaviour healthBar;

    public bool hasTurn;

    public void Start()
    {
        SetValues();
    }

    public void Selected(bool s, GameObject range)
    {
        _isSelected = s;
        if (_isSelected)
        {
            _selectedGO.SetActive(true);
            InstanceRangeMovement(range);
        }
        else
        {
            _selectedGO.SetActive(false);
            DeletRangeMovement();
        }
    }

    public void TakeEffect(int damage)
    {
        if (damage >= 0)
        {
            act_heal -= damage;
            var t = Instantiate(takeDamage, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            t.GetComponentInChildren<TextMeshPro>().color = Color.red;
            t.GetComponentInChildren<TextMeshPro>().text = "-" + damage;
            healthBar.SetHealth(act_heal, unit.health);
        }
        else //(here is healing)
        {
            if (act_heal - damage >= unit.health)//damage is negative value 
            {
                act_heal = unit.health;
            }
            else
            {
                act_heal -= damage;
            }
            var t = Instantiate(takeDamage, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            t.GetComponentInChildren<TextMeshPro>().color = Color.green;
            t.GetComponentInChildren<TextMeshPro>().text = "+" + (-damage).ToString();
        }
        if (act_heal <= 0)
        {
            GameObject.Find("SelectionSystem").GetComponent<SelectingAgent>().RemoveRow(this.gameObject);
            if (_isSelected)
            {
                GameObject.Find("SelectionSystem").GetComponent<SelectingAgent>().NextUnit();
            }
            Destroy(gameObject);
        }
    }

    public void InstanceRangeMovement(GameObject range)
    {
        for(int i = 0; i < act_act; i++)
        {
            GameObject r = Instantiate(range, this.gameObject.transform.position, Quaternion.identity);
            r.transform.localScale = new Vector3((i + 2) * 2, (i + 2) * 2, i + 2);
            r.transform.parent = null;
            ranges.Add(r);
        }
    }

    public void DeletRangeMovement()
    {
        for (var i = 0; i < ranges.Count; i++)
        {
            Destroy(ranges[i].gameObject);
        }
    }

    void SetValues()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = unit.sprite;
        act_heal = unit.health;
        act_atk = unit.atk;
        act_def = unit.def;
        act_spd = unit.spd;
        act_act = unit.act;
        act_wis = unit.wis;
        healthBar.SetHealth(act_heal, unit.health);
    }
}
