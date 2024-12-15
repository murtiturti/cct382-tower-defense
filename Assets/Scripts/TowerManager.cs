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
    }

    public void placeTower(int tower_index)
    {
        GameObject towerPrefab = towers[tower_index];
        var tower = towerPrefab.GetComponent<Tower>();

        if (playerMoney.Value >= tower.cost[0] && buildSelectorOpen)
        {
            Vector3 towerPos = _grid.GetCellCenterWorld(selectedCellpos);

            Instantiate(towerPrefab, towerPos, Quaternion.identity);

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

        foreach (TextMeshProUGUI text in towerSelectorObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.gameObject.name == "Upgrade Text")
            {
                text.text = $"-${tower.cost[tower.level]}";
            }
            else
            {
                text.text = $"+${tower.getRefund()}";
            }
        }

        openTowerSelectorDir.Play();
        towerSelectorOpen = true;
    }

    public void closeTowerSelectorMenu()
    {
        closeTowerSelectorDir.Play();
        towerSelectorOpen = false;
        selectedTower = null;
    }

    public void upgradeTower()
    {
        if (towerSelectorOpen)
        {
            Tower tower = selectedTower.GetComponent<Tower>();

            if (playerMoney.Value < tower.cost[tower.level] && tower.level < tower.cost.Length)
            {
                playerMoney.Value -= tower.cost[tower.level];
                selectedTower.GetComponent<Tower>().level += 1;
                closeTowerSelectorMenu();
            }
        }
    }

    public void sellTower()
    {
        if (towerSelectorOpen)
        {
            Tower tower = selectedTower.GetComponent<Tower>();

            playerMoney.Value -= tower.getRefund();
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
        }
    }
}
