using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public Text[] selectTexts;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.GetInt("TypeOfCrate");
    }

    private void Update()
    {
        PlayerPrefs.GetInt("TypeOfCrate");

        CrateSelectText();
    }

    public void SelectedCrate(int crateInt)
    {
        PlayerPrefs.SetInt("TypeOfCrate", crateInt);
    }

    void CrateSelectText()
    {
        switch (PlayerPrefs.GetInt("TypeOfCrate"))
        {
            case 0:
                selectTexts[0].text = "Equiped";
                selectTexts[1].text = "Select";
                selectTexts[2].text = "Select";
                break;

            case 1:
                selectTexts[0].text = "select";
                selectTexts[1].text = "Equiped";
                selectTexts[2].text = "Select";
                break;

            case 2:
                selectTexts[0].text = "select";
                selectTexts[1].text = "Select";
                selectTexts[2].text = "Equiped";
                break;
        }
    }
}
