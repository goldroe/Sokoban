using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public AudioSource audio_source;
    
    public Vector3Int Pos() {
        return Vector3Int.RoundToInt(transform.position);
    }

    public Vector3Int calculate_input_direction() {
        Vector3Int dir = Vector3Int.zero;
        if (Input.GetKeyDown(KeyCode.W)) {
            dir = new Vector3Int(0, 0, 1);
        } else if (Input.GetKeyDown(KeyCode.A)) {
            dir = new Vector3Int(-1, 0, 0);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            dir = new Vector3Int(0, 0, -1);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            dir = new Vector3Int(1, 0, 0);
        }
        return dir;
    }

    void Start() {
        audio_source = GetComponent<AudioSource>();
    }

    
    void Update() {
        Vector3Int dir = calculate_input_direction();
        if (dir != Vector3Int.zero) {
            if (can_move_toward(Pos(), dir)) {
                start_move(Pos(), dir);
                
                audio_source.Play();
            }
        }
    }
}
