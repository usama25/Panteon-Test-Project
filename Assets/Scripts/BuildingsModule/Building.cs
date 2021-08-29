using UnityEngine;

public abstract class Building : MonoBehaviour
{ 
    public  bool Placed { get; private set; }
    public BoundsInt area;
   
    
    #region Movement
   
    private Vector3 startPos;
    
    private float deltaX, deltaY;
    
    private void OnMouseDown()
    {
        //only respond if the building is not placed
        if (!Placed)
        {
           
            startPos = Input.mousePosition;
          
            startPos = Camera.main.ScreenToWorldPoint(startPos);
        
        
            deltaX = startPos.x - transform.position.x;
            deltaY = startPos.y - transform.position.y;
        }
    }

    private void OnMouseDrag()
    {
        //only respond if the building is not placed
        if (!Placed)
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           
            transform.position = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY, 0);
            
            GridBuildingSystem.current.FollowBuilding();
        }
    }

    private void OnMouseUp()
    {
        //only respond if the building is not placed
        if (!Placed)
        {
            
            Vector3Int cellPosition = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
          
            transform.localPosition = GridBuildingSystem.current.gridLayout.
                CellToLocalInterpolated(cellPosition + new Vector3(.5f, .5f, 0f));
            
            if (CanBePlaced())
            {
                //yes, we can
                Place();
            }
        }
    }

    #endregion
    
    #region Build Methods

    /*
     * Check if the building can be placed at its current position
     */
    public bool CanBePlaced()
    {
        //create an area under the building
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //call the GridBuildingSystem to check the area
        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    /*
     * Make the building placed
     */
    public void Place()
    {
        //create an area under the building
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        
        Placed = true;
        
        //call the GridBuildingSystem to take the area
        GridBuildingSystem.current.TakeArea(areaTemp);
    }
    
    #endregion
    
    public abstract void GetBuildingPrefab(GameObject building);
   
    public void CreateBuilding(GameObject _building)
    {
        Vector3 position = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(new Vector3(.5f, .5f, 0f));
       
        GridBuildingSystem.current.temp = Instantiate(_building, position, Quaternion.identity).GetComponent<Building>();
     
        GridBuildingSystem.current.FollowBuilding();
    }
  

}


