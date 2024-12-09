using System;
using UnityEngine;

public class AdManager : MonoBehaviour 
{
    private static string rewardName = ""; 
    public static Action<string, bool> OnRewardShowed; 
    public static void ShowInter()
    {
    }
    public static void ShowReward(string name)
    {
        rewardName = name;
    }
    private void RewardShowComplate(bool state)
    {
        OnRewardShowed?.Invoke(rewardName, state); 
    }
}
