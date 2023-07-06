using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    // Reference to the tower that the player will build.
    public BaseTower towerPrefab;
    public LayerMask buildSpotMask = ~((1 << 10));

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast for build spots.
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, buildSpotMask))
        {
            // If we hit a build spot
            BuildSpot buildSpot = hit.transform.GetComponent<BuildSpot>();
            if(buildSpot != null)
            {
                // Here we are only managing the click on the build spot, the tower building is handled by the build menu.
                buildSpot.OnBuildSpotClicked();
            }
        }
        else
        {
            // Check if we clicked on UI elements
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if (results.Count == 0)
            {
                TowerBuildMenu.instance.HideMenu();
            }
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
                    int refundAmount = buildSpot.tower.cost / 2;
                    buildSpot.RemoveTower();
                    Player.instance.AddGold(refundAmount);  // Give back half the cost as refund.
                }
            }
        }
    }
}
