using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class TriggerEvent_V : MonoBehaviour
{
    public string newEnterName = null;

    [HideInInspector] public bool paramSet = false;

    public enum WhoEntered
    {
        galeEntered,
        kieranEntered,
        objectEntered
    }

    //public WhoEntered type;
    [HideInInspector]
    public GameObject obj;

    private void Start()
    {
        this.gameObject.GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (newEnterName != null && newEnterName != string.Empty)
        {
            Debug.Log(other.gameObject.name + " entered the trigger zone of " + this.gameObject.name, this.gameObject);
            SceneManager.Instance.sceneEventVariables.declarations.Set(newEnterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " exited the trigger zone of " + this.gameObject.name, this.gameObject);
        SceneManager.Instance.sceneEventVariables.declarations.Set(newEnterName, false);
    }
}
