using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private List<GameObject> towers;
    [SerializeField] private GameObject buildSelectorObject;

    public Tilemap placementTilemap;
    public TileBase validTile;

    private TileBase selectedTile;
    private Vector3Int selectedCellpos;

    private PlayableDirector openBuildSelectorDir;
    private PlayableDirector closeBuildSelectorDir;

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

        openBuildSelectorDir = GameObject.Find("Open Build Selector Timeline").GetComponent<PlayableDirector>();
        closeBuildSelectorDir = GameObject.Find("Close Build Selector Timeline").GetComponent<PlayableDirector>();
    }

    public void placeTower(int tower_index)
    {
        GameObject towerPrefab = towers[tower_index];
        var tower = towerPrefab.GetComponent<Tower>();

        if (playerMoney.Value >= tower.cost && selectorOpen)
        {
            Vector3 towerPos = _grid.GetCellCenterWorld(selectedCellpos);

            Instantiate(towerPrefab, towerPos, Quaternion.identity);

            placementTilemap.SetTile(selectedCellpos, null);
            playerMoney.Value -= tower.cost;

            closeBuildSelectorMenu();
        }
    }

    private void openBuildSelectorMenu(Vector3 mousePos)
    {
        openBuildSelectorDir.Play();
        buildSelectorObject.transform.position = new Vector3(mousePos.x, mousePos.y, buildSelectorObject.transform.position.z);
        selectorOpen = true;
    }

    private void closeBuildSelectorMenu()
    {
        closeBuildSelectorDir.Play();
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
                    openBuildSelectorMenu(mousePos);
                }
            }
            else
            {
                closeBuildSelectorMenu();
            }
        }
    }
}
