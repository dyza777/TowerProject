using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;

    [SerializeField] private TextMeshProUGUI _coinsCountText;
    private int _visibleCoinsCount;
    [HideInInspector] public int CurrentCoinsCount { get; private set; } = 100;

    Coroutine _coinsCounting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _coinsCountText.text = "$ 0";
    }

    void Update()
    {

        if (_visibleCoinsCount != CurrentCoinsCount && _coinsCounting == null)
        {
            _coinsCounting = StartCoroutine(CountUpCoins(_visibleCoinsCount, CurrentCoinsCount));
        }
    }

    IEnumerator CountUpCoins(int startValue, int endValue)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / 0.5f;
            int currentValue = (int)Mathf.Lerp(startValue, endValue, t);
            _visibleCoinsCount = currentValue;
            _coinsCountText.text = "$ " + currentValue.ToString();
            yield return null;
        }
        _coinsCounting = null;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        CurrentCoinsCount += amount;
    }

    public bool TryPurchaseUpgrade(int price)
    {
        if (CurrentCoinsCount < price) return false;
        CurrentCoinsCount -= price;
        return true;
    }
}
