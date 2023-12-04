using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework.Internal;

[System.Serializable]
public class TowerInfo
{
    public GameObject towerPrefab;
    public TextMeshProUGUI towerCountText;
    public Button towerButton;

    public int cost; // The cost of the tower
}

public class TowerSpawner : MonoBehaviour
{
    public List<TowerInfo> towerInfos;

    public TextMeshProUGUI towerArcherCountText;

    public Vector3[] allowedPositions;

    public int currentTowerIndex = 0;

    private Dictionary<Vector3, Dictionary<string, string>> towerPositions = new Dictionary<Vector3, Dictionary<string, string>>();

    public MoneyCounter moneyCounter;

    void Start()
    {
        foreach (var towerInfo in towerInfos)
        {
            towerInfo.towerButton.onClick.AddListener(() => SwitchTower(towerInfo));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                        moneyCounter.SubtractMoney(towerInfos[currentTowerIndex].cost); //deduct specicifc tower cost
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
