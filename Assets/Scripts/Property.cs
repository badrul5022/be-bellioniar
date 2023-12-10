using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Property", menuName = "Property", order = 1)]
public class Property :ScriptableObject
{
    public string propertyTitle;
    public int propertyPrice;
    public PropertyNature nature;
    public bool ownable;
    public bool reward;
    public bool penalty;
    public bool spinWheel;
    public bool neutral;
}
