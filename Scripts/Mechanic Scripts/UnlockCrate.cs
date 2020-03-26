using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCrate : MonoBehaviour
{
    public string playerprefName;
    public CrateScriptableObject crate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
            PlayerPrefs.SetInt(playerprefName, 1);
            FindObjectOfType<LevelManager>().UnlockCrate(crate); //unlock crate function
        }
    }
}
