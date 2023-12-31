using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildMenu : MonoBehaviour
{
    public static TowerBuildMenu instance;
    public List<BaseTower> towerPrefabs = new List<BaseTower>();
    public GameObject towerMenu;

    private BuildSpot currentBuildSpot;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        towerMenu.SetActive(false);
    }

    public void SetBuildSpot(BuildSpot buildSpot)
    {
        currentBuildSpot = buildSpot;
    }

    public void BuildTower1()
    {
        if (Player.instance.gold >= towerPrefabs[0].cost)
        {
            Debug.Log("added tower 1");
            currentBuildSpot.BuildTower(towerPrefabs[0]);
        }
        else
        {
            Debug.Log("NOT ENOUGH GOLD!");
        }
    }

    public void BuildTower2()
    {
        if (Player.instance.gold >= towerPrefabs[1].cost)
        {
            Debug.Log("added tower 2");
            currentBuildSpot.BuildTower(towerPrefabs[1]);
        }
        else
        {
            Debug.Log("NOT ENOUGH GOLD!");
        }
    }

    public void BuildTower3()
    {
        if (currentBuildSpot != null && Player.instance.gold >= towerPrefabs[2].cost)
        {
            currentBuildSpot.BuildTower(towerPrefabs[2]);
        }
        else
        {
             Debug.Log("NOT ENOUGH GOLD!");
        }
    }
    
    public void HideMenu()
    {
        towerMenu.SetActive(false);
    }

    public BaseTower CheapestTower()
    {
        BaseTower cheapestTower = null;

        for(int i = 0; i < towerPrefabs.Count; i++)
        {
            if(cheapestTower == null || towerPrefabs[i].cost < cheapestTower.cost)
                cheapestTower = towerPrefabs[i];
        }

        return cheapestTower;
    }
}
