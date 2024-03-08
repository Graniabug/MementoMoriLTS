using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.Enumeration;
using UnityEditor;
using UnityEditor.Animations;

[RequireComponent(typeof(Animator))]
public class DialogueManger : MonoBehaviour
{
    public GameObject UICanvas;
    public TMP_Text kieranTextBox;
    public TMP_Text galeTextBox;
    public TMP_Text NPCNameBox;
    public TMP_Text NPCTextBox;
    public TMP_Text kieranButtonText;
    public TMP_Text galeButtonText;
    public AudioSource voiceAudioSource;

    public float printDelay = 0.0f; //time in seconds between the letters printing

    private Animator dialogueMachine_Anim;
    private AnimatorController dialogueMachine_AC;
    private string filename = string.Empty;
    private string assetPath;

#if UNITY_EDITOR
    private void OnValidate()
    {
        //get animator controller
        dialogueMachine_Anim = this.gameObject.GetComponent<Animator>();
        if (dialogueMachine_Anim.runtimeAnimatorController != null)
        {
            assetPath = AssetDatabase.GetAssetPath(dialogueMachine_Anim.runtimeAnimatorController);
            dialogueMachine_AC = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);

            //create core loop event parameters
            //if one of these is already in the event machine, it doesn't need to add any of them
            if (!HasParameter("GaleButtonPressed", dialogueMachine_Anim))
            {
                dialogueMachine_AC.AddParameter("GaleButtonPressed", AnimatorControllerParameterType.Bool);
                dialogueMachine_AC.AddParameter("KieranButtonPressed", AnimatorControllerParameterType.Bool);
            }
        }
        else
        {
            Debug.LogWarning("There is no animator controller assigned for " + this.gameObject.name, this.gameObject);
        }
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    { 
        //set animator
        //activate animator
        //center camera
    }

    public IEnumerator PrintandPlayDialogue(string text, TMP_Text textBox, string voice)
    {
        foreach (char c in text)
        {
            yield return new WaitForSeconds(printDelay);
            textBox.text += c;
            filename = voice + "_" + char.ToUpper(c) + ".wav";
            //get that file from the directory
            //assign it to the source
            //play it
        }
    }

    //runs through all the parameters in the given animator to find the one with the given name
    public bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    //runs through all the parameters in the given animator to find the index of the one with the given name
    //returns -1 if not found
    public int GetParameterIndex(string paramName, Animator animator)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].name == paramName)
                return i;
        }
        return -1;
    }

    public void KieranButtonPressed()
    {
        dialogueMachine_Anim.SetBool("KieranButtonPressed", true);
    }

    public void GaleButtonPressed()
    {
        dialogueMachine_Anim.SetBool("GaleButtonPressed", true);
    }
}
