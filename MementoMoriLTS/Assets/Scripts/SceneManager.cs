using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StateMachine))]
public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    public bool multiplayer = false;
    public GameObject GalePF;
    public GameObject KieranPF;
    public Transform GaleStart;
    public Transform KieranStart;

    [HideInInspector] public GameObject Gale;
    [HideInInspector] public GameObject Kieran;
    #region animator
    [HideInInspector] public AnimatorController eventStateMachine_AC;
    [HideInInspector] public Animator eventStateMachine_Anim;
    #endregion

    public StateMachine sceneEventMachine;
    public Variables sceneEventVariables;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

        }

        #region animator
        //get animator controller
        eventStateMachine_Anim = this.gameObject.GetComponent<Animator>();
        string assetPath = AssetDatabase.GetAssetPath(eventStateMachine_Anim.runtimeAnimatorController);
        eventStateMachine_AC = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);

        //create core loop event parameters
        //if one of these is already in the event machine, it doesn't need to add any of them
        if (!HasParameter("GaleHost", eventStateMachine_Anim))
        {
            eventStateMachine_AC.AddParameter("GaleHost", AnimatorControllerParameterType.Bool); //true if Gale JUST became the player character
            eventStateMachine_AC.AddParameter("KieranHost", AnimatorControllerParameterType.Bool); //true if Kieran JUST became the player character
            eventStateMachine_AC.AddParameter("isMultiplayer", AnimatorControllerParameterType.Bool); //true if the game is currently multiplayer
            eventStateMachine_AC.AddParameter("GaleOutsideDark", AnimatorControllerParameterType.Bool); //true if Gale is unsafe
            eventStateMachine_AC.AddParameter("KieranOutsideLight", AnimatorControllerParameterType.Bool); //true if Kieran is unsafe
        }
        #endregion

        sceneEventMachine = this.gameObject.GetComponent<StateMachine>();
        sceneEventVariables = this.gameObject.GetComponent<Variables>();
    }
#endif

    private void Awake()
    {
    }

    public void Start()
    {
        eventStateMachine_Anim.SetBool("isMultiplayer", multiplayer);

        if (multiplayer) //set up multiplayer controls
        {
            PlayerInputManager.instance.EnableJoining();

            PlayerInputManager.instance.playerPrefab = GalePF;
            Gale = PlayerInputManager.instance.JoinPlayer(-1, -1, "Keyboard", Keyboard.current).gameObject;

            PlayerInputManager.instance.playerPrefab = KieranPF;
            Kieran = PlayerInputManager.instance.JoinPlayer(-1, -1, "Keyboard", Keyboard.current).gameObject;

            Gale.transform.parent = this.gameObject.transform;
            Kieran.transform.parent = this.gameObject.transform;

            if (GaleStart != null)
            {
                Gale.transform.position = GaleStart.position;
                Gale.transform.rotation = GaleStart.rotation;
            }
            else
            {
                Debug.LogError("There is no starting possition assigned for Gale! Assign 'Gale Start' and try again.", this.gameObject);
            }

            if (KieranStart != null)
            {
                Kieran.transform.position = KieranStart.position;
                Kieran.transform.rotation = KieranStart.rotation;
            }
            else
            {
                Debug.LogError("There is no starting possition assigned for Kieran! Assign 'Kieran Start' and try again.", this.gameObject);
            }

            Gale.GetComponent<PlayerInput>().currentActionMap = Gale.GetComponent<PlayerInput>().actions.FindActionMap(nameOrId: "Player1");
            Kieran.GetComponent<PlayerInput>().currentActionMap = Kieran.GetComponent<PlayerInput>().actions.FindActionMap(nameOrId: "Player2");

            Kieran.GetComponent<KieranPlayerController>().isHost = true;
            Gale.GetComponent<GalePlayerController>().isHost = true;

            PlayerInputManager.instance.DisableJoining();
        }
        else //set up single player controls
        {
            Gale = Instantiate(GalePF);
            Kieran = Instantiate(KieranPF);

            Gale.transform.parent = this.gameObject.transform;
            Kieran.transform.parent = this.gameObject.transform;

            Gale.GetComponent<PlayerInput>().currentActionMap = Gale.GetComponent<PlayerInput>().actions.FindActionMap(nameOrId: "DefaultPlayer");
            Kieran.GetComponent<PlayerInput>().currentActionMap = Kieran.GetComponent<PlayerInput>().actions.FindActionMap(nameOrId: "DefaultPlayer");

            Kieran.GetComponent<KieranPlayerController>().isHost = false;
            Gale.GetComponent<GalePlayerController>().isHost = true;

            Kieran.GetComponent<CharacterController>().enabled = false;
            Kieran.GetComponent<PlayerInput>().enabled = false;

            eventStateMachine_Anim.SetBool("KieranHost", false);
            eventStateMachine_Anim.SetBool("GaleHost", true);
        }
    }

    private void Update()
    {
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
}
