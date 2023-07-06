using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;
    public Path path;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}