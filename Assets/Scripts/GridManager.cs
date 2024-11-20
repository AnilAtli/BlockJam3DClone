using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridItem
{
    public GameObject ground; 
    public GameObject character; 
    public Vector3Int gridPoz;
}
[System.Serializable]
public class CharacterType
{
    public GameObject[] character;
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 5; 
    public int columns = 5; 
    public float cellSize = 1f; 
    public List<GridItem> gridItems = new List<GridItem>();
    public Color gridColor = Color.white; 
    public CharacterType characterType; 

    void Start()
    {
        SpawnGrid();
        SpawnCharacters();
    }


    private void SpawnGrid()
    {
        foreach (GridItem item in gridItems)
        {
            if (item.ground != null)
            {
               
                Vector3 cellPosition = new Vector3(item.gridPoz.x * cellSize, 0, item.gridPoz.z * cellSize);

              
                GameObject cell = Instantiate(item.ground, cellPosition, Quaternion.identity, transform);
                cell.name = $"Cell ({item.gridPoz.x}, {item.gridPoz.z})";

               
                item.ground = cell;
            }
        }
    }

 
    private void SpawnCharacters()
    {
        foreach (GridItem item in gridItems)
        {
            
            if (characterType.character.Length > 0)
            {
                GameObject randomCharacter = characterType.character[Random.Range(0, characterType.character.Length)];

               
                Vector3 spawnPosition = new Vector3(item.gridPoz.x * cellSize, 0, item.gridPoz.z * cellSize);
                Quaternion rotation = Quaternion.Euler(0, -90, 0); 
                GameObject spawnedCharacter = Instantiate(randomCharacter, spawnPosition, rotation, transform);


              
                item.character = spawnedCharacter;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

       
        Vector3 startPos = transform.position;

      
        for (int x = 0; x <= columns; x++)
        {
            float xPos = startPos.x + x * cellSize;
            Gizmos.DrawLine(
                new Vector3(xPos, startPos.y, startPos.z),
                new Vector3(xPos, startPos.y, startPos.z + rows * cellSize)
            );
        }

       
        for (int z = 0; z <= rows; z++)
        {
            float zPos = startPos.z + z * cellSize;
            Gizmos.DrawLine(
                new Vector3(startPos.x, startPos.y, zPos),
                new Vector3(startPos.x + columns * cellSize, startPos.y, zPos)
            );
        }

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
            
                Vector3 cellCenter = new Vector3(
                    startPos.x + x * cellSize + cellSize / 2,
                    startPos.y,
                    startPos.z + z * cellSize + cellSize / 2
                );

           
                GUIStyle style = new GUIStyle();
                style.normal.textColor = gridColor;
#if UNITY_EDITOR
                UnityEditor.Handles.Label(cellCenter, $"({x}, {z})", style);
#endif
            }
        }
    }
}
