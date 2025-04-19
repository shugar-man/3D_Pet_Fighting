using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4 , minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20 , dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration() {
        CreateRooms();
    }
    private void CreateRooms() {
        var bounds = new BoundsInt(
            (Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, 1, dungeonHeight)); // Y = 1 เพราะเราจะไม่ใช้มัน

        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning3D(
            bounds,
            minRoomWidth,  // แนว X (ความกว้าง)
            1,             // ความสูง Y = 1 พอ เพราะพื้นอยู่ระดับเดียว
            minRoomHeight  // แนว Z (ความลึก/ยาว)
        );
        // var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning3D(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, 0, dungeonHeight)), minRoomWidth, 0, minRoomHeight);
         HashSet<Vector3Int> floor = new HashSet<Vector3Int>();

         if (randomWalkRooms) {
            floor = CreateRoomsRandomly(roomsList);
         }
         else {
            floor = CreateSimpleRooms(roomsList);
         }

         for (int i = 0; i < roomsList.Count; i++)
            {
                var roomBounds = roomsList[i];
                Debug.Log($"Room {i} bounds: pos={roomBounds.position}, size={roomBounds.size}");

                var roomCenter = new Vector3Int(
                    Mathf.RoundToInt(roomBounds.center.x),
                    0,
                    Mathf.RoundToInt(roomBounds.center.z));
                Debug.Log($"Room {i} center used for walk: {roomCenter}");

                var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
                Debug.Log($"Room {i} floor tiles: {roomFloor.Count}");
            }

         List<Vector3Int> roomCenters = new List<Vector3Int>();
         foreach( var room in roomsList) {
            roomCenters.Add(Vector3Int.RoundToInt(room.center));
            //player.position = room.center;
            Debug.Log(Vector3Int.RoundToInt(room.center));
         }
         Debug.Log(roomCenters.Count);
         HashSet<Vector3Int> corridors = ConnectRooms(roomCenters);
         tilemapVisualizer.VisualizeObjectMap(floor);
         floor.UnionWith(corridors);

         tilemapVisualizer.VisualizeFloor(floor);
         tilemapVisualizer.VisualizeCeil(floor);
         WallGenerator.CreateWalls(floor , tilemapVisualizer);
    }
    private HashSet<Vector3Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector3Int> floor = new HashSet<Vector3Int>();
        
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector3Int(
                Mathf.RoundToInt(roomBounds.center.x),
                0,
                Mathf.RoundToInt(roomBounds.center.z));
            
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && 
                position.x <= (roomBounds.xMax - offset) && 
                position.z >= (roomBounds.zMin + offset) && 
                position.z <= (roomBounds.zMax - offset))
                {
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
    private HashSet<Vector3Int> ConnectRooms(List<Vector3Int> roomCenters) {
        HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0) {
            Vector3Int closest = FindClosestPointTo(currentRoomCenter , roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector3Int> newCorridor = CreateCorridor(currentRoomCenter , closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }
    private HashSet<Vector3Int> CreateCorridor(Vector3Int currentRoomCenter, Vector3Int destination) {
        HashSet<Vector3Int> corridor = new HashSet<Vector3Int>();
        var position = currentRoomCenter;
        
        //corridor.Add(position);
        while (position.z != destination.z) {
            if (destination.z > position.z) {
                position += new Vector3Int(0, 0, 5);

            }
            else if (destination.z < position.z) {
                position += new Vector3Int(0, 0, -5);
            }
            //Debug.Log(position);
            corridor.Add(position);
        }
        while (position.x != destination.x) {
            if (destination.x > position.x) {
                position += new Vector3Int(5, 0, 0);
            }
            else if (destination.x < position.x) {
                position += new Vector3Int(-5, 0, 0);
            }
            corridor.Add(position);
        }
        return corridor;

    }
    private Vector3Int FindClosestPointTo(Vector3Int currentRoomCenter, List<Vector3Int> roomCenters) {
        Vector3Int closest = Vector3Int.zero;
        float distance = float.MaxValue;
        foreach( var position in roomCenters ) {
            float currentDistance = Vector3Int.Distance(position , currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;

    }

    private HashSet<Vector3Int> CreateSimpleRooms(List<BoundsInt> roomsList) {
        HashSet<Vector3Int> floor  = new HashSet<Vector3Int>();
        foreach (var room in roomsList ) {
            for (int col = offset ; col < room.size.x - offset ; col++) {
                for ( int row = offset; row <room.size.y - offset ; row++) {
                    Vector3Int position = (Vector3Int)room.min + new Vector3Int(col,0,row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
    /*private HashSet<Vector3Int> CreateSimpleRooms(List<BoundsInt> roomsList) {
        HashSet<Vector3Int> floor  = new HashSet<Vector3Int>();
        foreach (var room in roomsList ) {
            for (int col = offset ; col < room.size.x - offset ; col++) {
                for ( int row = offset; row <room.size.y - offset ; row++) {
                    for ( int z = offset; z <room.size.z - offset ; z++) {
                    Vector3Int position = (Vector3Int)room.min + new Vector3Int(col,row,z);
                    floor.Add(position);
                    }
                }
            }
        }
        return floor;
    }*/

}
