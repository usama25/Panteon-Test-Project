using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barack : Building
{
    public override void GetBuildingPrefab(GameObject building)
    {
        CreateBuilding(building);
    }
}
