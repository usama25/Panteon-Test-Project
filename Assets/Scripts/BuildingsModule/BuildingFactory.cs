using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuildingFactory : MonoBehaviour
{

    public Building GetBuilding(string buildingName)
    {
        switch(buildingName){
            
            case "barack":
                return new Barack(); 
             break;
            
            case "powerPlant":
                return new PowerPlant(); 
                break;
             
            case "SoldiersUnit":
                return new SoldiersUnit(); 
                break;
            
            default:
                return null;
             break;

        }
    }
    
}
