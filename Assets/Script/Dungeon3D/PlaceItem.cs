using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int enemyCounts = 14, roomCounts = 5 , itemCounts = 11 , tableCounts = 11 , boxCounts = 11;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent = 0.8f;


    protected override void RunProceduralGeneration() {
        ItemGeneration();
    }
    private void ItemGeneration() {

    }
}
