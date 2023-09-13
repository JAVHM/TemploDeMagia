using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayImgAbility : MonoBehaviour
{
    public GameObject canvas_prefab;
    public GameObject canvas_inst;

    public void DisplayImg(int i, Sprite s)
    {
        canvas_inst = Instantiate(canvas_prefab, Vector3.zero, Quaternion.identity);
        Image image = canvas_inst.GetComponentInChildren<Image>();
        image.sprite = s;
    }
}
