using UnityEngine;
using UnityEngine.AI;

public class RandomWalk : MonoBehaviour {

    // Size of the grid horizontally
    [SerializeField]
    private int gridWidth;

    // Size of the grid vertically
    [SerializeField]
    private int gridHeight;

    // Number of tunnels
    [SerializeField]
    private int tunnelNumber;

    // Tunnel length max
    [SerializeField]
    private int tunnelLengthMax;

    // Reference to the prefab for impassable ties
    [SerializeField]
    GameObject tileImpassable;

    // Reference to the prefab for passable tiles
    [SerializeField]
    GameObject tilePassable;

    [SerializeField]
    GameObject tileCarpetPassable;

    // Reference to the player game object
    [SerializeField]
    private GameObject playerObject;

    // Tile Heights
    private const float heightImpassable = 0.0f;
    private const float heightPassable = -0.5f;

    // An in-memory map of passable and impassable areas
    private TileTypes[,] map;

    // Reference to start tile for player object
    private GameObject playerSpawn;

    /// <summary>
    /// Initialisation
    /// </summary>
    void Start() {
        // Map will be initialised to all zeros, as that is the
        // default value for shorts. To make life easy, we'll decide
        // that zero is going to be our impassable value
        map = new TileTypes[gridWidth, gridHeight];

        // Call these functions
        RandomWalker();
        DrawMapWithPrefabs();
        DrawWallPrefabs();
        GenerateNavMesh();
        SpawnPlayer();
    }

    /// <summary>
    /// Spawns the player onto the saved reference for the first
    /// generated passable tile
    /// </summary>
    void SpawnPlayer() {
        GameObject go = Instantiate(playerObject, playerSpawn.transform.position + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);

        go.AddComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Generates the navmesh on the generated maze
    /// </summary>
    void GenerateNavMesh() {
        //Add the navmesh surface component
        NavMeshSurface nm = gameObject.AddComponent<NavMeshSurface>();

        //Tell it to bake a navmesh for us
        nm.BuildNavMesh();
    }

    /// <summary>
    /// This method "draws" our map from 2D, into a 3D floor. Since we're using
    /// our map as a floor map, we turn Y into Z to position cubes.
    /// 
    /// We use a switch to decide what to do for each tile type, this makes this
    /// system a little extensible, i.e. you might add a pickup tile type, and for
    /// that one, instantiate the floor, but then also instantiate a pickup above
    /// the floor.
    /// </summary>
    void DrawMapWithPrefabs() {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                switch (map[x, y])
                {
                    case TileTypes.Passable:
                        {
                            MarkWalls(x, y);

                            if (map[x, y] != TileTypes.Passable)
                                break;

                            // Instantiate tile and get a reference to the new tiles gameobject
                            GameObject go = Instantiate(tilePassable, new Vector3(x, heightPassable, y), Quaternion.identity);

                            // If we haven't already got a player spawn point, make this it.
                            playerSpawn = playerSpawn ?? go;

                            // Set the object to be a child of this gameobject - keeps the inspector neat and
                            // lets us stick a navmesh on them all.
                            go.transform.parent = transform;

                            
                            break;
                        }
                }
            }
        }

    }

    void DrawWallPrefabs()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                switch (map[x, y])
                {
                    case TileTypes.RenderImpassable:
                        {
                            // Instantiate tile and get a reference to the new tiles gameobject
                            GameObject go = Instantiate(tileImpassable, new Vector3(x, heightImpassable, y), Quaternion.identity);
                            // Set the object to be a child of this gameobject - keeps the inspector neat and
                            // lets us stick a navmesh on them all.
                            go.transform.parent = transform;
                            break;
                        }
                }
            }
        }
    }


    void MarkWalls(int x, int y)
    {
        int i = 0;

        //if x is less than the gridWidth
        if (x < gridWidth)
        {
            switch (map[x + 1, y])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x + 1, y] = TileTypes.RenderImpassable;                 
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }

            switch(map[x + 1, y + 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x + 1, y + 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }
        }

        //if x is greater than 0
        if (x > 0)
        {
            switch (map[x - 1, y])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x - 1, y] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }

            switch (map[x - 1, y - 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x - 1, y - 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }
        }

        //if y is less than gridHeight
        if (y < gridHeight)
        {
            switch (map[x, y + 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x, y + 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }

            }

            switch (map[x - 1, y + 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x - 1, y + 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }
        }

        //if y is greater than 0
        if (y > 0)
        {
            switch (map[x, y - 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x, y - 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }

            switch (map[x + 1, y - 1])
            {
                case TileTypes.Impassable:
                    {
                        i++;
                        map[x + 1, y - 1] = TileTypes.RenderImpassable;
                        break;
                    }

                case TileTypes.RenderImpassable:
                    {
                        i++;
                        break;
                    }
            }
        }

        switch (map[x, y])
        {
            case TileTypes.Passable:
                {
                    if (i == 0)
                        map[x, y] = TileTypes.CarpetPassable;

                    break;
                }
        }

        switch (map[x, y])
        {
            case TileTypes.CarpetPassable:
                {
                    GameObject go = Instantiate(tileCarpetPassable, new Vector3(x, heightPassable, y), Quaternion.identity);

                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                    break;
                }
        }
    }

    /// <summary>
    /// This is a random walk algorithm, implemented to use a helper method
    /// </summary>
    void RandomWalker() {
        // Pick a random starting point on the grid (which is just an array in
        // memory)
        Vector2 position = new Vector2(
            Random.Range(1, gridWidth - 1),
            Random.Range(1, gridHeight - 1)
        );

        for (int i = 0; i < tunnelNumber; i++) {
            // Vertical or horizontal tunnel?
            // We set vertical to true, if a random float (0<x<1) is less
            // than 0.5f, and false if it is greater than 0.5f
            bool vertical = Random.value < 0.5f;

            // Similarly, we set direction to 1 if a random float is less
            // than 0.5f, and -1 if it is greater than 0.5f
            float direction = Random.value < 0.5f ? 1 : -1;

            // If we're moving vertically, generate an appropriate tunnel
            // length (max is set from unity inspector) and then
            // create a tunnel that is that long (if we don't hit a boundary)
            // and moving in the appropriate positive or negative direction
            // Similarly, if we're moving horizontall, we generate that tunnel
            // in the same way. Each time we generate a tunnel a new position
            // is returned, this is the position of our walk progressing.
            if (vertical) {
                int tunnelLength = Random.Range(0, tunnelLengthMax);
                CreateTunnel(ref position, tunnelLength, new Vector2(0, direction));
            } else {
                int tunnelLength = Random.Range(0, tunnelLengthMax);
                CreateTunnel(ref position, tunnelLength, new Vector2(direction, 0));
            }
        }
    }

    /// <summary>
    /// Creates the tunnel defined by starting position, length and direction.
    /// </summary>
    /// <returns>Updated current position.</returns>
    /// <param name="position">Starting position.</param>
    /// <param name="length">Length of desired tunnel.</param>
    /// <param name="direction">Direction of desired tunnel.</param>
    void CreateTunnel(ref Vector2 position, int length, Vector2 direction) {
        // This is a complicated for loop, but it's not as bad as it at first looks
        // let's break it down into three segments and it should become clearer.

        // int x = (int)position.x, i = 0 : This segment starts our X off at the X position
        // We have to cast to int with (int) otherwise the compiler won't let us store
        // position.x which is a float type, in x, which is an int. It also creates a i
        // variable, which we'll use to track our tunnel length.

        // i < length && x < gridWidth - 1 && x > 0 : this segment is basically three checks in one
        // we want to loop whilst i is less than length, AND x is less than gridWidth, AND x is
        // greater than zero. If any of those conditions cease to be true, we want to stop looping.

        // x += (int) direction.x, i++ : finally, if we're moving in vertical axis, direction.x will be 0
        // so we can safely add direction.x, to get our direction - it will be 0, -1 or 1. We can simply
        // increment i, as we're using that to track length independant of direction

        // The second loop is exactly the same, except over the Y axis, and using j to track length.

        for (int x = (int)position.x, i = 0; i < length && x < gridWidth - 1 && x > 0; x += (int)direction.x, i++) {
            map[x, (int)position.y] = TileTypes.Passable; // Update appropriate map tile to walkable

            // Update current position
            position.x = x;
        }

        for (int y = (int)position.y, j = 0; j < length && y < gridHeight - 1 && y > 0; y += (int)direction.y, j++) {
            map[(int)position.x, y] = TileTypes.Passable; // Update appropriate map tile to walkable

            // Update current position
            position.y = y;
        }
    }
}
