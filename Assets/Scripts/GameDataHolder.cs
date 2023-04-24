using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHolder : MonoBehaviourSingleton<GameDataHolder>
{

    public List<JengaBlock> BlocksPrefabs;
    public Stack StackPrefab;
}
