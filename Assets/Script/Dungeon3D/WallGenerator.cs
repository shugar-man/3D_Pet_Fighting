using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    [SerializeField]
    private static GameObject floorPrefab , wallPrefab ;
    public static void CreateWalls(HashSet<Vector3Int> floorPositions , TilemapVisualizer tilemapVisualizer) {

        var basicWallPositions = FindWallsInDirections(floorPositions, Direction3D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions , Direction3D.diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions , floorPositions);
        //CreateCornerWall(tilemapVisualizer, basicWallPositions , floorPositions);
        
        //tilemapVisualizer.PaintSingleBesicWall(basicWallPositions);
    }
    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector3Int> basicWallPositions, HashSet<Vector3Int> floorPositions) {
        tilemapVisualizer.PaintSingleBesicWall(basicWallPositions ,floorPositions );
    }
    /*private static void CreateCornerWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector3Int> basicWallPositions, HashSet<Vector3Int> floorPositions) {
        tilemapVisualizer.PaintSingleCornerWall(basicWallPositions ,floorPositions );
    }*/

    private static HashSet<Vector3Int> FindWallsInDirections(HashSet<Vector3Int> floorPositions, List<Vector3Int> directionList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (!floorPositions.Contains(neighbourPosition)) {
                // Calculate wall position based on floor position and direction
                Vector3Int wallPosition = position + (direction); 
                wallPositions.Add(wallPosition); 
                }
            }
        }
        return wallPositions;
    }

}
