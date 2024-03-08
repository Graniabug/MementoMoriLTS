using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/*
 * Needs:
 * Safety
 * Bug fixes
 * Comments
 */

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public string newEnterName = null;

    [HideInInspector] public bool paramSet = false;

    public enum WhoEntered
    {
        galeEntered,
        kieranEntered,
        objectEntered
    }

    public WhoEntered type;
    [HideInInspector]
    public GameObject obj;

    private void Start()
    {
        this.gameObject.GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case WhoEntered.galeEntered:
                if (other.gameObject == SceneManager.Instance.Gale)
                {
                    Debug.Log("Gale entered the trigger zone of " + this.gameObject.name, this.gameObject);
                    SceneManager.Instance.eventStateMachine_Anim.SetBool(newEnterName, true);
                }
                break;
            case WhoEntered.kieranEntered:
                if (other.gameObject == SceneManager.Instance.Kieran)
                {
                    Debug.Log("Kieran entered the trigger zone of " + this.gameObject.name, this.gameObject);
                    SceneManager.Instance.eventStateMachine_Anim.SetBool(newEnterName, true);
                }
                break;
            case WhoEntered.objectEntered:
                if (other.gameObject == obj)
                {
                    Debug.Log(obj.name + " entered the trigger zone of " + this.gameObject.name, this.gameObject);
                    SceneManager.Instance.eventStateMachine_Anim.SetBool(newEnterName, true);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " exited the trigger zone of " + this.gameObject.name, this.gameObject);
        SceneManager.Instance.eventStateMachine_Anim.SetBool(newEnterName, false);
    }
}

[CustomEditor(typeof(TriggerEvent))]
public class TriggerEventEditor : Editor
{
    SerializedProperty objRef;

    private void OnEnable()
    {
        // hook up the serialized properties
        objRef = serializedObject.FindProperty(nameof(TriggerEvent.obj));
    }

    public override void OnInspectorGUI()
    {
        TriggerEvent script = (TriggerEvent)target;

        if (!script.paramSet)
        {
            // Show default inspector property editor
            DrawDefaultInspector();

            if (script.type == TriggerEvent.WhoEntered.objectEntered)
            {
                EditorGUILayout.PropertyField(objRef);
            }

            if (GUILayout.Button("Add Parameter"))
            {
                if (!string.IsNullOrWhiteSpace(script.newEnterName) && !SceneManager.Instance.HasParameter(script.newEnterName, SceneManager.Instance.eventStateMachine_Anim))
                {
                    SceneManager.Instance.eventStateMachine_AC.AddParameter(script.newEnterName, AnimatorControllerParameterType.Bool);
                    script.paramSet = true;
                }
                else
                {
                    Debug.LogError("Cannot add parameter as no name was given or the name is the same as one that already exists.", script.gameObject);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
        else //make the parameter name uneditable if it has been set
        {
            EditorGUILayout.LabelField("Parameter set to:");
            EditorGUILayout.LabelField(script.newEnterName);

            switch (script.type)
            {
                case TriggerEvent.WhoEntered.galeEntered:
                    EditorGUILayout.LabelField("It will trigger when Gale enters the zone.");
                    break;
                case TriggerEvent.WhoEntered.kieranEntered:
                    EditorGUILayout.LabelField("It will trigger when Kieran enters the zone.");
                    break;
                case TriggerEvent.WhoEntered.objectEntered:
                    EditorGUILayout.LabelField("It will trigger when the following object enters the zone: " + script.obj);
                    break;
            }

        }
    }
}
