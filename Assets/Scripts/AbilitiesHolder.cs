using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitiesHolder : MonoBehaviour
{
    public Ability[] ability;
    public KeyCode[] key;
    Unit unit;
    int ability_n = -1;
    public SelectingAgent select_agent;
    public Vector2 load_pos_atk;
    public int cumRest = 0;
    public int loopStop = 0;
    public bool abilityLocked = false;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    enum AbilityState
    {
        ready,
        aiming,
        active,
        execute,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && ability_n != -1)
        {
            ResetAll();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && abilityLocked == false)
        {
            if (ability_n != -1)
            {
                state = AbilityState.ready;
                ResetAbility(ability_n);
            }
            ability_n = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && abilityLocked == false)
        {
            if (ability_n != -1)
            {
                state = AbilityState.ready;
                ResetAbility(ability_n);
            }
            ability_n = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && abilityLocked == false)
        {
            if (ability_n != -1)
            {
                state = AbilityState.ready;
                ResetAbility(ability_n);
            }
            ability_n = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && abilityLocked == false)
        {
            if (ability_n != -1)
            {
                state = AbilityState.ready;
                ResetAbility(ability_n);
            }
            ability_n = 3;
        }
        if (ability_n != -1)
        {
            switch (state)
            {
                case AbilityState.ready:
                    Ready(ability_n);
                    break;
                case AbilityState.aiming:
                    Aiming(ability_n);
                    break;
                case AbilityState.execute:
                    Execute(ability_n);
                    break;
                case AbilityState.cooldown:
                    Cooldown(ability_n);
                    ability_n = -1;
                    break;
            }
        }
    }

    void Ready(int i)
    {
        ability[i].SetInstance(this.gameObject);
        state = AbilityState.aiming;
    }
    bool block_aim = false;
    void Aiming(int i)
    {
        bool validTarget = false;
        if (!block_aim)
        {
            validTarget = ability[i].Aiming(this.gameObject);
        }
        if (Input.GetMouseButtonDown(0) && validTarget && !block_aim)
        {
            abilityLocked = true;
            if (!ability[i].hasPrevImage)
            {
                state = AbilityState.execute;
            }
            else
            {
                block_aim = true;
                StartCoroutine(PrevImg(i, ability[i].prevImage));
            }
        }
    }
    bool isBlockedExe = false;
    void Execute(int i)
    {
        ability[i].Execute(this.gameObject, unit.act_atk);
        if (!ability[i].hasNextImage)
        {
            state = AbilityState.cooldown;
        }
        else if(!isBlockedExe)
        {
            isBlockedExe = true;
            StartCoroutine(PostImg(i, ability[i].nextImage));
        }
    }
    void Cooldown(int i)
    {
        abilityLocked = false;
        if (select_agent.lastUnitSelected.GetComponent<Unit>().act_act >= ability[i].cost)
        {
            ability[i].OnCooldown(this.gameObject);
            state = AbilityState.ready;
            select_agent.isAiming = false;
            select_agent.AbilityCost(ability[i].cost);
        }
        else
        {
            SaveAction(i);
        }
        
    }
    void ResetAbility(int i)
    {
        ability[i].Reset(this.gameObject);
        state = AbilityState.ready;
    }
    void ResetAll()
    {
        state = AbilityState.ready;
        ResetAbility(ability_n);
        ability_n = -1;
        select_agent.isAiming = false;
    }
    void SaveAction(int i)
    {
        int rest = ability[i].cost - select_agent.lastUnitSelected.GetComponent<Unit>().act_act - cumRest;
        ResetAll();
        load_pos_atk = ability[i].SaveAction(this.gameObject);
        select_agent.SaveAbility(new Vector2Int(i, (select_agent.lastUnitSelected.GetComponent<Unit>().act_act + cumRest)));
    }
    public void LoadAction(Vector2Int v)
    {
        ability_n = v.x;
        int cost = ability[v.x].cost - v.y;
        cumRest = v.y + cumRest;

        if (select_agent.lastUnitSelected.GetComponent<Unit>().act_act >= cost)
        {
            ability[v.x].LoadAction(this.gameObject, unit.act_atk, load_pos_atk);
            select_agent.isAiming = false;
            ability[v.x].Reset(this.gameObject);
            state = AbilityState.ready;
            select_agent.ChannelingAbility2(cost);

            cumRest = 0;
        }
        else
        {
            SaveAction(v.x);
        }
    }
    private IEnumerator PrevImg(int i, Sprite s)
    {
        DisplayImgAbility imageManager = GameObject.Find("ImageManager").GetComponent<DisplayImgAbility>();
        yield return new WaitForSeconds(0.1f);

        imageManager.DisplayImg(i, s);
        yield return new WaitForSeconds(2f);

        Destroy(imageManager.canvas_inst);
        state = AbilityState.execute;
        block_aim = false;
    }
    private IEnumerator PostImg(int i, Sprite s)
    {
        DisplayImgAbility imageManager = GameObject.Find("ImageManager").GetComponent<DisplayImgAbility>();
        yield return new WaitForSeconds(1f);

        imageManager.DisplayImg(i, s);
        yield return new WaitForSeconds(2f);

        Destroy(imageManager.canvas_inst);
        state = AbilityState.cooldown;
        isBlockedExe = false;
    }
}
