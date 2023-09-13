
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.ComponentModel;

public class SelectingAgent : MonoBehaviour
{
    private Vector3 target;
    public GameObject showcase;
    public GameObject lastUnitSelected;
    public GameObject showCaseInstance;
    public bool unitSelected = false;
    public bool isAiming = false;
    public int ability = 0;
    public AbilitiesHolder abilitiesHolder;
    public AgentMovement agentMovement;
    public int count = 0;
    public GameObject range;
    public GameObject[] abilitiesIcons;
    public DynamicGridLayout DynamicGridLayout;
    public TurnIndicator turnIndicator;

    public List<GameObject> unitsList = new List<GameObject>();
    public GameObject[,] mat;
    int rows;
    int cols;
    int rowlss;
    public int cTemp = 0;
    public int rTemp = 0;

    bool needUpdateRange = false;

    private void Start()
    {
        GameObject[] unitsToPlay = GameObject.FindGameObjectsWithTag("Unit");
        unitsToPlay = unitsToPlay.OrderBy(x => x.GetComponent<Unit>().actualInitiaive).ToArray();
        Array.Reverse(unitsToPlay);

        foreach (GameObject unitToPlay in unitsToPlay)
        {
            if (unitToPlay.GetComponent<Unit>().hasTurn)
            {
                unitsList.Add(unitToPlay);
            }
        }

        mat = UpdateColums(CreateMatrix(unitsList)); //OJOJOJOJOJOJOJOJO, SI HAY PROBLEMAS VER ACA
        UnitSelected(mat[0, 0], 0, 0);
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        
        unitSelected = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (lastUnitSelected.GetComponent<Unit>().act_act != 0 && lastUnitSelected.GetComponent<AgentMovement>().moving == false && needUpdateRange)
        {
            UpdateRange();
            UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        }
        if (lastUnitSelected.GetComponent<Unit>().act_act <= 0 && lastUnitSelected.GetComponent<AgentMovement>().moving == false)
        {
            NextUnit();
        }
        if (Input.GetMouseButtonDown(0) && unitSelected && !isAiming && lastUnitSelected.GetComponent<Unit>().act_act >= (int)Vector2.Distance(clickpos, lastUnitSelected.transform.position))
        {
            agentMovement.SetTargetPosition(clickpos);
            needUpdateRange = true;
            if ((int)Vector2.Distance(clickpos, lastUnitSelected.transform.position) == 0)
            {
                lastUnitSelected.GetComponent<Unit>().act_act -= 1;
            }
            else
            {
                lastUnitSelected.GetComponent<Unit>().act_act -= (int)Vector2.Distance(clickpos, lastUnitSelected.transform.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) && lastUnitSelected != null)
        {
            isAiming = true;
        }
    }

    void UnitSelected(GameObject unit, int c, int r)
    {
        if (lastUnitSelected != null)
        {
            lastUnitSelected.GetComponent<Unit>().Selected(false, range);
            abilitiesHolder.enabled = false;

        }
        if (showCaseInstance != null)
        {
            //Destroy(showCaseInstance);
        }
        //showCaseInstance = Instantiate(showcase);

        lastUnitSelected = unit;

        lastUnitSelected.GetComponent<Unit>().act_act = lastUnitSelected.GetComponent<Unit>().unit.act;
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);

        lastUnitSelected.GetComponent<Unit>().Selected(true, range);
        abilitiesHolder = lastUnitSelected.GetComponent<AbilitiesHolder>();
        abilitiesHolder.select_agent = this;
        abilitiesHolder.enabled = true;
        agentMovement = lastUnitSelected.GetComponent<AgentMovement>();
        //showCaseInstance.transform.Find("Name_txt").GetComponent<TextMeshProUGUI>().text = unit.GetComponent<Unit>()._Unit.nombre;

        DynamicGridLayout.DeleteChildrens();
        DynamicGridLayout.InstantiateBlock(lastUnitSelected.GetComponent<Unit>().act_act);
        turnIndicator.DeleteChildrens();
        turnIndicator.InstantiateBlock(rowlss, mat, c, r);

        unitSelected = true;

        if (lastUnitSelected.GetComponent<Unit>().loadedAction != new Vector2Int(0, 0))
        {
            abilitiesHolder.LoadAction(lastUnitSelected.GetComponent<Unit>().loadedAction);
        }
    }

    public void NextUnit(int ctemp, int rtemp)
    {
        bool found = false;

        if (rTemp > rows)
        {
            rTemp = 0;
            cTemp += 1;
            if (cTemp > cols)
            {
                rTemp = 0;
                cTemp = 0;
                NextUnit(rTemp, cTemp);
            }
        }

        for (int i = rtemp; i < rows; i++)
        {
            for (int j = ctemp; j < cols; j++)
            {
                if (mat[j, i].name != "Empty" && mat[j, i].GetComponent<Unit>().hasTurn)
                {
                    UnitSelected(mat[j, i], j, i);
                    rTemp = i ;
                    cTemp = j + 1;
                    found = true;
                    break;
                }
                if (found == true)
                {
                    break;
                }
            }
            ctemp = 0;
            if (found == true)
            {
              
                break;
            }
        }

        if (found == false)
        {
            NextUnit(0, 0);
        }
    }

    public void AbilityCost(int cost)
    {
        lastUnitSelected.GetComponent<Unit>().act_act -= cost;
        UpdateRange();
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        if (lastUnitSelected.GetComponent<Unit>().act_act <= 0)
        {
            NextUnit();
        }
        
    }

    public void ChannelingAbility()
    {
        lastUnitSelected.GetComponent<Unit>().loadedAction = new Vector2Int(0,0);
        lastUnitSelected.GetComponent<Unit>().act_act = 0;
        
        NextUnit();
        UpdateRange();
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        
    }

    public void ChannelingAbility2(int cost)
    {
        lastUnitSelected.GetComponent<Unit>().loadedAction = new Vector2Int(0, 0);
        lastUnitSelected.GetComponent<Unit>().act_act -= cost;
        UpdateRange();
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        if (lastUnitSelected.GetComponent<Unit>().act_act <= 0)
        {

            NextUnit();
        }
    }

    public void SaveAbility(Vector2Int v)
    {
        lastUnitSelected.GetComponent<Unit>().loadedAction = v;
        UpdateRange();
        UpdateIcons(lastUnitSelected.GetComponent<AbilitiesHolder>().ability);
        NextUnit();
    }

    public void UpdateRange()
    {
        lastUnitSelected.GetComponent<Unit>().DeletRangeMovement();
        lastUnitSelected.GetComponent<Unit>().InstanceRangeMovement(range);
        needUpdateRange = false;
        DynamicGridLayout.DeleteChildrens();
        DynamicGridLayout.InstantiateBlock(lastUnitSelected.GetComponent<Unit>().act_act);
    }

    public void UpdateIcons(Ability[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            abilitiesIcons[i].GetComponent<Image>().sprite = a[i].image;
        }
    }

    public GameObject[,] CreateMatrix(List<GameObject> UL)
    {
        rows = GetMaxInit(UL) - GetMinInit(UL) + GetSpeedFromMinInit(UL) + GetDiifMaxSpeedFromMin(UL);
        rowlss = GetMinInit(UL) - GetSpeedFromMinInit(UL) - GetDiifMaxSpeedFromMin(UL);
        cols = UL.Count;

        GameObject[,] matrix = new GameObject[cols, rows];

        for (int j = 0; j < cols; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                if (UL[j].GetComponent<Unit>().actualInitiaive >= (rows - i + rowlss) && (UL[j].GetComponent<Unit>().actualInitiaive - UL[j].GetComponent<Unit>().act_spd) < (rows - i + rowlss))
                {
                    matrix[j,i] = UL[j];
                }
                else
                {
                    matrix[j, i] = new GameObject("Empty");
                }
            }
        }
        return matrix;
    }
    public int GetMaxSpeed(List<GameObject> unitsList)
    {
        int maxVal = unitsList.Max(obj => obj.GetComponent<Unit>().act_spd);
        return maxVal;
    }
    public int GetMinSpeed(List<GameObject> unitsList)
    {
        int maxVal = unitsList.Min(obj => obj.GetComponent<Unit>().act_spd);
        return maxVal;
    }
    public int GetMaxInit(List<GameObject> unitsList)
    {
        int maxVal = unitsList.Max(obj => obj.GetComponent<Unit>().actualInitiaive);
        return maxVal;
    }
    public int GetMinInit(List<GameObject> unitsList)
    {
        int minVal = unitsList.Min(obj => obj.GetComponent<Unit>().actualInitiaive);
        return minVal;
    }
    public int GetSpeedFromMinInit(List<GameObject> unitsList)
    {
        int minVal = unitsList.Min(obj => obj.GetComponent<Unit>().actualInitiaive);
        GameObject desiredObject = unitsList.FirstOrDefault(obj => obj.GetComponent<Unit>().actualInitiaive == minVal);

        return desiredObject.GetComponent<Unit>().act_spd;
    }
    public int GetDiifMaxSpeedFromMin(List<GameObject> unitsList)
    {
        int minVal = unitsList.Min(obj => obj.GetComponent<Unit>().actualInitiaive);
        GameObject desiredObject = unitsList.FirstOrDefault(obj => obj.GetComponent<Unit>().actualInitiaive == minVal);
        int minRowless = 999;
        for (int i = 0; i < unitsList.Count; i++)
        {
            int temp = unitsList[i].GetComponent<Unit>().actualInitiaive - unitsList[i].GetComponent<Unit>().act_spd;
            //Debug.Log(unitsList[i].GetComponent<Unit>().actualInitiaive + " - " + unitsList[i].GetComponent<Unit>().actualSpeed + " = " + temp);
            if(temp < minRowless)
            {
                minRowless = temp;
            }
        }
        if(minRowless < 0)
        {
            return -minRowless;
        }
        else
        {
            return 0;
        }
    }

    public void RemoveRow(GameObject g)
    {
        int numRows = mat.GetLength(0);
        int numCols = mat.GetLength(1);
        int temp = 0;
        bool founded = false;

        for (int i = 0; i < numRows; i++)
        {
            
            for (int j = 0; j < numCols; j++)
            {
                GameObject gameObject = mat[i, j];
                
                if (gameObject.name == g.name)
                {
                    if (founded == false)
                    {
                        temp = i;
                        founded = true;
                        if(rTemp >= i)
                        {
                            rTemp -= 1;
                        }
                    }
                }
            }
        }

        GameObject[,] newMatrix = new GameObject[numRows - 1, numCols];

        int newRow = 0;
        for (int i = 0; i < numRows; i++)
        {
            if (i != temp)
            {
                for (int j = 0; j < numCols; j++)
                {
                    newMatrix[newRow, j] = mat[i, j];
                }
                newRow++;
            }
        }
        mat = UpdateColums(newMatrix);
        cols = cols - 1; 
        turnIndicator.DeleteChildrens();
        PrintMatrix(mat);
        turnIndicator.InstantiateBlock(rowlss, mat, cTemp, rTemp);
    }
    public void PrintMatrix(GameObject[,] matrix)
    {
        for (int j = 0; j < cols; j++)
        {
            string rowStr = "";
            for (int i = 0; i < rows; i++)
            {
                rowStr += matrix[j, i].name + " ";
            }
        }
    }
    public void PrintMatrix(GameObject[,] matrix,int rows, int cols)
    {
        for (int j = 0; j < cols; j++)
        {
            string rowStr = "";
            for (int i = 0; i < rows; i++)
            {
                rowStr += matrix[j, i].name + " ";
            }
        }
    }

    public void NextUnit()
    {
        int ctemp = cTemp + 1;
        int rtemp = rTemp;

        bool found = false;

        if (rTemp > rows)
        {
            rTemp = 0;
            cTemp += 1;
            if (cTemp > cols)
            {
                rTemp = 0;
                cTemp = 0;
                NextUnit(rTemp, cTemp);
            }
        }
        for (int i = rtemp; i < rows; i++)
        {
            for (int j = ctemp; j < cols; j++)
            {
                if (mat[j, i].name != "Empty" && mat[j, i].GetComponent<Unit>().hasTurn)
                {
                    rTemp = i;
                    cTemp = j;
                    UnitSelected(mat[j, i], j, i);
                    
                    found = true;
                    break;
                }
                if (found == true)
                {
                    break;
                }
            }
            if (found == true)
            {
                break;
            }
            ctemp = 0;
        }

        if (found == false)
        {
            NextUnit(0, 0);
        }
    }

    public GameObject[,] UpdateColums(GameObject[,] originalMatrix)
    {
        //ReadGameObjectMatrixByColumns(originalMatrix);

        // Get the number of rows and columns in the original matrix
        int numRows = originalMatrix.GetLength(0);
        int numCols = originalMatrix.GetLength(1);

        // Define a list to store the column indices that need to be removed
        List<int> columnsToRemove = new List<int>();

        int temporal = 0;

        // Loop through each column of the original matrix
        for (int col = 0; col < numCols; col++)
        {
            // Assume the current column is composed only by game objects named "Empty"
            bool columnIsEmpty = true;

            // Loop through each row of the current column
            for (int row = 0; row < numRows; row++)
            {
                // If the current game object is not named "Empty", the column is not empty
                if (originalMatrix[row, col].name != "Empty")
                {
                    columnIsEmpty = false;
                    break;
                }
            }

            // If the column is empty, add its index to the list of columns to remove
            if (columnIsEmpty)
            {
                rows = rows - 1;
                if(col <= cTemp)
                {
                    cTemp -= 1;
                }
                //rTemp = 0;
                temporal += 1;
                
                columnsToRemove.Add(col);
            }
        }

        // Define the new matrix with the appropriate size
        GameObject[,] newMatrix = new GameObject[numRows, numCols - columnsToRemove.Count];

        // Loop through each row of the original matrix
        for (int row = 0; row < numRows; row++)
        {
            // Define a variable to keep track of the current column index in the new matrix
            int newCol = 0;

            // Loop through each column of the original matrix
            for (int col = 0; col < numCols; col++)
            {
                // If the current column is not in the list of columns to remove, add it to the new matrix
                if (!columnsToRemove.Contains(col))
                {
                    newMatrix[row, newCol] = originalMatrix[row, col];
                    newCol++;
                }
            }
        }

        //ReadGameObjectMatrixByColumns(newMatrix);
        return newMatrix;
    }

    public void ReadGameObjectMatrixByColumns(GameObject[,] matrix)
    {
        // Get the number of rows and columns in the matrix
        int numRows = matrix.GetLength(0);
        int numColumns = matrix.GetLength(1);

        // Loop through each row of the matrix
        for (int row = 0; row < numRows; row++)
        {
            // Define an empty string to store the row data
            string rowData = "";

            // Loop through each column of the current row
            for (int col = 0; col < numColumns; col++)
            {
                // Append the name of the game object at the current row and column to the row data string
                rowData += matrix[row, col].name + " ";
            }

            // Log the row data to the console using Debug.Log
            Debug.Log(rowData);
        }
    }
}