using UnityEngine;
using Strungerhulder.Gameplay.ScriptableObjects;

namespace Strungerhulder.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        //[SerializeField] private QuestManagerSO m_QuestManager = default;

        [SerializeField] private GameStateSO m_GameState = default;


        private void Start()
        {
            StartGame();
        }


        public void PauseGame() { }

        public void UnpauseGame() => m_GameState.ResetToPreviousGameState();


        private void StartGame()
        {
            m_GameState.UpdateGameState(GameState.Gameplay);
            //m_QuestManager.StartGame();
        }
    }
}
