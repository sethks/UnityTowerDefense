using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    // Reference to the tower that is currently built on this spot.
    [HideInInspector]
    public BaseTower tower;
    public GameObject towerMenu;

    // offset for barrackstower
    public Vector3 offset1 = new Vector3(1, 0, 0);
    public Vector3 offset2 = new Vector3(-1, 0, 0);
    private void Awake()
    {
        tower = null;
    }

    public void OnBuildSpotClicked()
    {
        // Check if there is already a tower built here.
        if(tower != null)
        {
            Debug.Log("There is already a tower here!");
            return;
        }

        TowerBuildMenu.instance.HideMenu();

        towerMenu = TowerBuildMenu.instance.towerMenu;
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

        if(Player.instance.gold >= towerPrefab.cost)
        {
            // If there's no tower, we create a new one from the provided prefab.
            tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);

            if(tower is BarracksTower barracksTower)
            {
                barracksTower.offset1 = offset1;
                barracksTower.offset2 = offset2;
            }
            Player.instance.SpendGold(towerPrefab.cost);
        
            // Parents the tower to the spot for organization in the hierarchy.
            tower.transform.parent = transform;
            TowerBuildMenu.instance.HideMenu();
        }
        else
        {
            Debug.Log("NO GOLD MF");
            TowerBuildMenu.instance.HideMenu();
        }
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
        BuildTower(TowerBuildMenu.instance.towerPrefabs[2]);
    }

    // TO-DO Upgrade tower options and effects
}
