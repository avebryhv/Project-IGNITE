using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Ranking", menuName = "Level Score Ranking")]
public class LevelScoreRanking : ScriptableObject
{
    public List<float> styleRanks;
    public List<float> damageRanks;
    public List<float> timeRanks;
}
