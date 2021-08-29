using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    //singletone pattern
    public static GridBuildingSystem current;
    
    public GridLayout gridLayout; 
    public Tilemap MainTilemap; //Main tilemap - for checking placement availability
    public Tilemap TempTilemap; //Temp tilemap - to indicate where the building is now

   
    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    //to keep track of the current building
    [HideInInspector]
    public Building temp; 
    private BoundsInt prevArea; 
    
    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
       
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    private void Update()
    {
        //we don't have a building currently being placed
        if (!temp)
        {
           
            return;
        }
        
   
        //Escape button to cancel the building placement
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
          
            ClearArea();
            
            Destroy(temp.gameObject);
        }
    }

    #endregion

    #region Tilemap management
    
    
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        
        
        foreach (var v in area.allPositionsWithin)
        {
           
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
          
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        
        return array;
    }
    
    
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
       
        FillTiles(tileArray, type);
        
        tilemap.SetTilesBlock(area, tileArray);
    }
    
    /*
     * Fills an array of tiles with the chosen TileType
     */
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    
    #endregion

    #region Building Placement

    
    /*
     * Set previous area that the house was standing on to empty
     */
    public void ClearArea()
    {
        
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
     
        FillTiles(toClear, TileType.Empty);
      
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    /*
     * Highlight the area under the building (on the Temporary tilemap)
     */
    public void FollowBuilding()
    {
        
        ClearArea();

       
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        
        BoundsInt buildingArea = temp.area;

       
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
       
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
           
            if (baseArray[i] == tileBases[TileType.White])
            {
                
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        
        //set the tiles on the temporary tilemap (highlight the area)
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    /*
     * Check if an area is available for placement
     * BoundsInt area - area to check
     */
    public bool CanTakeArea(BoundsInt area)
    {
       
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
       
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                //not white = not available
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    /*
     * Take the area for a building
     */
    public void TakeArea(BoundsInt area)
    {
     
        SetTilesBlock(area, TileType.Empty, TempTilemap);
   
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
    
    #endregion
}

//types of tiles
public enum TileType
{
    Empty, //empty
    White, //available
    Green, //can place
    Red    //can't place
}