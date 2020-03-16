using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Dialogue[] dialogue;

    [Header("Sentences Variables")]
    [TextArea(3, 10)]
    public string[] sentences;
    [SerializeField]private int currentSentenceInt;
    public int numberOfSentences;
    bool ableToContinue = false;

    [Header("Text and Gameobject Variables")]
    public Text dialogText;
    public GameObject tutorial;
    public Animator drSpikyAnim;
    public Animator windMageAnim;
    public GameObject continueButton;

    private void Start()
    {
        StartDialogue();
    }

    private void Update()
    {
        if (ableToContinue == true)
        {
            continueButton.SetActive(true);
        } else
        {
            continueButton.SetActive(false);
        }

        //SwitchDialog();
    }

    /*public void SwitchDialog()
    {
        //use this for guidence
        /*drSpikyAnim.SetBool("Talking", true); //Dr Spikey is talking (This is for start)
        windMageAnim.SetBool("Talking", true); //Wind Mage is talking (This is for start)
        StartCoroutine(DrSpikeyTalk()); //Dr Spikey is Talking 
        StartCoroutine(WindMageTalk()); //Wind Mage is Talking  */

        /*switch (dialogue[0].toPlay)
        {
            case 0:
                windMageAnim.SetBool("Talking", true); //Wind Mage is talking
                break;

            case 1:
                StartCoroutine(DrSpikeyTalk());
                break;

            case 2:
                StartCoroutine(WindMageTalk());
                break;

            default:
                EndDialog();
                break;
        }
    }*/

    public void StartDialogue()
    {
        dialogText.text = sentences[currentSentenceInt];
        LevelManager.runGame = false;
        StartCoroutine(TypeSentence(sentences));
    }

    //display new sentence
    public void DisplayNextSentence()
    {
        //check if the current Sentence Int is less than amt of arrays needed, if so play 1 more sentence
        if (currentSentenceInt < numberOfSentences)
        {
            //StartCoroutine(TypeSentence(sentences));
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
        ableToContinue = false;

        dialogText.text = "";
        foreach (char letter in sentence[currentSentenceInt].ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }

        ableToContinue = true;
    }

    IEnumerator WindMageTalk()
    {
        drSpikyAnim.SetBool("Talking", false); //dr spikey is talking

        yield return new WaitForSeconds(Time.deltaTime);

        windMageAnim.SetBool("Talking", true); //Wind Mage is talking
    }

    IEnumerator DrSpikeyTalk()
    {
        windMageAnim.SetBool("Talking", false); //Wind Mage is talking

        yield return new WaitForSeconds(Time.deltaTime);

        drSpikyAnim.SetBool("Talking", true); //Dr Spikey is talking
    }

    public void EndDialog()
    {
        LevelManager.runGame = true;
        tutorial.SetActive(false);
    }
}
