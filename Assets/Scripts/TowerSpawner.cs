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

    private string[] towerPositions = null;
    public MoneyCounter moneyCounter;

    [SerializeField] private AudioSource placeTowerSoundEffect;

    void Start()
    {
        foreach (var towerInfo in towerInfos)
        {
            towerInfo.towerButton.onClick.AddListener(() => SwitchTower(towerInfo));
        }

        //create our store of used tower positions
        towerPositions = new string[allowedPositions.Length];
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


                int positionNumber = DeriveTowerPosition(worldPosition, allowedPositions);
                if (positionNumber >= 0 && !IsPositionOccupied(positionNumber))
                {
                    Instantiate(towerInfos[currentTowerIndex].towerPrefab, worldPosition, Quaternion.identity);
                    UpdateTowerPosition(positionNumber);
                    moneyCounter.SubtractMoney(towerInfos[currentTowerIndex].cost); //deduct specicifc tower cost
                    placeTowerSoundEffect.Play(); //play the sound effect
                }
                else
                {
                    UnityEngine.Debug.Log("Position is occupied");
                }
            }
        }
    }

    public int DeriveTowerPosition(Vector3 worldPosition)
    {
        return DeriveTowerPosition(worldPosition, allowedPositions);
    }

    private int DeriveTowerPosition(Vector3 worldPosition, Vector3[] allowedPositions)
    {
        int positionNumber = 0;
        foreach (Vector3 position in allowedPositions)        
        {
            if (Vector3.Distance(worldPosition, position) < 0.5f )
            {
                return positionNumber;
            }
            positionNumber++;
        }
        return -1; //no position found
    }

    private void SwitchTower(TowerInfo selectedTower)
    {
        int newIndex = towerInfos.IndexOf(selectedTower);
        if (newIndex != -1)
        {
            currentTowerIndex = newIndex;
        }
    }

    public void RemoveTowerPosition(int position)
    {
        towerPositions[position] = null;
    }
    private bool IsPositionOccupied(int position)
    {
        return towerPositions[position] != null;
    }

    private void UpdateTowerPosition(int position)
    {
        towerPositions[position] = "occupied";
    }
}
