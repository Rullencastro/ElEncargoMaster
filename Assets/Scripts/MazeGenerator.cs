
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
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
    
 
    private Cell[,] maze;
    private int startWidth = 0;
    private int startHeight = 0;
    private int endWidth = 0;
    private int endHeight = 0;
    private List<GameObject> wallsCreated = new List<GameObject>();
    private GameObject player;
    private GameObject end;

    private void Start()
    {
        GenerateMaze();
        CreateMazeVisuals();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            DestroyMazeVisuals();
            GenerateMaze();
            CreateMazeVisuals();
        }
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
        Destroy(end);
    }
}




