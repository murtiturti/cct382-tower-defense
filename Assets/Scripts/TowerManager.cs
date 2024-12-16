using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;
using TMPro;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance;

    [SerializeField] private List<GameObject> towers;
    [SerializeField] private GameObject buildSelectorObject;
    [SerializeField] private GameObject towerSelectorObject;

    public Tilemap placementTilemap;
    public TileBase validTile;

    private TileBase selectedTile;
    private Vector3Int selectedCellpos;
    private GameObject selectedTower;

    private PlayableDirector openBuildSelectorDir;
    private PlayableDirector closeBuildSelectorDir;
    private PlayableDirector openTowerSelectorDir;
    private PlayableDirector closeTowerSelectorDir;

    private TextMeshProUGUI upgradeText;
    private TextMeshProUGUI sellText;

    private Grid _grid;
    private Camera _mainCam;

    private bool buildSelectorOpen;
    private bool towerSelectorOpen;

    [SerializeField] private IntVariable playerMoney;

    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(this);
        _mainCam = Camera.main;
    }

    private void Start()
    {
        buildSelectorOpen = false;
        towerSelectorOpen = false;
        _grid = placementTilemap.layoutGrid;

        openBuildSelectorDir = GameObject.Find("Open Build Selector Timeline").GetComponent<PlayableDirector>();
        closeBuildSelectorDir = GameObject.Find("Close Build Selector Timeline").GetComponent<PlayableDirector>();

        openTowerSelectorDir = GameObject.Find("Open Tower Selector Timeline").GetComponent<PlayableDirector>();
        closeTowerSelectorDir = GameObject.Find("Close Tower Selector Timeline").GetComponent<PlayableDirector>();

        upgradeText = GameObject.Find("Upgrade Text").GetComponent<TextMeshProUGUI>();
        sellText = GameObject.Find("Sell Text").GetComponent<TextMeshProUGUI>();
    }

    public void placeTower(int tower_index)
    {
        GameObject towerPrefab = towers[tower_index];
        var tower = towerPrefab.GetComponent<Tower>();

        if (playerMoney.Value >= tower.cost[0] && buildSelectorOpen)
        {
            Vector3 towerPos = _grid.GetCellCenterWorld(selectedCellpos);

            var towerInstance = Instantiate(towerPrefab, towerPos, Quaternion.identity);
            
            Tower placedTower = towerInstance.GetComponent<Tower>();
            placedTower.SetTilePosition(selectedCellpos);

            placementTilemap.SetTile(selectedCellpos, null);
            playerMoney.Value -= tower.cost[0];

            closeBuildSelectorMenu();
        }
    }

    public void clickOnTower(GameObject tower)
    {
        towerSelectorOpen = false;
        selectedTower = tower;
        
        towerSelectorObject.transform.position = new Vector3(tower.transform.position.x, tower.transform.position.y, towerSelectorObject.transform.position.z);

        openTowerSelectorMenu();
    }

    public void openTowerSelectorMenu()
    {
        if (buildSelectorOpen)
        {
            closeBuildSelectorMenu();
        }

        Tower tower = selectedTower.GetComponent<Tower>();
        tower.ToggleRange(true);

        if (tower.level >= 3)
        {
            upgradeText.text = $"";
        }
        else
        {
            upgradeText.text = $"-${tower.cost[tower.level]}";
        }

        sellText.text = $"+${tower.getRefund()}";

        openTowerSelectorDir.Play();
        towerSelectorOpen = true;
    }

    public void closeTowerSelectorMenu()
    {
        closeTowerSelectorDir.Play();
        towerSelectorOpen = false;
        selectedTower.GetComponent<Tower>().ToggleRange(false);
        selectedTower = null;
    }

    public void upgradeTower()
    {
        if (towerSelectorOpen)
        {
            Tower tower = selectedTower.GetComponent<Tower>();

            if (tower.level < 3)
            {
                if (playerMoney.Value >= tower.cost[tower.level])
                {
                    playerMoney.Value -= tower.cost[tower.level];
                    selectedTower.GetComponent<Tower>().level += 1;
                    closeTowerSelectorMenu();
                }
            }
        }
    }

    public void sellTower()
    {
        if (towerSelectorOpen)
        {
            Tower tower = selectedTower.GetComponent<Tower>();
            
            Vector3Int towerTilePos = new Vector3Int(tower.x, tower.y, tower.z);

            playerMoney.Value += tower.getRefund();
            placementTilemap.SetTile(towerTilePos, validTile);
            Destroy(selectedTower);
            closeTowerSelectorMenu();
            
        }
    }

    private void openBuildSelectorMenu(Vector3 mousePos)
    {
        if (towerSelectorOpen)
        {
            closeTowerSelectorMenu();
        }

        openBuildSelectorDir.Play();
        buildSelectorObject.transform.position = new Vector3(mousePos.x, mousePos.y, buildSelectorObject.transform.position.z);
        buildSelectorOpen = true;
    }

    private void closeBuildSelectorMenu()
    {
        closeBuildSelectorDir.Play();
        buildSelectorOpen = false;
        selectedTile = null;
        selectedCellpos = Vector3Int.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!buildSelectorOpen)
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

            if (towerSelectorOpen)
            {
                closeTowerSelectorMenu();
            }
        }
    }
}
