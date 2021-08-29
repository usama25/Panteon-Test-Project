using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersUnit : Building
{
    public override void GetBuildingPrefab(GameObject building)
    {
        CreateBuilding(building);
    }
}
