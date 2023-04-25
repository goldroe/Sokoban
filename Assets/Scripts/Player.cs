using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public AudioSource audio_source;

    public bool is_turning;
    public const float turn_duration = 0.22f;
    
    public float current_angle; // degrees around y axis

    public Vector3Int get_base_tile_position() {
        return Vector3Int.RoundToInt(transform.position);
    }

    public Vector3Int get_fork_tile_position() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles) {
            if (tile.name == "Fork") {
                Vector3Int fork_pos = vector_to_int(tile.transform.position);
                return fork_pos;
            }
        }
        return default;
    }
    
    public Vector3Int vector_to_int(Vector3 v) {
        return Vector3Int.RoundToInt(v);
    }

    public Vector3Int get_input_direction() {
        Vector3Int dir = Vector3Int.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            dir = Vector3Int.forward;
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            dir = Vector3Int.left;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            dir = Vector3Int.back;
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            dir = Vector3Int.right;
        }
        return dir;
    }

    // for angle rotation on Y axis
    public Vector3Int get_direction_from_angle(float angle) {
        Vector3Int dir = Vector3Int.zero;
        // have to do this because angles are fake and the z axes is reversed?
        if (angle == 90) {
            dir.z = -1;
        } else if (angle == 180) {
            dir.x = -1;
        } else if (angle == 270) {
            dir.z = 1;
        } else if (angle == 0 || angle == 360) {
            dir.x = 1;
        }
        return dir;
    }

    // replace with atan2
    public float get_angle_from_direction(Vector3Int dir) {
        if (dir == Vector3Int.forward) {
            return 270;
        } else if (dir == Vector3Int.left) {
            return 180;
        } else if (dir == Vector3Int.back) {
            return 90;
        } else if (dir == Vector3Int.right) {
            return 0;
        }

        return -1;
        // float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
    }

    bool turn_is_needed(Vector3Int dir) {
        float turn_angle = get_angle_from_direction(dir);
        if (Mathf.Abs(current_angle - turn_angle) != 180) {
            return true;
        }
        
        return false;
    }

    bool can_turn_player(Vector3Int dir) {
        Vector3Int facing_direction = get_direction_from_angle(current_angle);
        Vector3Int fork_pos = get_fork_tile_position();
        Vector3Int base_pos = get_base_tile_position();
        
        // check if block next to base on turn direction
        if (Utils.get_wall_at_position(base_pos + dir) || Utils.get_mover_at_position(base_pos + dir)) {
            return false;
        }

        Wall wall = Utils.get_wall_at_position(fork_pos + dir);
        if (wall != null) {
            return false;
        }
        // check if block next to fork can move on turn
        Mover mover = Utils.get_mover_at_position(fork_pos + dir);
        if (mover != null && !can_push_toward(fork_pos + dir, dir)) {
            return false;
        }

        return true;
    }

    void try_push(Vector3Int pos, Vector3Int dir) {
        if (can_push_toward(pos, dir)) {
            StartCoroutine(start_push(pos, dir));
        } else {
            
        }
    }

    void try_turn(Vector3Int dir) {
        float dir_angle = get_angle_from_direction(dir);
        float target_angle = dir_angle - current_angle;
        if (can_turn_player(dir)) {
            StartCoroutine(turn_player(dir));
        } else {
            
        }
    }
    
    bool can_input() {
        return (!is_pushing && !is_turning);
    }
    
    void Start() {
        // audio_source = GetComponent<AudioSource>();
        current_angle = transform.rotation.eulerAngles.y; // init based on rotation in scene
    }

    void Update() {
        Vector3Int dir = Vector3Int.zero;
        if (can_input()) {
            dir = get_input_direction();
        } else {
            // queue input
        }
        
        if (dir != Vector3Int.zero) {
            float dir_angle = get_angle_from_direction(dir);
            float target_angle = dir_angle - current_angle;
            if (dir_angle == current_angle) {
                try_push(get_fork_tile_position(), dir);
            } else if (Mathf.Abs(dir_angle - current_angle) == 180) {
                try_push(get_base_tile_position(), dir);
            } else {
                try_turn(dir);
            }
        }
    }
    
    IEnumerator turn_player(Vector3Int dir) {
        Vector3Int fork_pos = get_fork_tile_position();
        Mover mover = Utils.get_mover_at_position(fork_pos + dir);
        
        if (mover != null) {
            Debug.Log("Pushing block next to fork");
            StartCoroutine(mover.start_push(fork_pos + dir, dir));
        }
        
        float dir_angle = get_angle_from_direction(dir);
        float target_angle = dir_angle - current_angle;
        
        is_turning = true;
        float time_elapsed = 0;

        Quaternion start_rotation = transform.rotation;
        Quaternion target_rotation = transform.rotation * Quaternion.Euler(0, target_angle, 0);

        while (time_elapsed < turn_duration) {
            transform.rotation = Quaternion.Slerp(start_rotation, target_rotation, time_elapsed / turn_duration);
            time_elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = target_rotation;
        is_turning = false;
        current_angle += target_angle;
        current_angle %= 360; // wrap to [0, 360]
    }
}
