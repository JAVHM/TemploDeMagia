using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GameObject container;
    public GameObject prefab;
    public int velocities = 1;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InstantiateBlock(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject g = Instantiate(prefab, container.transform);
            g.transform.SetParent(container.GetComponent<RectTransform>());
        }
        velocities = amount;

        float width = container.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(
                width / velocities - 1,
                container.GetComponent<GridLayoutGroup>().cellSize.y);
        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }

    public void DeleteChildrens()
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        velocities = 0;
    }
}
