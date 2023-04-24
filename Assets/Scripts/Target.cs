using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool filled = false;
    void OnTriggerEnter(Collider collider) {
        filled = true;
    }
    void OnTriggerExit() {
        filled = false;
    }
}
