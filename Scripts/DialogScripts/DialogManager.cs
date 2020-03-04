using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Sentences Variables")]
    [TextArea(3, 10)]
    public string[] sentences;
    int currentSentenceInt;

    [Header("Text and Gameobject Variables")]
    public Text dialogText;
    public GameObject tutorial;

    private void Start()
    {
        StartDialogue();
    }

    private void Update()
    {
        //this works
        if (Input.GetKey(KeyCode.S))
        {
            Time.timeScale = 0f;
        } else if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 1f;
        }
    }

    public void StartDialogue()
    {
        dialogText.text = sentences[currentSentenceInt];
        StartCoroutine(TypeSentence(sentences));
    }

    //display new sentence
    public void DisplayNextSentence()
    {
        //check if the current Sentence Int is less than amt of arrays needed, if so play 1 more sentence
        if (currentSentenceInt < 2)
        {
            dialogText.text = sentences[currentSentenceInt += 1];
            StartCoroutine(TypeSentence(sentences));
        } else
        {
            EndDialog();
        }
    }

    //type the sentence words 
    IEnumerator TypeSentence(string[] sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence[currentSentenceInt].ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    public void EndDialog()
    {
        tutorial.SetActive(false);
    }
}
