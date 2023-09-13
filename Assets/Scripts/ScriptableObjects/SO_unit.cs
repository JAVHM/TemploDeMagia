using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "unit", menuName = "Scriptables/unit", order = 0)]
public class SO_unit : ScriptableObject
{
    public string nombre;
    public Sprite sprite;
    public int health;
    public int atk;
    public int def;
    public int spd;
    public int ini;
    public int act;
    public int wis;
}
