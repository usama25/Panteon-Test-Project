using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BuildingFactory buildingFactory;
    public GameObject barack;
    public GameObject powerPlant;
    public GameObject soldiersUnit;
    public void instantiateBarak()
    {
        var building = buildingFactory.GetBuilding("barack");
        building.CreateBuilding(barack);
    }
    
    public void instantiatePowerPlant()
    {
        var building = buildingFactory.GetBuilding("powerPlant");
        building.CreateBuilding(powerPlant);
    }
    
    public void instantiateSoldiersUnit()
    {
        var building = buildingFactory.GetBuilding("SoldiersUnit");
        building.CreateBuilding(soldiersUnit);
    }

}
