using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{
    public GameObject container;
    public GameObject prefab;
    public int velocities = 1;

    public void InstantiateBlock(int rowlss, GameObject[,] matrix, int co, int ro)
    {
        container.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
        container.GetComponent<GridLayoutGroup>().constraintCount = matrix.GetLength(0);

        int cols = matrix.GetLength(0);
        int rows = matrix.GetLength(1);

        for (int j = 0; j < cols; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                GameObject g = Instantiate(prefab, container.transform);
                g.transform.SetParent(container.GetComponent<RectTransform>());
                if (matrix[j,i].GetComponent<Unit>() != null)
                {
                    g.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = matrix[j, i].GetComponent<Unit>().unit.sprite;
                    g.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    if (j == co && ro == i)
                        g.gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                }
            }
        }

        float width = container.GetComponent<RectTransform>().rect.width; float height = container.GetComponent<RectTransform>().rect.height;
        Vector2 newSize = new Vector2(0, 0);
        newSize = new Vector2(width / rows, height / cols);
        container.GetComponent<GridLayoutGroup>().cellSize = newSize;


        foreach (Transform child in container.transform)
            foreach (Transform c in child.transform)
                c.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    public void DeleteChildrens()
    {
        foreach (Transform child in container.transform)
            Destroy(child.gameObject);
        velocities = 0;
    }
}
