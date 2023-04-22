using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public bool can_move_toward(Vector3Int position, Vector3Int dir) {
        Vector3Int pos_check = position + dir;
        
        if (Utils.wall_is_at_position(pos_check)) {
            return false;
        }

        Mover mover = Utils.get_mover_at_position(pos_check);

        if (mover != null) {
            if (!mover.can_move_toward(pos_check, dir)) {
                return false;
            }
        } else {
            return true;
        }

        return true;
    }

    public void move(Vector3Int dir) {
        transform.position += dir;
    }
    
    public void start_move(Vector3Int position, Vector3Int dir) {
        move(dir);
        
        Vector3Int pos_move = position + dir;
        Mover mover = Utils.get_mover_at_position(pos_move);
        if (mover != null) {
            mover.start_move(pos_move, dir);
        }
    }


    void Start() {
        
    }

    void Update() {
        
    }
}
