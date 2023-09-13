using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayImgAbility : MonoBehaviour
{
    public GameObject canvasPrefab;
    public GameObject canvas_inst;

    public void DisplayImg(int i, Sprite s)
    {
        canvas_inst = Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
        Image image = canvas_inst.GetComponentInChildren<Image>();
        image.sprite = s;
    }
}
