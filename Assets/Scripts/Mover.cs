using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float push_duration;
    
    public bool can_push_toward(Vector3Int position, Vector3Int dir) {
        Vector3Int pos_check = position + dir;
        
        if (Utils.wall_is_at_position(pos_check)) {
            return false;
        }

        Mover mover = Utils.get_mover_at_position(pos_check);

        if (mover != null) {
            if (!mover.can_push_toward(pos_check, dir)) {
                return false;
            }
        } else {
            return true;
        }

        return true;
    }

    public void push(Vector3Int dir) {
        transform.position += dir;
    }
    
    public void start_push(Vector3Int position, Vector3Int dir) {
        push(dir);
        
        Vector3Int pos_move = position + dir;
        Mover mover = Utils.get_mover_at_position(pos_move);
        if (mover != null) {
            mover.start_push(pos_move, dir);
        }
    }
}
