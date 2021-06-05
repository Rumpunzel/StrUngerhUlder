using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Gameplay,// regular state: player moves, attacks, can perform actions
    Pause,// pause menu is opened, the whole game world is frozen
    Inventory, //when inventory UI or cooking UI are open
    Dialogue,
    Cutscene,
    LocationTransition,// when the character steps into LocationExit trigger, fade to black begins and control is removed from the player
    Combat,//enemy is nearby and alert, player can't open Inventory or initiate dialogues, but can pause the game
}

[CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 51)]
public class GameStateSO : ScriptableObject 
{
    public GameState currentGameState => m_CurrentGameState;

    private GameState m_CurrentGameState = default;
    private GameState m_PreviousGameState = default; 


    public void UpdateGameState( GameState newGameState)
	{
           m_PreviousGameState = m_CurrentGameState; 
           m_CurrentGameState = newGameState; 
    }

    public void ResetToPreviousGameState() => m_CurrentGameState = m_PreviousGameState;
}
