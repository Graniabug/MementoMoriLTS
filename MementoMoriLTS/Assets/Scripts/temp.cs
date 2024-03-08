using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    [Range(0, 0.5f)]
    public float _Amount = 0.25f;
    [Range(0, 1)]
    public float normal = 0;
    // Start is called before the first frame update
    void Update()
    {
        Debug.Log(Mathf.Lerp(_Amount, normal, 0.5f + _Amount));
        /*if (normal > 0.5)
        {
            Debug.Log(Mathf.Lerp(_Amount, (0.5f + _Amount), normal) + _Amount);
        }
        else if (normal < 0.5)
        {
            Debug.Log(Mathf.Lerp(_Amount, (0.5f + _Amount), normal) - _Amount);
        }
        else
        {
            Debug.Log(Mathf.Lerp(_Amount, (0.5f + _Amount), normal));
        }*/
    }

    float Abs(float val)
    {
        if (val >= 0)
        {
            //initialValue is already not negative, so return itself
            return val;
        }
        else
        {
            return val - (val * 2);
        }
    }

    float percentDifferent(float v1, float v2)
    {
        return Abs(v1 - v2) / ((v1 + v2) / 2);
    }
}
