using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class RecursiveBacktracker : MonoBehaviour {

    // Size of the grid horizontally
    [SerializeField]
    private int gridWidth;

    // Size of the grid vertically
    [SerializeField]
    private int gridHeight;

    [SerializeField]
    GameObject WallSegmentHorizontal;

    [SerializeField]
    GameObject WallSegmentVertical;

    [SerializeField]
    GameObject FloorSegment;

    [SerializeField]
    GameObject FloorSegmentVisited;

    public float a = 1103515245;
    public float c = 12345;
    public float m = 2147483648; // aka 2^31

    private float randomNum;

    // Tile Heights
    private const float heightWall = 0.0f;
    private const float heightPath = -0.5f;

    private Node[,] map;
    

    void Start()
    {
        ResetMap();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            ResetMap();
            
        }
    }

    void ResetMap()
    {
        SetUpMap();
        GenerateMaze();
        DrawWallPrefabs();
    }

    void SetUpMap()
    {
        map = new Node[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                map[x, y] = new Node(x, y);
            }
        }

    }

    void DrawWallPrefabs()
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (map[x, y].North)
                {
                    // Instantiate tile and get a reference to the new tiles gameobject
                    GameObject go = Instantiate(WallSegmentHorizontal, new Vector3(x + this.transform.position.x, heightWall, y + 0.5f), WallSegmentHorizontal.transform.rotation);
                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                }

                if (map[x, y].South)
                {
                    // Instantiate tile and get a reference to the new tiles gameobject
                    GameObject go = Instantiate(WallSegmentHorizontal, new Vector3(x + this.transform.position.x, heightWall, y - 0.5f), WallSegmentHorizontal.transform.rotation);
                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                }

                if (map[x, y].East)
                {
                    // Instantiate tile and get a reference to the new tiles gameobject
                    GameObject go = Instantiate(WallSegmentVertical, new Vector3((x + 0.5f) + this.transform.position.x, heightWall, y), WallSegmentVertical.transform.rotation);
                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                }

                if (map[x, y].West)
                {
                    // Instantiate tile and get a reference to the new tiles gameobject
                    GameObject go = Instantiate(WallSegmentVertical, new Vector3((x - 0.5f) + this.transform.position.x, heightWall, y), WallSegmentVertical.transform.rotation);
                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                }

                if (map[x, y].HasBeenVisited)
                {
                    // Instantiate tile and get a reference to the new tiles gameobject
                    GameObject go = Instantiate(FloorSegmentVisited, new Vector3(x + this.transform.position.x, heightPath, y), Quaternion.identity);
                    // Set the object to be a child of this gameobject - keeps the inspector neat and
                    // lets us stick a navmesh on them all.
                    go.transform.parent = transform;
                }
            }
        }
    }

    void GenerateMaze()
    {
        //Sets inital values for x and y map coords
      //  Random.InitState((int)Random.onUnitSphere.x);
        Random.InitState((int)GenerateRandomSeed());
        int x = Random.Range(1, gridWidth - 1), y = Random.Range(1, gridHeight - 1);

        Node currentNode = map[x, y];

        Stack<Node> nodeStack = new Stack<Node>();

        //Set the currentNode to has been visited = true
        map[currentNode.x, currentNode.y].HasBeenVisited = true;

        bool nodesToVisit = true;

        while (nodesToVisit == true)
        { 
            Node nextNode = CheckNeighbours(currentNode);

            if (nextNode != null)
            {
                map[nextNode.x, nextNode.y].HasBeenVisited = true;

                RemoveWalls(currentNode, nextNode);

                nodeStack.Push(currentNode);

                currentNode = nextNode;
            }

            else if (nodeStack.Count > 0)
            {
                currentNode = nodeStack.Pop();
            }

            else
                nodesToVisit = false;

        }

    }

    void RemoveWalls(Node _currentNode, Node _nextNode)
    {
        //if the difference between the 2 x is greater than 0 we have moved North
        if (_currentNode.x - _nextNode.x == 1)
        {
            map[_currentNode.x, _currentNode.y].West = false;
            map[_nextNode.x, _nextNode.y].East = false;
        }
        //if the difference between the 2 y is less than 0 we have moved North
        if (_currentNode.x - _nextNode.x == -1)
        {
            map[_currentNode.x, _currentNode.y].East = false;
            map[_nextNode.x, _nextNode.y].West = false;
        }
        //if the difference between the 2 y is greater than 0 we have moved North
        if (_currentNode.y - _nextNode.y == 1)
        {
            map[_currentNode.x, _currentNode.y].South = false;
            map[_nextNode.x, _nextNode.y].North = false;
        }
        //if the difference between the 2 y is less than 0 we have moved North
        if (_currentNode.y - _nextNode.y == -1)
        {
            map[_currentNode.x, _currentNode.y].North = false;
            map[_nextNode.x, _nextNode.y].South = false;
        }


    }

    long GenerateRandomSeed()
    {
        long num = System.DateTime.Now.Millisecond;
        //lcg
        return (long)(((a * num) + c) % m);
    }


    Node CheckNeighbours(Node _currentNode)
    {
        List<Node> neighbourNodes = new List<Node>();

        //North
        if (_currentNode.y - 1 >= 0)
        {
            if (!map[_currentNode.x, _currentNode.y - 1].HasBeenVisited)
            {
                neighbourNodes.Add(map[_currentNode.x, _currentNode.y - 1]);
            }
        }

        //South
        if (_currentNode.y + 1 < gridHeight)
        {
            if (!map[_currentNode.x, _currentNode.y + 1].HasBeenVisited)
            {
                neighbourNodes.Add(map[_currentNode.x, _currentNode.y + 1]);
            }
        }

        //East
        if (_currentNode.x + 1 < gridWidth)
        {
            if (!map[_currentNode.x + 1, _currentNode.y].HasBeenVisited)
            {
                neighbourNodes.Add(map[_currentNode.x + 1, _currentNode.y]);
            }
        }

        //West
        if (_currentNode.x - 1 >= 0)
        {
            if (!map[_currentNode.x - 1, _currentNode.y].HasBeenVisited)
            {
                neighbourNodes.Add(map[_currentNode.x - 1, _currentNode.y]);
            }
        }

        if (neighbourNodes.Count > 0)
        {
            int num = Random.Range(0, neighbourNodes.Count);

            return neighbourNodes[num];
            
        }
        return null;


    }
}