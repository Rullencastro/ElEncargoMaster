
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum Cell
{
    Wall,Floor,Start,End
}

public class MazeGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public List<GameObject> wallPrefab;
    public List<GameObject> wallCornerPrefab;
    public GameObject playerPrefab;
    public GameObject endPrefab;
    public static MazeGenerator Instance;


    private Cell[,] maze;
    private int startWidth = 0;
    private int startHeight = 0;
    private int endWidth = 0;
    private int endHeight = 0;
    private List<GameObject> wallsCreated = new List<GameObject>();
    private GameObject player;
    private GameObject end;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ResetMaze()
    {
        DestroyMazeVisuals();
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // Crea una matriz para el laberinto
        maze = new Cell[width, height];

        StartMaze();
        StartEndCells();
        RecursiveBackTracking(startWidth, startHeight);
        maze[startWidth, startHeight] = Cell.Start;
        CreateMazeVisuals();
    }

    private void StartEndCells()
    {
        if (Random.value <= 0.5f)
        {
            startWidth = Random.value <= 0.5f ? 0 : width - 1;
            startHeight = Random.Range(0, height);
            endWidth = startWidth == 0 ? width - 1 : 0;
            endHeight = startHeight < height / 2 ? Random.Range(height / 2, height) : Random.Range(0, height / 2);
        }
        else
        {
            startWidth = Random.Range(0, width);
            startHeight = Random.value <= 0.5f ? 0 : height - 1;
            endWidth = startWidth < width / 2 ? Random.Range(width / 2, width) : Random.Range(0, width / 2);
            endHeight = startHeight == 0 ? height - 1 : 0;
        }

        maze[endWidth, endHeight] = Cell.End;
    }

    private void StartMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = Cell.Wall;
            }
        }
    }


    private void RecursiveBackTracking(int startX, int startY)
    {
        // Marca la celda actual como visitada
        maze[startX, startY] = Cell.Floor;

        // Crea una lista con las direcciones posibles para avanzar
        List<Vector2> directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.right);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);

        // Mezcla las direcciones aleatoriamente
        ShuffleList(directions);

        // Avanza en cada direccion posible
        foreach (Vector2 direction in directions)
        {
            int nextX = startX + (int)direction.x * 2;
            int nextY = startY + (int)direction.y * 2;

            if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height)
            {
                if (maze[nextX, nextY] == Cell.Wall || maze[nextX, nextY] == Cell.End)
                {
                    // Marca la celda intermedia como visitada
                    maze[nextX - (int)direction.x, nextY - (int)direction.y] = Cell.Floor;

                    // Quita la pared entre las celdas
                    maze[nextX, nextY] = Cell.Floor;

                    // Avanza recursivamente desde la celda siguiente
                    RecursiveBackTracking(nextX, nextY);
                }
            }
        }
    }



    // Funcion para mezclar una lista aleatoriamente
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }



    private void CreateMazeVisuals()
    {
        wallsCreated = new List<GameObject>();
        // Crea los prefabs de la pared para cada celda del laberinto
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x + 0.5f, 0, y + 0.5f);

                if (maze[x, y] == Cell.Wall)
                {
                    int wallIndex = Random.Range(0, wallPrefab.Count);
                    wallsCreated.Add(Instantiate(wallPrefab[wallIndex], position, Quaternion.identity));
                }

                if (maze[x, y] == Cell.Start)
                {
                    player = Instantiate(playerPrefab, position, Quaternion.identity);
                }

                if (maze[x, y] == Cell.End)
                {
                    end = Instantiate(endPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    private void DestroyMazeVisuals()
    {
        foreach (var w in wallsCreated)
        {
            Destroy(w);
        }
        Destroy(player);
        end.transform.DOKill();
        Destroy(end);
    }


    public List<Vector2Int> AStarFindPath()
    {
        Vector2Int start = new(startWidth, startHeight);
        Vector2Int end = new(endWidth, endHeight);
        // Create a list to store the path
        List<Vector2Int> path = new List<Vector2Int>();

        // Create a dictionary to store the g scores for each node
        Dictionary<Vector2Int, int> gScores = new Dictionary<Vector2Int, int>();

        // Create a dictionary to store the parent of each node
        Dictionary<Vector2Int, Vector2Int> parents = new Dictionary<Vector2Int, Vector2Int>();

        // Add the start node to the list of nodes to expand
        List<Vector2Int> nodesToExpand = new List<Vector2Int>
            {
                start
            };

        // Initialize the g score and parent of the start node
        gScores[start] = 0;
        parents[start] = Vector2Int.zero;

        // Loop until the list of nodes to expand is empty
        while (nodesToExpand.Count > 0)
        {
            // Take the last node from the list of nodes to expand
            Vector2Int current = nodesToExpand[nodesToExpand.Count - 1];
            nodesToExpand.RemoveAt(nodesToExpand.Count - 1);

            // If the current node is the end node, construct the path and return it
            if (current == end)
            {
                Vector2Int node = end;
                while (node != start)
                {
                    path.Insert(0, node);
                    node = parents[node];
                }
                path.Insert(0, start);
                return path;
            }

            // Expand the current node
            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                int cost = gScores[current] + 1;
                if (!gScores.ContainsKey(neighbor) || cost < gScores[neighbor])
                {
                    gScores[neighbor] = cost;
                    parents[neighbor] = current;
                    nodesToExpand.Add(neighbor);
                }
            }
        }

        // If no path was found, return an empty list
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (current.x > 0 && maze[current.x - 1, current.y] != Cell.Wall)
        {
            neighbors.Add(new Vector2Int(current.x - 1, current.y));
        }
        if (current.x < maze.GetLength(0) - 1 && maze[current.x + 1, current.y] != Cell.Wall)
        {
            neighbors.Add(new Vector2Int(current.x + 1, current.y));
        }
        if (current.y > 0 && maze[current.x, current.y - 1] != Cell.Wall)
        {
            neighbors.Add(new Vector2Int(current.x, current.y - 1));
        }
        if (current.y < maze.GetLength(1) - 1 && maze[current.x, current.y + 1] != Cell.Wall)
        {
            neighbors.Add(new Vector2Int(current.x, current.y + 1));
        }
        return neighbors;
    }
}




