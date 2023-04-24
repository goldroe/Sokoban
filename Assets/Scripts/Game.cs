using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public List<Mover> movers = new List<Mover>();
    public Dictionary<Vector3Int, HashSet<GameObject>> grid;
    // The name of the scene you want to load.
    [SerializeField] private string sceneToLoad = "PopUpScene";

    private static Game instance_ref;
    public static Game instance
    {
        get
        {
            if (instance_ref == null)
            {
                instance_ref = FindObjectOfType<Game>();
            }
            return instance_ref;
        }
    }

    public void populate_grid()
    {
        grid = new Dictionary<Vector3Int, HashSet<GameObject>>();

        var tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tiles)
        {
            Vector3Int position = Vector3Int.RoundToInt(tile.transform.position);
            if (!grid.ContainsKey(position))
            {
                grid[position] = new HashSet<GameObject>();
            }
            grid[position].Add(tile);
        }
    }

    public HashSet<GameObject> get_tiles_at_position(Vector3Int position)
    {
        HashSet<GameObject> tiles = grid[position];
        return tiles;
    }

    public T get_object_at_position<T>(Vector3Int position)
    {
        foreach (var tile in get_tiles_at_position(position))
        {
            var obj = tile.GetComponentInParent<T>();
            if (obj != null)
                return obj;
        }
        return default;
    }

    public Wall get_wall_at_position(Vector3Int position)
    {
        Wall wall = get_object_at_position<Wall>(position);
        return wall;
    }

    public bool wall_is_at_position(Vector3Int position)
    {
        Wall wall = get_wall_at_position(position);
        return wall != null;
    }

    void Awake()
    {
        instance_ref = this;
    }

    void Start()
    {
        populate_grid();
    }

    void Update()
    {
        populate_grid();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        bool all_targets = true;
        for (int i = 0; i < targets.Length; i++)
        {
            GameObject target = targets[i];
            bool is_filled = target.GetComponentInParent<Target>().filled;
            if (!is_filled)
            {
                all_targets = false;
                break;
            }
        }

        if (all_targets)
        {
            LoadSceneOnEvent();

            Debug.Log("YOU WON HOORAY");
        }
    }

    public void LoadSceneOnEvent()
    {
        SceneManager.LoadScene(sceneToLoad);

    }

}