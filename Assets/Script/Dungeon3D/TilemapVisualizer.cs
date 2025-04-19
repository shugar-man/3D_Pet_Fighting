using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private GameObject floorPrefab , wallPrefab , wallSide ,wallTop , table , bookshelf , box , enemy , boss , coin ,fire ,sewer , pet;
    [SerializeField]
    private int tableCount, bookshelfCount , boxCount , enemyCount , bossCount , coinCount , sewerCount , fireCount;
    [System.Serializable]
    public class WallTypeSets
    {
        public List<int> wallTop = new List<int>();
        public List<int> wallSideRight = new List<int>();
        public List<int> wallSideLeft = new List<int>();
        public List<int> wallBottom = new List<int>();
    }
    private HashSet<Vector3Int> instantiatedWallPositions = new HashSet<Vector3Int>();
    public WallTypeSets wallTypeSets; 
    public void VisualizeFloor(IEnumerable<Vector3Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            // Instantiate a floor prefab at the given position
            GameObject floorInstance = Instantiate(floorPrefab, position, Quaternion.identity, transform); 
            floorInstance.name = $"Floor_{position.x}_{position.y}_{position.z}";
            
        }
    }
    public void VisualizeObjectMap(IEnumerable<Vector3Int> floorPositions) {
        VisualizeObject(enemy,floorPositions ,new Vector3(0, 0.5f, 0) ,enemyCount );
        VisualizeObject(boss ,floorPositions ,new Vector3(0, 0.5f, 0),bossCount);
        VisualizeObject(sewer ,floorPositions ,new Vector3(0, 0.1f, 0),sewerCount);
        VisualizeObject(table ,floorPositions ,new Vector3(0, 0.3f, 0),tableCount);
        VisualizeObject(box ,floorPositions ,new Vector3(0, 0.3f, 0),boxCount);
        VisualizeObject(coin ,floorPositions ,new Vector3(0, 0.9f, 0),coinCount);
    }
    public void VisualizeObject(GameObject gameObject ,IEnumerable<Vector3Int> floorPositions , Vector3 positiont,int count) {
        var randomPositionsForOject = floorPositions.OrderBy(_ => Random.value).Take(count);


        foreach (var position in randomPositionsForOject)
        {
            // Instantiate a floor prefab at the given position
            GameObject instance = Instantiate(gameObject, position + positiont, Quaternion.identity, transform);
            instance.name = $"{gameObject.name}_{position.x}_{position.y}_{position.z}";
        }
    }
    public void VisualizePet() {
            GameObject instance = Instantiate(pet, new Vector3(1, 0.5f, 0), Quaternion.identity, transform);
            instance.name = $"pet";
    }
    public void VisualizeCeil(IEnumerable<Vector3Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            // Instantiate a floor prefab at the given position
            GameObject ceilInstance = Instantiate(floorPrefab, position + new Vector3(0,9.35f,0), Quaternion.identity, transform); 
            ceilInstance.name = $"Ceil_{position.x}_{position.y}_{position.z}";
            
        }
    }
    /*public void VisualizeEnemy(IEnumerable<Vector3Int> floorPositions)
    {
        var randomPositionsForEnemy = floorPositions.OrderBy(_ => Random.value).Take(enemyCount);


        foreach (var position in randomPositionsForEnemy)
        {
            // Instantiate a floor prefab at the given position
            GameObject enemyInstance = Instantiate(enemy, position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform);
            enemyInstance.name = $"Enemy_{position.x}_{position.y}_{position.z}";
        }
    }*/

    public void VisualizeWall(HashSet<Vector3Int> basicWallPositions, HashSet<Vector3Int> floorPositions)
    {
        
        HashSet<Vector3Int> shelfPosition = new HashSet<Vector3Int>();
        HashSet<Vector3Int> firePosition = new HashSet<Vector3Int>();
        foreach (var position in floorPositions)
        {

            // Generate a binary string based on neighbors
                Vector3Int neighbourTopPosition = position + new Vector3Int(0, 0, 5);
                Vector3Int neighbourRightPosition = position + new Vector3Int(5, 0, 0);
                Vector3Int neighbourBottomPosition = position + new Vector3Int(0, 0, -5);
                Vector3Int neighbourLeftPosition = position + new Vector3Int(-5, 0, 0);
                
                if (!floorPositions.Contains(neighbourTopPosition)) {
                    InstantiateWall(position + new Vector3Int(0, 0, 2),wallPrefab, 90, "Wall_Top", position);
                    InstantiateWall(position + new Vector3Int(0, 8, 1),fire , 0, "Fire_Top", position);
                    shelfPosition.Add(position);
                }
                if (!floorPositions.Contains(neighbourRightPosition)) {
                    InstantiateWall(position + new Vector3Int(2, 0, 0),wallPrefab, 0, "Wall_SideRight", position);
                    InstantiateWall(position + new Vector3Int(1, 8, 0),fire , 90, "Fire_SideRight", position);
                    shelfPosition.Add(position);
                }
                if (!floorPositions.Contains(neighbourBottomPosition)) {
                    InstantiateWall(position + new Vector3Int(0, 0, -2),wallPrefab, 90, "Wall_Bottom", position);
                    InstantiateWall(position + new Vector3Int(0, 8, -1),fire , 0, "Fire_Bottom", position);
                    shelfPosition.Add(position);
                }
                if (!floorPositions.Contains(neighbourLeftPosition)) {
                    InstantiateWall(position + new Vector3Int(-2, 0, 0),wallPrefab, 0, "Wall_SideLeft", position);
                    InstantiateWall(position + new Vector3Int(-1, 8, 0),fire , 90, "Fire_SideLeft", position);
                    shelfPosition.Add(position);

                }    
        }
        var randomPositionsForShelt = shelfPosition.OrderBy(_ => Random.value).Take(bookshelfCount);
        foreach (var position in randomPositionsForShelt)
        {

            // Generate a binary string based on neighbors
                Vector3Int neighbourTopPosition = position + new Vector3Int(0, 0, 5);
                Vector3Int neighbourRightPosition = position + new Vector3Int(5, 0, 0);
                Vector3Int neighbourBottomPosition = position + new Vector3Int(0, 0, -5);
                Vector3Int neighbourLeftPosition = position + new Vector3Int(-5, 0, 0);
                
                if (!shelfPosition.Contains(neighbourTopPosition)) {
                    InstantiateWall(position + new Vector3Int(0, 0, 1),bookshelf , 0, "Shelf_Top", position);
                    shelfPosition.Add(position);
                    
                }
                if (!shelfPosition.Contains(neighbourRightPosition)) {
                    InstantiateWall(position + new Vector3Int(1, 0, 0),bookshelf , 90, "Shelf_SideRight", position);
                    shelfPosition.Add(position);

                }
                if (!shelfPosition.Contains(neighbourBottomPosition)) {
                    InstantiateWall(position + new Vector3Int(0, 0, -1),bookshelf , 0, "Shelf_Bottom", position);
                    shelfPosition.Add(position);

                }
                if (!shelfPosition.Contains(neighbourLeftPosition)) {
                    InstantiateWall(position + new Vector3Int(-1, 0, 0),bookshelf , 90, "Shelf_SideLeft", position);
                    shelfPosition.Add(position);


                }    
        }
    }

/// <summary>
/// Helper method to instantiate and name wall objects.
/// </summary>
/// <param name="position">Position to place the wall.</param>
/// <param name="rotationY">Y-axis rotation for the wall.</param>
/// <param name="wallType">Type of wall for naming.</param>
/// <param name="originalPosition">Original grid position for reference.</param>
private void InstantiateWall(Vector3Int position, GameObject gameObject, float rotationY, string objectType, Vector3Int originalPosition)
{
    if (instantiatedWallPositions.Contains(position))
    {
        Debug.Log($"object already exists at position: {position}");
        return;
    }
    GameObject wallInstance = Instantiate(gameObject, position, Quaternion.Euler(0, rotationY, 0), transform);
    //GameObject wallInstance_2 = Instantiate(gameObject, position + new Vector3(0,4.7f,0), Quaternion.Euler(0, rotationY, 0), transform);
    wallInstance.name = $"{objectType}_{originalPosition.x}_{originalPosition.y}_{originalPosition.z}";
    instantiatedWallPositions.Add(position); // Track this position
}


    internal void PaintSingleBesicWall(HashSet<Vector3Int> positions, HashSet<Vector3Int> floorPositions)
    {
        VisualizeWall(positions , floorPositions);
    }

    
    /*internal void PaintSingleCornerWall (HashSet<Vector3Int> positions, HashSet<Vector3Int> floorPositions) {
    HashSet<Vector3Int> instantiatedWallPositions = new HashSet<Vector3Int>();

    foreach (var position in positions)
    {
        string neighboursBinaryType = "";

        // Determine neighbors' binary type
        foreach (var direction in Direction3D.eightDirectionsList)
        {
            Vector3Int neighbourPosition = position + direction;
            neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
        }

        Debug.Log($"Corner {neighboursBinaryType} sdf {position}");
        int typeAsInt = Convert.ToInt32(neighboursBinaryType, 2);

        // Check each wall type and place walls accordingly
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 90f, "Wall_InnerCornerDownLeft", position);
            InstantiateWall(position + new Vector3Int(0, 0, 2), 0f, "Wall_InnerCornerDownLeft", position);
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(2, 0, 0), 90f, "Wall_InnerCornerDownRight", position);
            InstantiateWall(position + new Vector3Int(0, 0, -2), 0f, "Wall_InnerCornerDownRight", position);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 90f, "Wall_DiagonalCornerDownLeft", position);
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 90f, "Wall_DiagonalCornerDownRight", position);
            InstantiateWall(position + new Vector3Int(0, 0, 2), 0f, "Wall_DiagonalCornerDownRight", position);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 90f, "Wall_DiagonalCornerUpRight", position);
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 90f, "Wall_DiagonalCornerUpLeft", position);
            InstantiateWall(position + new Vector3Int( 0, 0, 2), 0f, "Wall_DiagonalCornerUpLeft", position);
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 0f, "Wall_Full", position);
        }
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            InstantiateWall(position + new Vector3Int(-2, 0, 0), 0f, "Wall_Bottom", position);
        }
    }
}*/
    
}
