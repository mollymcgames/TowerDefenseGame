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

    public bool CanAfford(int amount)
    {
        return money >= amount; //check if the player has enough money
    }

    public int GetMoney()
    {
        return money;
    }

    private void UpdateMoneyText()
    {
        moneyText.text = money.ToString();

    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    public void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 0);

        //Set the money to at least £10 if it is less than £10
        if (money < 10)
        {
            money = 10;
            SaveMoney(); //save the updated money value
        }
    }

    public void GameOver()  //want to make sure this doesnt exist in memory when we go back to the main menu
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
