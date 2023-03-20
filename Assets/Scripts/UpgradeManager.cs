using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<UpgradeBlock>().SetupBlock(CoinsManager.Instance.CurrentCoinsCount);
        }
    }
}
