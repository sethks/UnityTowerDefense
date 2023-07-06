using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    // Reference to the tower that is currently built on this spot.
    [HideInInspector]
    public BaseTower tower;
    public GameObject towerMenu;

    private void Awake()
    {
        tower = null;
    }

    private void OnMouseDown()
    {

        Debug.Log("TowerBuild instance: " + TowerBuildMenu.instance);
        Debug.Log("Tower prefabs count in OnMouseDown: " + TowerBuildMenu.instance.towerPrefabs.Count);

        // Check if there is already a tower built here.
        if(tower != null)
        {
            Debug.Log("There is already a tower here!");
            return;
        }

        towerMenu.SetActive(true);
        towerMenu.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        TowerBuildMenu.instance.SetBuildSpot(this);
    }

    // This method is called when we want to build a tower.
    public void BuildTower(BaseTower towerPrefab)
    {
        if(tower != null)
        {
            Debug.Log("There is already a tower here!");
            return;
        }

        // If there's no tower, we create a new one from the provided prefab.
        tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        Player.instance.SpendGold(towerPrefab.cost);
        
        // Parents the tower to the spot for organization in the hierarchy.
        tower.transform.parent = transform;
    }

    public void RemoveTower()
    {
        if(tower == null)
        {
            Debug.Log("There's no tower to remove!");
            return;
        }

        Destroy(tower.gameObject);
        tower = null;
    }

    public void BuildTower1()
    {
        Debug.Log("added tower 1");
        BuildTower(TowerBuildMenu.instance.towerPrefabs[0]);
    }

    public void BuildTower2()
    {
        Debug.Log("added tower 2");
        BuildTower(TowerBuildMenu.instance.towerPrefabs[1]);
    }

    public void BuildTower3()
    {
        Debug.Log("added tower 3");
    }

    // TO-DO Upgrade tower options and effects
}