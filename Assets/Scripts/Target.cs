using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool filled = false;
    void OnTriggerEnter(Collider collider) {
        GameObject obj = collider.transform.parent.gameObject;
        if (obj.CompareTag("Block")) {
            filled = true;
        }   
    }
    void OnTriggerExit() {
        filled = false;
    }
}
