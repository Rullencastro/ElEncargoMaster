
using System.Collections.Generic;
using UnityEngine;


public enum Cell
{
    Wall,Floor,Start,End
}

public class MazeGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public List<GameObject> wallPrefab;
    public GameObject playerPrefab;
    public GameObject endPrefab;  
 
    private Cell[,] maze;
    private int startWidth = 0;
    private int startHeight = 0;
    private int endWidth = 0;
    private int endHeight = 0;

    private void Start()
    {
        GenerateMaze();
        CreateMazeVisuals();
    }

    private void GenerateMaze()
    {
        // Crea una matriz para el laberinto
        maze = new Cell[width, height];

        StartMaze();
        CreateWalls();
        StartEndCells();
        RecursiveBackTracking(startWidth, startHeight);
        FindPath(startWidth, startHeight,endWidth,endHeight);
        maze[startWidth, startHeight] = Cell.Start;
        maze[endWidth, endHeight] = Cell.End;
    }

    private void StartEndCells()
    {
        if (Random.value <= 0.5f)
        {
            startWidth = Random.value <= 0.5f ? 0 : width - 1;
            startHeight = Random.Range(0,height);
        }
        else
        {
            startWidth = Random.Range(0, width);
            startHeight = Random.value <= 0.5f ? 0 : height - 1;
        }

        do {
            if (Random.value <= 0.5f)
            {
                endWidth = Random.value <= 0.5f ? 0 : width - 1;
                endHeight = Random.Range(0, height);
            }
            else
            {
                endWidth = Random.Range(0, width);
                endHeight = Random.value <= 0.5f ? 0 : width - 1;
            }
        }while(endWidth == startWidth && endHeight == startHeight);
    }

    private void StartMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                    maze[x, y] = Cell.Floor;
            }
        }
    }

    private void CreateWalls()
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

        // Avanza en cada dirección posible
        foreach (Vector2 direction in directions)
        {
            int nextX = startX + (int)direction.x * 2;
            int nextY = startY + (int)direction.y * 2;

            if (nextX >= 0 && nextX < width && nextY >= 1 && nextY < height)
            {
                if (maze[nextX, nextY] == Cell.Wall)
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

    private bool FindPath(int startX, int startY, int endX, int endY)
    {
        // Create a boolean 2D array to keep track of visited cells
        bool[,] visited = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == Cell.Wall)
                {
                    visited[x, y] = true;
                }
            }
        }

        // Create a queue to store the cells to be visited
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(startX, startY));
        visited[startX, startY] = true;

        // Create a dictionary to store the parent of each visited cell
        Dictionary<Vector2, Vector2> parent = new Dictionary<Vector2, Vector2>();
        parent.Add(new Vector2(startX, startY), new Vector2(-1, -1));

        // Crea una lista con las direcciones posibles para avanzar
        List<Vector2> directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.right);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);

        // BFS algorithm to find the path
        while (queue.Count > 0)
        {
            // Dequeue the cell at the front of the queue
            Vector2 current = queue.Dequeue();

            // If we have reached the end cell, we have found a path
            if (current.x == endX && current.y == endY)
            {
                return true;
            }

            // Check the neighbors of the current cell
            foreach (Vector2 d in directions)
            {
                Vector2 neighbour = new Vector2(current.x + d.x, current.y + d.y);

                // Check if the neighbor is inside the maze and has not been visited
                if (neighbour.x >= 0 && neighbour.x < width && neighbour.y >= 0 && neighbour.y < height && !visited[(int)neighbour.x, (int)neighbour.y])
                {
                    // Mark the neighbor as visited, add it to the queue, and set its parent
                    visited[(int)neighbour.x, (int)neighbour.y] = true;
                    queue.Enqueue(neighbour);
                    parent.Add(neighbour, current);
                }
            }
        }

        // If we have not found a path, we need to remove some walls to connect the start and end cells
        Vector2 c = new Vector2(endX, endY);
        while (parent.ContainsKey(c))
        {
            Vector2 p = parent[c];
            if (maze[(int)p.x, (int)p.y] == Cell.Wall)
            {
                maze[(int)p.x, (int)p.y] = Cell.Floor;
            }
            c = p;
        }
        return false;
    }

       

    private void AddExitPath(int startX, int startY, int endX, int endY)
    {
        int currentX = startX;
        int currentY = startY;

        while (currentX != endX || currentY != endY)
        {
            // Avanza hacia la celda de destino
            if (currentX < endX)
            {
                currentX++;
            }
            else if (currentX > endX)
            {
                currentX--;
            }
            else if (currentY < endY)
            {
                currentY++;
            }
            else if (currentY > endY)
            {
                currentY--;
            }

            // Quita la pared entre las celdas
            maze[currentX, currentY] = Cell.Floor;
        }
    }

    // Función para mezclar una lista aleatoriamente
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
        // Crea los prefabs de la pared para cada celda del laberinto
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x + 0.5f, 0, y + 0.5f);

                if (maze[x, y] == Cell.Wall)
                {

                    int wallIndex = Random.Range(0, wallPrefab.Count);
                    Instantiate(wallPrefab[wallIndex] , position, Quaternion.identity);
                }
                else if (maze[x, y] == Cell.Start)
                {
                    Instantiate(playerPrefab, position, Quaternion.identity);
                }
                else if (maze[x, y] == Cell.End)
                {
                    Instantiate(endPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}




