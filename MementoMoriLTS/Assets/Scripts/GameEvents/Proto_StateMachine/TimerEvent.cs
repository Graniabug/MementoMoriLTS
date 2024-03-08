using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TimerEvent : MonoBehaviour
{
    [HideInInspector] public string newParamName = null;
    public float timer;
    [HideInInspector] public float currentTime = 0.0f;

    [HideInInspector] public bool paramSet = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Timer " + this.gameObject.name + " started!", this.gameObject);
        SceneManager.Instance.eventStateMachine_Anim.SetBool(newParamName, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < timer)
        {
            currentTime += Time.deltaTime;
        }
        else 
        {
            Debug.Log("Timer " + this.gameObject.name + " ended!", this.gameObject);
            SceneManager.Instance.eventStateMachine_Anim.SetBool(newParamName, false);
            this.enabled = false;
        }
    }
}

[CustomEditor(typeof(TimerEvent))]
public class TimerEventEditor : Editor
{
    SerializedProperty paramName;

    private void OnEnable()
    {
        // hook up the serialized properties
        paramName = serializedObject.FindProperty(nameof(TimerEvent.newParamName));
    }
    public override void OnInspectorGUI()
    {
        TimerEvent script = (TimerEvent)target;

        if (script.timer <= 0)
        {
            Debug.LogError("Timer is less than 0.", script.gameObject);
        }

        DrawDefaultInspector();

        if (!script.paramSet)
        {
            EditorGUILayout.PropertyField(paramName);
            if (GUILayout.Button("Add Parameter"))
            {
                if (!string.IsNullOrWhiteSpace(script.newParamName) && !SceneManager.Instance.HasParameter(script.newParamName, SceneManager.Instance.eventStateMachine_Anim))
                {
                    SceneManager.Instance.eventStateMachine_AC.AddParameter(script.newParamName, AnimatorControllerParameterType.Bool);
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
            EditorGUILayout.LabelField(script.newParamName);
        }
    }
}
