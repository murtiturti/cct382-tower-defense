using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    public Tilemap placementTilemap;
    public TileBase validTile;
    
    private Grid _grid;

    private Camera _mainCam;

    [SerializeField] private IntVariable playerMoney;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        _grid = placementTilemap.layoutGrid;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var tower = towerPrefab.GetComponent<Tower>();
            if (tower != null)
            {
                if (playerMoney.Value >= tower.cost)
                {
                    Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPosition = _grid.WorldToCell(mousePos);
            
                    TileBase clickedTile = placementTilemap.GetTile(cellPosition);
                    if (clickedTile == validTile)
                    {
                        Vector3 towerPos = _grid.GetCellCenterWorld(cellPosition);

                        Instantiate(towerPrefab, towerPos, Quaternion.identity);
                
                        placementTilemap.SetTile(cellPosition, null);
                        playerMoney.Value -= tower.cost;
                    }
                }
            }
        }
    }
}
