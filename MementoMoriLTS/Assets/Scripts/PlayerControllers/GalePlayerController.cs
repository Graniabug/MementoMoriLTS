using UnityEngine;
using UnityEngine.InputSystem;

public class GalePlayerController : PlayerController
{

    private new void Start()
    {
        base.Start();
        base.companion = SceneManager.Instance.Kieran.GetComponent<PlayerController>();
    }
    void Update()
    {
        base.DoPlayerMovement();
    }

    public new void OnCharacterSwitch(InputAction.CallbackContext context)
    {
        SceneManager.Instance.eventStateMachine_Anim.SetBool("KieranHost", true);
        SceneManager.Instance.eventStateMachine_Anim.SetBool("GaleHost", false);
        base.OnCharacterSwitch(context);
    }
}
