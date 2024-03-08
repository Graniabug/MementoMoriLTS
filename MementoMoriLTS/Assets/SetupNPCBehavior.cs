using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupNPCBehavior : StateMachineBehaviour
{
    public string characterName; //NEEDS to match the name for the audio files - maybe later I'll make a dropdown enum for this
    public string startingText;
    public string galeAnswer;
    public string kieranAnswer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().NPCNameBox.text = characterName;
        animator.gameObject.GetComponent<DialogueManger>().NPCTextBox.text = startingText;
        animator.gameObject.GetComponent<DialogueManger>().galeButtonText.text = galeAnswer;
        animator.gameObject.GetComponent<DialogueManger>().kieranButtonText.text = kieranAnswer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().PrintandPlayDialogue(startingText, animator.gameObject.GetComponent<DialogueManger>().NPCTextBox, characterName);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().NPCTextBox.text = startingText;
    }
}
