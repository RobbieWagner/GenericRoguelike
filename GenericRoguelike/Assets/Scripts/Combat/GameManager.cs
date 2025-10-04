using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void EnterCombatMode(CombatDetails combatDetails = null)
    {
        CombatManager.Instance.StartCombat(combatDetails);
    }
}
