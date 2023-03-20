using UnityEngine;

public class GameSettings : ScriptableObject
{
    public int FireRateLevel = 1;
    public int ShootingDistanceLevel = 1;
    public int ShotsToDestroyLevel = 1;

    public float[] FireRateValues = { 1.5f, 1.1f, 0.7f };
    public float[] ShootingDistanceValues = { 2f, 2.4f, 2.8f };
    public float[] ShotsToDestroyValues = { 3, 2, 1 };

    public int BaseUpgradePrice = 100;
}