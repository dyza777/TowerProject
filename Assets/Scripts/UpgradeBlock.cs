using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBlock : MonoBehaviour
{
    [SerializeField] private string _upgradeId;
    [SerializeField] private string _upgradeTitle;

    [SerializeField] private TextMeshProUGUI _upgradeTitleText;
    [SerializeField] private TextMeshProUGUI _upgradeLevelText;
    [SerializeField] private TextMeshProUGUI _upgradeButtonText;
    [SerializeField] private Image _upgradeButtonImage;

    private int maxLevel = 3;
    private int _baseUpgradePrice = 100;
    private int levelValue = 1;

    private void Start()
    {
        var gameSettings = GameManager.Instance.GameSettings;
        var values = (float[])gameSettings.GetType().GetField(_upgradeId + "Values").GetValue(gameSettings);
        maxLevel = values.Length;

        _upgradeTitleText.text = _upgradeTitle;
        _baseUpgradePrice = gameSettings.BaseUpgradePrice;
    }

    public void SetupBlock(int currentCoinsCount)
    {
        if (levelValue >= maxLevel)
        {
            _upgradeButtonText.text = "MAX\nLEVEL";
            _upgradeButtonImage.color = new Color32(38, 95, 137, 255);
        }
        else if (currentCoinsCount < levelValue * _baseUpgradePrice)
        {
            _upgradeButtonText.text = "YOU NEED\n$ " + levelValue * _baseUpgradePrice;
            _upgradeButtonImage.color = new Color32(154, 112, 101, 255);
        }
        else
        {
            _upgradeButtonText.text = "UPGRADE\n$ " + levelValue * _baseUpgradePrice;
            _upgradeButtonImage.color = new Color32(76, 135, 101, 255);
        }
        _upgradeLevelText.text = "LVL " + levelValue;
    }

    public void OnPressUpgrade()
    {
        if (levelValue >= maxLevel || !CoinsManager.Instance.TryPurchaseUpgrade(_baseUpgradePrice * levelValue)) return;
        levelValue++;
        GameManager.Instance.HandleUpgrade(_upgradeId);
    }
}
