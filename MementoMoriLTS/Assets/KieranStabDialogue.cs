using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KieranStabDialogue : StateMachineBehaviour
{
    [SerializeField]
    private string dialogue;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().kieranTextBox.text = "";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().PrintandPlayDialogue(dialogue, animator.gameObject.GetComponent<DialogueManger>().kieranTextBox, "Kieran");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<DialogueManger>().kieranTextBox.text = dialogue;
    }
}
