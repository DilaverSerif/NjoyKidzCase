using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int GridX;
    public int GridY;
    public float DistanceZ;
    
    public Transform CellTransform;
    private void Awake()
    {
        CreateCells();
        SetCameraByGridSize();
    }

    private void OnEnable()
    {
       Player.OnClickCell += CheckCells;
    }

    private void OnDisable()
    {
        Player.OnClickCell -= CheckCells;
    }

    private void CreateCells()
    {
        transform.position = new Vector3(GridX / -2, GridY / -2, DistanceZ);
        
        for (var x = 0; x < GridX; x++)
        {
            for (var y = 0; y < GridY; y++)
            {
                var cell = Instantiate(CellTransform,  transform.position + new Vector3(x, y, 0), Quaternion.identity);
                cell.transform.name = "Cell_" + x + "_" + y;
                cell.SetParent(transform);
            }
        }
    }
    
    public void CheckCells(Vector3 pos)
    {
        var cell = ExtensionMethods.Raycast2D<Cell>(pos);
        
        if(!cell || cell.isX) return;
        
        cell.OpenX();
        
        var cellArray = Physics2D.BoxCastAll(cell.transform.position,Vector2.one,0,Vector2.zero);
        
        if(cellArray.Length == 0) return;
        
        var cellList = cellArray.ToList().Select(x => x.transform.GetComponent<Cell>()).ToList();
        
        cellList.Remove(cell);
        
        var check = cell.CheckTopBottom(cellList);
        
        if (check.Count == 3)
        {
            foreach (var _cell in check)
            {
                _cell.OpenX(false);
            }
            
            return;
        }

        
        check = cell.CheckLeftRight(cellList);
        
        if (check.Count == 3)
        {
            foreach (var _cell in check)
            {
                _cell.OpenX(false);
            }
            
            return;
        }
        
        check = cell.CheckCross(cellList);
        
        if (check.Count == 3)
        {
            foreach (var _cell in check)
            {
                _cell.OpenX(false);
            }
        }
    }
    
    private void SetCameraByGridSize()
    {
        Cameras.cam1.GetVirtualCamera().virtualCamera.m_Lens.OrthographicSize = GridX - 0.5f;
    }
}
public static class GridExtension
{
    public static List<Cell> CheckTopBottom(this Cell cell,List<Cell> cells)
    {
        var cellPos = cell.transform.position;
        var cellList = new List<Cell> { cell };

        foreach (var _cell in cells)
        {
            if(cellList.Count == 3) break;
            
            for (int y = -1; y < 2; y++)
            {
                if(cellList.Count == 3) break;
                if (y == 0) continue;
                if (cellPos + new Vector3(0, y, 0) != _cell.transform.position) continue;
                
                var cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(0, y, 0));
                if (!cellCheck) return cellList;
                
                if (cellCheck.isX)
                {
                    cellList.Add(cellCheck);
                    
                    //Bir birim ilerisini kontrol et
                    cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(0, y * 2, 0)); 
                    if (!cellCheck) return cellList;
                    
                    if (cellCheck.isX)
                    {
                        cellList.Add(cellCheck);
                        if(cellList.Count == 3) break;
                    }
                    
                    //Tam tersini kontrol et
                    cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(0, y * -1, 0));
                    if (!cellCheck) return cellList;
                    
                    if (cellCheck.isX)
                        cellList.Add(cellCheck);
                }
            }
        } 
        return cellList;
    }
    
    public static List<Cell> CheckLeftRight(this Cell cell,List<Cell> cells)
    {
        var cellPos = cell.transform.position;
        var cellList = new List<Cell> { cell };

        foreach (var _cell in cells)
        {
            if(cellList.Count == 3) break;
            for (int x = -1; x < 2; x++)
            {
                if(cellList.Count == 3) break;
                if (x == 0) continue;
                if (cellPos + new Vector3(x, 0, 0) != _cell.transform.position) continue;
                
                var cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x, 0, 0));
                if (!cellCheck) return cellList;
                
                if(cellCheck.isX)
                {
                    cellList.Add(cellCheck);
                    
                    //Bir birim ilerisini kontrol et
                    cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x * 2, 0, 0));
                    if (!cellCheck) return cellList;
                    
                    if (cellCheck.isX)
                    {
                        cellList.Add(cellCheck);
                        if(cellList.Count == 3) break;
                    }
                        
                    //Tam tersini kontrol et
                    cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x * -1, 0, 0)); 
                    if (!cellCheck) return cellList;
                    
                    if (cellCheck.isX)
                        cellList.Add(cellCheck);
                }
            }
        } 
        return cellList;
    }
    
    public static List<Cell> CheckCross(this Cell cell,List<Cell> cells)
    {
        var cellPos = cell.transform.position;
        var cellList = new List<Cell> { cell };

        foreach (var _cell in cells)
        {
            if(cellList.Count == 3) break;
            cellList = new List<Cell> { cell };

            for (int x = -1; x < 2; x++)
            {
                
                for (int y = -1; y < 2; y++)
                {
                    if(cellList.Count == 3) break;
                    if (x == 0 || y == 0) continue;
                    if (cellPos +  new Vector3(x, y, 0) != _cell.transform.position) continue;
                    
                    var cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x, y, 0));
                    if (!cellCheck) return cellList;
                    
                    if (cellCheck.isX)
                    {
                        cellList.Add(cellCheck);
                        
                        //Bir birim ilerisini kontrol et
                        cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x * 2, y * 2, 0)); 
                        if (!cellCheck) return cellList;
                        
                        if (cellCheck.isX)
                        {
                            cellList.Add(cellCheck);
                            if(cellList.Count == 3) break;
                        }
                        
                        //Tam tersini kontrol et
                        cellCheck = ExtensionMethods.Raycast2D<Cell>(cellPos + new Vector3(x * -1, y * -1, 0)); 
                        if (!cellCheck) return cellList;
                        
                        if (cellCheck.isX)
                            cellList.Add(cellCheck);
                    }
                }
            }
            
        }

        return cellList;
    }
}