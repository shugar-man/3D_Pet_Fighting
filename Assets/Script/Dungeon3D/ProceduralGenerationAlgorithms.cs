using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector3Int> SimpleRandomWalk3D(Vector3Int startPosition, int walkLength)
    {
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();
        path.Add(startPosition);
        var currentPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = currentPosition + Direction3D.GetRandomCardinalDirection();
            path.Add(newPosition);
            currentPosition = newPosition;
        }

        return path;
    }
    
    public static List<Vector3Int> RandomWalkCorridor(Vector3Int startPosition , int corridorLength) {
        List<Vector3Int> corridor = new List<Vector3Int>();
        var direction = Direction3D.GetRandomEightDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);
        for (int i = 0 ; i < corridorLength ; i ++) {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;


    }

    public static List<BoundsInt> BinarySpacePartitioning3D(BoundsInt spaceToSplit, int minWidth, int minHeight, int minDepth)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            // Check if the room can be split
            if (room.size.x >= minWidth && room.size.z >= minDepth)
            {
                // Randomly decide whether to split or keep as a room
                if (Random.value < 0.7f) // 70% chance to split
                {
                    int axis = Random.Range(0, 2); // Only 0 (X) or 1 (Z)

                    switch (axis)
                    {
                        case 0: // Split along X-axis
                            if (room.size.x >= minWidth * 2)
                                SplitAlongX(minWidth, roomsQueue, room);
                            else
                                roomsList.Add(room);
                            break;

                        case 1: // Split along Z-axis
                            if (room.size.z >= minDepth * 2)
                                SplitAlongZ(minDepth, roomsQueue, room);
                            else
                                roomsList.Add(room);
                            break;
                    }
                }
                else
                {
                    roomsList.Add(room);
                }
            }
            else
            {
                roomsList.Add(room);
            }
        }
        foreach (var room in roomsList) {
            Debug.Log("asdf");
            Debug.Log(room.position);
        }
        for (int i = 0 ; i < roomsList.Count ; i ++) {
                Debug.Log(roomsList[i].position);
            }

        return roomsList;
        
    }
private static void SplitAlongX(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
{
    int xSplit = Random.Range(minWidth, room.size.x - minWidth);

    BoundsInt room1 = new BoundsInt(
        new Vector3Int(room.min.x, 0, room.min.z),
        new Vector3Int(xSplit, 1, room.size.z));

    BoundsInt room2 = new BoundsInt(
        new Vector3Int(room.min.x + xSplit, 0, room.min.z),
        new Vector3Int(room.size.x - xSplit, 1, room.size.z));

    roomsQueue.Enqueue(room1);
    roomsQueue.Enqueue(room2);
}

private static void SplitAlongY(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
{
    int ySplit = Random.Range(minHeight, room.size.y - minHeight);
    BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
    BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
        new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
    roomsQueue.Enqueue(room1);
    roomsQueue.Enqueue(room2);
}

private static void SplitAlongZ(int minDepth, Queue<BoundsInt> roomsQueue, BoundsInt room)
{
    int zSplit = Random.Range(minDepth, room.size.z - minDepth);

    BoundsInt room1 = new BoundsInt(
        new Vector3Int(room.min.x, 0, room.min.z),
        new Vector3Int(room.size.x, 1, zSplit));

    BoundsInt room2 = new BoundsInt(
        new Vector3Int(room.min.x, 0, room.min.z + zSplit),
        new Vector3Int(room.size.x, 1, room.size.z - zSplit));

    roomsQueue.Enqueue(room1);
    roomsQueue.Enqueue(room2);
}
}

public static class Direction3D
{
    public static Vector3Int GetRandomCardinalDirection()
    {
        int directionIndex = Random.Range(0, 4); // 6 directions: up, down, left, right, forward, backward

        switch (directionIndex)
        {
            
            case 0:
                return new Vector3Int(-5, 0, 0);
            case 1:
                return new Vector3Int(5, 0, 0);
            case 2:
                return new Vector3Int(0, 0, 5);
            case 3:
                return new Vector3Int(0, 0, -5);
            /*case 4:
                return Vector3Int.up;
            case 5:
                return Vector3Int.down;*/
            default:
                return Vector3Int.zero; // Should never happen
        }
    }
    
    public static Vector3Int GetRandomEightDirection()
{
    // Generate a random number between 0 and 5 for six possible directions
    int directionIndex = Random.Range(0, 6);

    switch (directionIndex)
    {
        case 0:
            return new Vector3Int(-5, 0, 0); // Left
        case 1:
            return new Vector3Int(5, 0, 0);  // Right
        case 2:
            return new Vector3Int(0, 0, 5);  // Forward
        case 3:
            return new Vector3Int(0, 0, -5); // Backward
        //case 4:
        //    return new Vector3Int(0, 5, 0);  // Up

        default:
            return Vector3Int.zero; // Fallback, should not occur
    }
}
    public static List<Vector3Int> cardinalDirectionsList = new List<Vector3Int>
    {
        /*//Vector3Int.up, // UP
        //Vector3Int.down, // DOWN
        Vector3Int.right, // RIGHT
        Vector3Int.forward, // FORWARD
        Vector3Int.left, // LEFT
        Vector3Int.back, // BACK*/
        new Vector3Int(0, 0, 5), // FORWARD
        new Vector3Int(5, 0, 0), // RIGHT
        new Vector3Int(0, 0, -5), // BACKWARD
        new Vector3Int(-5, 0, 0), // LEFT
    };
    public static List<Vector3Int> diagonalDirectionsList = new List<Vector3Int>
    {
        new Vector3Int(5, 0, 5), // RIGHT-FORWARD
        new Vector3Int(5, 0, -5), // RIGHT-BACKWARD
        new Vector3Int(-5, 0, 5), // LEFT-FORWARD
        new Vector3Int(-5, 0, -5), // LEFT-BACKWARD
    };

    public static List<Vector3Int> eightDirectionsList = new List<Vector3Int>
    {
        new Vector3Int(0, 0, 5), // FORWARD
        new Vector3Int(5, 0, 5), // RIGHT-FORWARD
        new Vector3Int(5, 0, 0), // RIGHT
        new Vector3Int(5, 0, -5), // RIGHT-BACKWARD
        new Vector3Int(0, 0, -5), // BACKWARD
        new Vector3Int(-5, 0, -5), // LEFT-BACKWARD
        new Vector3Int(-5, 0, 0), // LEFT
        new Vector3Int(-5, 0, 5), // LEFT-FORWARD
        
        

        
        
        
        
    };
}