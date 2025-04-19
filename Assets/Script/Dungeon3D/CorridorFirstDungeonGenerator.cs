using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = 0.8f;


    protected override void RunProceduralGeneration() {
        CorridorFirstDungeonGeneration();
    }
    private void CorridorFirstDungeonGeneration() {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        //HashSet<Vector3Int> BeginRoomPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> potentialRoomPositions = new HashSet<Vector3Int>();

        //CreateCorridors(floorPositions);
        CreateCorridors(floorPositions , potentialRoomPositions);
        HashSet<Vector3Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector3Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds , roomPositions);

        floorPositions.UnionWith(roomPositions);
        tilemapVisualizer.VisualizeFloor(floorPositions);
        //tilemapVisualizer.VisualizeTableAndBox(roomPositions);
        //tilemapVisualizer.VisualizeEnemy(roomPositions);
        //tilemapVisualizer.VisualizeBoss(roomPositions);
        tilemapVisualizer.VisualizeObjectMap(roomPositions);
        tilemapVisualizer.VisualizeCeil(roomPositions);
        //tilemapVisualizer.VisualizePet();

        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    
    private void CreateRoomsAtDeadEnd(List<Vector3Int> deadEnds , HashSet<Vector3Int> roomFloors) {
        foreach (var position in deadEnds) {
            if (roomFloors.Contains(position) == false) {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }


    private List<Vector3Int> FindAllDeadEnds(HashSet<Vector3Int> floorPositions) {
        List<Vector3Int> deadEnds = new List<Vector3Int>();
        foreach ( var position in floorPositions) {
            int neighboursCount = 0;
            foreach (var direction in Direction3D.cardinalDirectionsList) {
                if (floorPositions.Contains(position + direction)) {
                    neighboursCount++;
                }
            }
            if (neighboursCount ==1) {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector3Int> CreateRooms(HashSet<Vector3Int> potentialRoomPositions) {
        HashSet<Vector3Int> roomPositions = new HashSet<Vector3Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);
        bool roomBegin = true;
        List<Vector3Int> roomToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        foreach (var roomPosition in roomToCreate) {
            var roomFloor = RunRandomWalk(randomWalkParameters , roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector3Int> floorPositions , HashSet<Vector3Int> potentialRoomPositions) {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0 ; i < corridorCount ; i ++) {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
