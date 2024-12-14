using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private List<GameObject> towers;
    [SerializeField] private GameObject towerSelectorObject;

    public Tilemap placementTilemap;
    public TileBase validTile;

    private TileBase selectedTile;
    private Vector3Int selectedCellpos;

    private PlayableDirector openSelectorDir;
    private PlayableDirector closeSelectorDir;

    private Grid _grid;
    private Camera _mainCam;

    private bool selectorOpen;

    [SerializeField] private IntVariable playerMoney;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        selectorOpen = false;
        _grid = placementTilemap.layoutGrid;

        openSelectorDir = GameObject.Find("Open Selector Timeline").GetComponent<PlayableDirector>();
        closeSelectorDir = GameObject.Find("Close Selector Timeline").GetComponent<PlayableDirector>();
    }

    public void placeTower(int tower_type)
    {
        GameObject towerPrefab = towers[tower_type];
        var tower = towerPrefab.GetComponent<Tower>();

        if (playerMoney.Value >= tower.cost && selectorOpen)
        {
            Vector3 towerPos = _grid.GetCellCenterWorld(selectedCellpos);

            Instantiate(towerPrefab, towerPos, Quaternion.identity);

            placementTilemap.SetTile(selectedCellpos, null);
            playerMoney.Value -= tower.cost;

            closeSelectorMenu();
        }
    }

    private void openSelectorMenu(Vector3 mousePos)
    {
        openSelectorDir.Play();
        towerSelectorObject.transform.position = new Vector3(mousePos.x, mousePos.y, towerSelectorObject.transform.position.z);
        selectorOpen = true;
    }

    private void closeSelectorMenu()
    {
        closeSelectorDir.Play();
        selectorOpen = false;
        selectedTile = null;
        selectedCellpos = Vector3Int.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!selectorOpen)
            {
                Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                selectedCellpos = _grid.WorldToCell(mousePos);
                selectedTile = placementTilemap.GetTile(selectedCellpos);

                if (selectedTile)
                {
                    openSelectorMenu(mousePos);
                }
            }
            else
            {
                closeSelectorMenu();
            }
        }
    }
}
