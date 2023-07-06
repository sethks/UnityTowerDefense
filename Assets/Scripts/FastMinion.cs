using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMinion : Minion
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        activeMinions++;
    }
}
