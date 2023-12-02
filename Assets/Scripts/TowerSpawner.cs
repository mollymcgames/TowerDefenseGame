using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class TowerInfo
{
    public GameObject towerPrefab;
    public TextMeshProUGUI towerCountText;
    public Button towerButton;

    [SerializeField] private int maxTowersPerTower;
    public int MaxTowerPerTower { get { return maxTowersPerTower; } }

    // //Individual counts for each tower prefab 
    public int currentTowers;
}

public class TowerSpawner : MonoBehaviour
{
    public List<TowerInfo> towerInfos;
    public int maxTowers = 3;
    private int currentTowers = 0;

    public TextMeshProUGUI towerArcherCountText;

    public Vector3[] allowedPositions;

    public int currentTowerIndex = 0;

    private Dictionary<Vector3, Dictionary<string, string>> towerPositions = new Dictionary<Vector3, Dictionary<string, string>>();

    public MoneyCounter moneyCounter;

    void Start()
    {
        UpdateTowerCountText();

        foreach (var towerInfo in towerInfos)
        {
            towerInfo.towerButton.onClick.AddListener(() => SwitchTower(towerInfo));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (towerInfos[currentTowerIndex].currentTowers < towerInfos[currentTowerIndex].MaxTowerPerTower))
        {
            if (moneyCounter.CanAfford(1))
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0;

                foreach (Vector3 position in allowedPositions)
                {
                    if (Vector3.Distance(worldPosition, position) < 0.5f && !IsPositionOccupied(position, towerInfos[currentTowerIndex].towerPrefab.name))
                    {
                        Instantiate(towerInfos[currentTowerIndex].towerPrefab, worldPosition, Quaternion.identity);
                        UpdateTowerPosition(worldPosition, towerInfos[currentTowerIndex].towerPrefab.name);
                        towerInfos[currentTowerIndex].currentTowers++;
                        UpdateTowerCountText();
                        moneyCounter.SubtractMoney(1);
                        break;
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("Not enough money to place a tower");
            }
        }
    }

    private void UpdateTowerCountText()
    {
        foreach (var towerInfo in towerInfos)
        {
            int remainingTowers = towerInfo.MaxTowerPerTower - towerInfo.currentTowers;
            towerInfo.towerCountText.text = remainingTowers.ToString();
        }
    }

    private void SwitchTower(TowerInfo selectedTower)
    {
        int newIndex = towerInfos.IndexOf(selectedTower);
        if (newIndex != -1)
        {
            currentTowerIndex = newIndex;
        }
    }

    public void RemoveTowerPosition(Vector3 position, string towerType)
    {
        if (towerPositions.ContainsKey(position) && towerPositions[position].ContainsKey(towerType))
        {
            towerPositions[position].Remove(towerType);
        }
    }
    private bool IsPositionOccupied(Vector3 position, string towerType)
    {
        if (towerPositions.ContainsKey(position))
        {
            return towerPositions[position].ContainsKey(towerType);
        }
        return false;
    }

    private void UpdateTowerPosition(Vector3 position, string towerType)
    {
        if (!towerPositions.ContainsKey(position))
        {
            towerPositions[position] = new Dictionary<string, string>();
        }
        towerPositions[position][towerType] = "occupied";
    }
}
