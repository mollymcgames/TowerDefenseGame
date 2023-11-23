using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private int money = 10;

    private void Start()
    {
        LoadMoney(); // Load the money value when the script starts
        UpdateMoneyText(); // Update the money text
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney(); // Save the updated money value
        UpdateMoneyText();
    }

    public void SubtractMoney(int amount)
    {
        money = Mathf.Max(0, money - amount); //ensure the money doesn't go below 0
        SaveMoney(); // Save the updated money value
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = money.ToString();
        // moneyText.text = "Money: £" + money.ToString();

    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 0);
    }

    public void ClearMoney()
    {
        money = 0;
        SaveMoney(); //save the updated money value
        UpdateMoneyText();
    }

    public void GameOver()  //want to make sure this doesnt exist in memory when we go back to the main menu
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
