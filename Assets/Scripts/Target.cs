using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool filled = false;
    void OnTriggerEnter(Collider collider) {
        Debug.Log("TRIGGER ENTER");
        filled = true;
    }
    void OnTriggerExit() {
        Debug.Log("TRIGGER EXIT");
        filled = false;
    }
}
