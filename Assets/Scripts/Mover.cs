using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public bool is_pushing;
    public const float push_duration = 0.22f;

    public void round_position() {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
    }
    
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

    public List<Mover> get_movers_for_push(Vector3Int position, Vector3Int dir) {
        List<Mover> movers = new List<Mover>();
        
        for (;;) {
            Mover mover = Utils.get_mover_at_position(position);
            if (mover != null) {
                movers.Add(mover);
            } else {
                break;
            }
            
            position += dir;
        }

        return movers;
    }
    
    public IEnumerator start_push(Vector3Int position, Vector3Int dir) {
        is_pushing = true;
        List<Mover> movers = get_movers_for_push(position, dir);
        // Lists<Vector3> positions = new List<Vector3>()
        
        float time_elapsed = 0;
        while (time_elapsed < push_duration) {
            Vector3 dist = Utils.to_vector3(dir) * (Time.deltaTime / push_duration);
            foreach (var mover in  movers) {
                mover.push(dist);
            }
            
            time_elapsed += Time.deltaTime;
            
            yield return null;
        }

        foreach (var mover in movers) {
            mover.round_position();
        }
        
        is_pushing = false;
    }
    
    public void push(Vector3 dist) {
        transform.position += dist;
    }
}
