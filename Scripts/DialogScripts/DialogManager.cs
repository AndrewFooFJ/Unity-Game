using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Place to put Dialogue Scriptable Object")]
    public Dialogues dialogues;

    [Header("Sentences Variables")]
    int currentSentenceInt;
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
    }

    public void SwitchAnims()
    {
        switch (dialogues.dialogue[currentSentenceInt].toPlay)
        {
            case Dialogue.TypeOfAnims.drSpikyStart:
                drSpikyAnim.SetBool("Talking", true);
                break;

            case Dialogue.TypeOfAnims.mageStart:
                windMageAnim.SetBool("Talking", true);
                break;

            case Dialogue.TypeOfAnims.drSpikyTalk:
                StartCoroutine(DrSpikeyTalk());
                break;

            case Dialogue.TypeOfAnims.mageTalk:
                StartCoroutine(WindMageTalk());
                break;
        }
    }

    #region Dialogue Functions
    public void StartDialogue()
    {
        LevelManager.runGame = false;

        dialogText.text = dialogues.dialogue[currentSentenceInt].sentence;
        StartCoroutine(TypeSentence(dialogues.dialogue[currentSentenceInt].sentence));

        SwitchAnims();
    }

    //display new sentence
    public void DisplayNextSentence()
    {
        //check if the current Sentence Int is less than amt of arrays needed, if so play 1 more sentence
        if (currentSentenceInt < dialogues.dialogue.Length)
        {
            dialogText.text = dialogues.dialogue[currentSentenceInt += 1].sentence;
            StartCoroutine(TypeSentence(dialogues.dialogue[currentSentenceInt].sentence));

            SwitchAnims();
        } else
        {
            EndDialog();
        }
    }

    public void EndDialog()
    {
        LevelManager.runGame = true;
        tutorial.SetActive(false);
    }
    #endregion

    //type the sentence words 
    IEnumerator TypeSentence(string sentence)
    {
        ableToContinue = false;

        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }

        ableToContinue = true;
    }

    #region Animator Functions
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
    #endregion
}
