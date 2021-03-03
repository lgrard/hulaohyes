using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.camera;
using hulaohyes.enemy;
using hulaohyes.inputs;
using Cinemachine;

namespace hulaohyes
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static PlayerController _player0;
        private static PlayerController _player1;
        private static List<EnemyController> _stageEnemyList;

        private CameraManager _camManager;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            else
            {
                Debug.Log("Attempt to create a second GameManager");
                Destroy(this.gameObject);
            }

            Init();
        }

        private void Init()
        {
            _stageEnemyList = new List<EnemyController>();
            _camManager = CameraManager.getInstance();
            foreach (PlayerController lPlayer in FindObjectsOfType<PlayerController>())
            {
                if (lPlayer.playerIndex == 0) _player0 = lPlayer; 
                else if (lPlayer.playerIndex == 1) _player1 = lPlayer;
                else Debug.LogError(lPlayer.gameObject.name + " has invalid player index");
            }
        }

        static GameManager getInstance()
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }

        public static PlayerController getPlayer(int pIndex)
        {
            PlayerController lPlayer = null;

            if (pIndex == 0) lPlayer = _player0;
            else if (pIndex == 1) lPlayer = _player1;
            else Debug.LogError("Invalid player index");

            return lPlayer;
        }

        public static void AddEnemy(EnemyController pEnemy)
        {
            _stageEnemyList.Add(pEnemy);
        }
    }
}