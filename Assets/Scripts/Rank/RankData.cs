using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RankData", menuName ="Data/Rank Data")]
public class RankData : ScriptableObject
{

    [SerializeField]
    private string bestTime;

    [SerializeField]
    private int bestCombo;

    [SerializeField]
    private int bestEnemiesKilled;

    public string BestTime
    {
        get
        {
            return bestTime;
        }
    }

    public int BestCombo
    {
        get
        {
            return bestCombo;
        }
    }

    public int BestEnemiesKilled
    {
        get
        {
            return bestEnemiesKilled;
        }
    }


}
