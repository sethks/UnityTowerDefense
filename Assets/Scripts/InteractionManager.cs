using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Reference to the tower that the player will build.
    public BaseTower towerPrefab;
    public LayerMask buildSpotMask;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Perform the actual Raycast
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, buildSpotMask))
            {
                // If we hit a build spot
                BuildSpot buildSpot = hit.transform.GetComponent<BuildSpot>();
                if(buildSpot != null)
                {
                    // Build a tower if the player has enough gold (TO-DO change interaction for towers)
                    if(Player.instance.gold >= towerPrefab.cost)
                    {
                        buildSpot.BuildTower(towerPrefab);
                    }
                    else
                        Debug.Log("NOT ENOUGH GOLD!");
                }
            }
            else
            {
                TowerBuildMenu.instance.HideMenu();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast.
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, buildSpotMask))
            {
                // If we hit a build spot.
                BuildSpot buildSpot = hit.transform.GetComponent<BuildSpot>();
                if(buildSpot != null && buildSpot.tower != null)
                {
                    // Remove the tower and refund some of the gold.
                    int refuntAmount = buildSpot.tower.cost / 2;
                    buildSpot.RemoveTower();
                    Player.instance.AddGold(refuntAmount);  // Give back half the cost as refund.
                }
            }
        }
    }
}
