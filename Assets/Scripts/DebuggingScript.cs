using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingScript : MonoBehaviour
{
    public BaseTower towerPrefab;
    public BuildSpot buildSpot;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            buildSpot.BuildTower(towerPrefab);
        else if(Input.GetKeyDown(KeyCode.R))
            buildSpot.RemoveTower();
    }
}
