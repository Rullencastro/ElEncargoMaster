
using System;
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
        StartEndCells();
        RecursiveBackTracking(startWidth, startHeight);
        maze[startWidth, startHeight] = Cell.Start;
    }

    private void StartEndCells()
    {
        if (Random.value <= 0.5f)
        {
            startWidth = Random.value <= 0.5f ? 0 : width - 1;
            startHeight = Random.Range(0,height);
            endWidth = startWidth == 0 ? width - 1 : 0 ;
            endHeight = startHeight < height/2 ? Random.Range(height/2, height):Random.Range(0, height/2);
        }
        else
        {
            startWidth = Random.Range(0, width);
            startHeight = Random.value <= 0.5f ? 0 : height - 1;
            endWidth = startWidth < width/2 ? Random.Range(width/2, width):Random.Range(0, width/2);
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
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        // Create a dictionary to store the parent of each visited cell
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();
        parent.Add(new Vector2Int(startX, startY), new Vector2Int(-1, -1));

        // Crea una lista con las direcciones posibles para avanzar
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(Vector2Int.up);
        directions.Add(Vector2Int.right);
        directions.Add(Vector2Int.down);
        directions.Add(Vector2Int.left);

        // BFS algorithm to find the path
        while (queue.Count > 0)
        {
            // Dequeue the cell at the front of the queue
            Vector2Int current = queue.Dequeue();

            // If we have reached the end cell, we have found a path
            if (current.x == endX && current.y == endY)
            {
                return true;
            }

            // Check the neighbors of the current cell
            foreach (Vector2Int d in directions)
            {
                Vector2Int neighbour = new Vector2Int(current.x + d.x, current.y + d.y);

                // Check if the neighbor is inside the maze and has not been visited
                if (neighbour.x >= 0 && neighbour.x < width && neighbour.y >= 0 && neighbour.y < height && !visited[neighbour.x,neighbour.y])
                {
                    // Mark the neighbor as visited, add it to the queue, and set its parent
                    visited[neighbour.x, neighbour.y] = true;
                    queue.Enqueue(neighbour);
                    parent.Add(neighbour, current);
                }
            }
        }

        // If we have not found a path, we need to remove some walls to connect the start and end cells
        Vector2Int c = new Vector2Int(endX, endY);
        while (parent.ContainsKey(c))
        {
            Vector2Int p = parent[c];
            if (maze[p.x, p.y] == Cell.Wall)
            {
                maze[p.x, p.y] = Cell.Floor;
            }
            c = p;
        }
        return false;
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




