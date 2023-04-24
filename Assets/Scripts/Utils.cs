using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils {
    public static HashSet<GameObject> get_tiles_at_position(Vector3Int position) {
        HashSet<GameObject> tiles = new HashSet<GameObject>();
        if (Game.instance.grid.ContainsKey(position)) { 
            tiles = Game.instance.grid[position];
        }

        return tiles;
    }

    public static T get_object_at_position<T>(Vector3Int position) {
        foreach (var tile in get_tiles_at_position(position)) {
            var obj = tile.GetComponentInParent<T>();
            if (obj != null)
                return obj;
        }
        return default;
    }

    public static Wall get_wall_at_position(Vector3Int position) {
        Wall wall = get_object_at_position<Wall>(position);
        return wall;
    }

    public static Mover get_mover_at_position(Vector3Int position) {
        Mover mover = get_object_at_position<Mover>(position);
        return mover;
    }
    
    public static bool wall_is_at_position(Vector3Int position) {
        Wall wall = get_wall_at_position(position);
        return wall != null;
    }

    public static Vector3 to_vector3(Vector3Int v) {
        return new Vector3(v.x, v.y, v.z);
    }
}
