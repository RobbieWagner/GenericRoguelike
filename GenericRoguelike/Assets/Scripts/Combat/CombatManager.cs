using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public enum CombatPhase
{
    NONE = -1,
    COMBAT_START = 0,
    TURN_START = 1,
    SELECTION = 2,
    EXECUTION = 3,
    TURN_END = 5,
    COMBAT_END = 6
}

public class CombatManager : MonoBehaviourSingleton<CombatManager>
{
    public CombatPhase currentCombatPhase;

    // Starts a new combat 
    public void StartCombat(CombatDetails combatDetails)
    {
        // clear any existing combat ui/info
        // set the current combat to the input combat details
        // set currentCombatPhase to COMBAT_START
        // update the ui
        // move to start the first turn
    }

    private void StartNewTurn()
    {
        // trigger any events caused by the start of the new turn
        // move to SELECTION phase
    }

    private void StartUserSelectionPhase()
    {
        // set up ui for user to make selections
    }

    private void StartExecutionPhase()
    {
        // execute actions from user and enemies
    }

    private void EndTurn()
    {
        // tick turn counter up by one, check for combat end
    }

    private void EndCombat()
    {
        // display win/lose based on combat outcome
    }

    private void CheckForCombatEnd()
    {
        // check if the user has run out of hp, or all enemies are eliminated
    }
}