using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public AudioSource audio_source;

    public bool is_pushing;
    public bool is_turning;

    private float time_elapsed;
    public float push_duration;
    public float turn_duration;

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
        //dir.x = (int)Mathf.Cos(Mathf.Deg2Rad * angle);
        //dir.z = (int)Mathf.Sin(Mathf.Deg2Rad * angle);
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
        
        // if (facing_direction.x != 0 && (Mathf.Abs(dir.x) != Mathf.Abs(facing_direction.x))
        //     ||
        //     facing_direction.z != 0 && (Mathf.Abs(dir.z) != Mathf.Abs(facing_direction.z))
        //     ) {
        //     return true;
        // }

        return false;
    }

    /*
        Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
     */

    bool can_turn_player(Vector3Int dir) {
        Vector3Int facing_direction = get_direction_from_angle(current_angle);
        Vector3Int fork_pos = get_fork_tile_position();
        Vector3Int base_pos = get_base_tile_position();
        
        // check if block next to base on turn direction
        if (Utils.get_wall_at_position(base_pos + dir) || Utils.get_mover_at_position(base_pos + dir)) {
            Debug.Log("Can't turn due to block adjacent to base");
            return false;
        }

        Wall wall = Utils.get_wall_at_position(fork_pos + dir);
        if (wall != null) {
            Debug.Log("Can't push wall");
            return false;
        }
        // check if block next to fork can move on turn
        Mover mover = Utils.get_mover_at_position(fork_pos + dir);
        if (mover != null && !can_push_toward(fork_pos + dir, dir)) {
            Debug.Log("Can't push block on new fork for turn");
            return false;
        }

        return true;
    }

    void try_push(Vector3Int pos, Vector3Int dir) {
        if (can_push_toward(pos, dir)) {
            start_push(pos, dir);
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
            Debug.Log("Direction: " + dir_angle);
            if (dir_angle == current_angle) {
                Debug.Log("Moving forward");
                try_push(get_fork_tile_position(), dir);
            } else if (Mathf.Abs(dir_angle - current_angle) == 180) {
                Debug.Log("Moving backward");
                try_push(get_base_tile_position(), dir);
            } else {
                Debug.Log("Trying turn");
                try_turn(dir);
            }
        }
    }
    
    void test_do() {
        Vector3Int dir = Vector3Int.zero;

        if (can_input()) {
             dir = get_input_direction();
        } else {
            // queue moves
        }
        
        if (dir != Vector3Int.zero) {
            float dir_angle = get_angle_from_direction(dir);
            Debug.Log("Direction Angle: " + dir_angle);
            // forward or behind current direction
            if (dir_angle == current_angle) {
                Debug.Log("try move forward direction");
                try_push(get_fork_tile_position(), dir);
            } else if (Mathf.Abs(dir_angle - current_angle) == 180) {
                Debug.Log("try move backward direction");
                try_push(get_base_tile_position(), dir);
            } else {
                try_turn(dir);
                // StartCoroutine(turn_player(target_angle));
                // current_angle += target_angle;
                // current_angle %= 360; // wrap to [0, 360]
                // current_rotation += new Vector3(0, target_angle, 0);
                // transform.eulerAngles = current_rotation;
            }
        }
    }

    IEnumerator turn_player(Vector3Int dir) {
        Vector3Int fork_pos = get_fork_tile_position();
        Mover mover = Utils.get_mover_at_position(fork_pos + dir);

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

        if (mover != null) {
            Debug.Log("Pushing block next to fork");
            mover.start_push(fork_pos + dir, dir);
        }
        
        transform.rotation = target_rotation;
        is_turning = false;
        current_angle += target_angle;
        current_angle %= 360; // wrap to [0, 360]
    }
}
