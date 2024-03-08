using UnityEngine;
using UnityEngine.InputSystem;

public class KieranPlayerController : PlayerController
{ 
    private new void Start()
    {
        base.Start();
        companion = SceneManager.Instance.Gale.GetComponent<PlayerController>();
    }
    void Update()
    {
        base.DoPlayerMovement();
    }

    public new void OnCharacterSwitch(InputAction.CallbackContext context)
    {
        SceneManager.Instance.eventStateMachine_Anim.SetBool("KieranHost", false);
        SceneManager.Instance.eventStateMachine_Anim.SetBool("GaleHost", true);
        base.OnCharacterSwitch(context);
    }
}

