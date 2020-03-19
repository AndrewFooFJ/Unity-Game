using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crate", menuName = "Crate Type")]
public class CrateScriptableObject : ScriptableObject
{
    //public int id;
    public string nameOfCrate;
    public string unlockCritrea;
    public Sprite crateSprite;
}
