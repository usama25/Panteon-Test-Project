using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building
{
    public override void GetBuildingPrefab(GameObject building)
    {
        CreateBuilding(building);
    }
}
