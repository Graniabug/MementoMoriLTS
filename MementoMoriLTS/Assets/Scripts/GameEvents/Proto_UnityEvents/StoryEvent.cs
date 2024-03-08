using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;

public class StoryEvent : MonoBehaviour
{
    public bool useAudio = false;
    [HideInInspector]
    public List<AudioClip> audio;
    public bool useAnimation = false;
    [HideInInspector]
    public List<Animation> animation;
    public UnityEvent events;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExecuteEvent()
    {
        if (useAudio)
        {
            foreach (AudioClip ac in audio)
            {
                //get audio source
                //assign audio clip 
                //play the audio
            }
        }

        if (useAnimation)
        {
            foreach (Animation anim in animation)
            {
                anim.gameObject.SetActive(true);
                anim.enabled = true;
                anim.Play();
            }
        }

        if (events != null)
        {
            events.Invoke();
        }
    }
}

[CustomEditor(typeof(StoryEvent))]
public class StoryEventEditor : Editor
{
    SerializedProperty audioBool;
    SerializedProperty animationBool;
    SerializedProperty audioList;
    SerializedProperty animationList;

    private void OnEnable()
    {
        // hook up the serialized properties
        audioBool = serializedObject.FindProperty(nameof(StoryEvent.useAudio));
        animationBool = serializedObject.FindProperty(nameof(StoryEvent.useAnimation));
        audioList = serializedObject.FindProperty(nameof(StoryEvent.audio));
        animationList = serializedObject.FindProperty(nameof(StoryEvent.animation));
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        // update the current values into the serialized object and propreties
        serializedObject.Update();

        // if the first bool is true
        if (audioBool.boolValue)
        {
            // draw the second bool field
            EditorGUILayout.PropertyField(audioList);
        }

        // if the first bool is true
        if (animationBool.boolValue)
        {
            // draw the second bool field
            EditorGUILayout.PropertyField(animationList);
        }

        // Write back changed values
        // This also handles all marking dirty, saving, undo/redo etc
        serializedObject.ApplyModifiedProperties();
    }
}
