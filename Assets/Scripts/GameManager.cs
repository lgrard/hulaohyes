using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.camera;
using hulaohyes.enemy;
using hulaohyes.levelbrick.checkpoint;

namespace hulaohyes
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static PlayerController _player0;
        private static PlayerController _player1;
        private static List<EnemyController> _stageEnemyList;

        private CameraManager _camManager;

        private PlayerStart playerStart;
        private Checkpoint currentCheckPoint0;
        private Checkpoint currentCheckPoint1;
        private Checkpoint currentCheckPointGlobal;
        private Checkpoint[] CheckPointList;


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

            playerStart = PlayerStart.getInstance();
            CheckPointList = FindObjectsOfType<Checkpoint>();
        }

        public static GameManager getInstance()
        {
            if (_instance == null) _instance = new GameManager();
            return _instance;
        }

        public static PlayerController getPlayer(int pIndex)
        {
            PlayerController lPlayer = pIndex ==0? _player0: _player1;
            return lPlayer;
        }

        public void SpawnPlayer(int pIndex)
        {
            PlayerController lPlayerToSpawn = getPlayer(pIndex);
            Checkpoint lSpawner = null;
            Vector3 lSpawnPos;
            Quaternion lSpawnRot;

            if (currentCheckPoint0 != null && pIndex == 0)
            {
                if(currentCheckPointGlobal != null && currentCheckPoint0.CheckPointIndex > currentCheckPointGlobal.CheckPointIndex || currentCheckPointGlobal == null)
                    lSpawner = currentCheckPoint0;
            }

            else if (currentCheckPoint1 != null && pIndex == 1)
            {
                if (currentCheckPointGlobal != null && currentCheckPoint1.CheckPointIndex > currentCheckPointGlobal.CheckPointIndex || currentCheckPointGlobal == null)
                    lSpawner = currentCheckPoint1;
            }

            else if (currentCheckPointGlobal != null)
                lSpawner = currentCheckPointGlobal;

            if(lSpawner != null)
            {
                lSpawnPos = lSpawner.SpawnPosition;
                lSpawnRot = lSpawner.SpawnRotation;
            }

            else
            {
                lSpawnPos = playerStart.SpawnPosition;
                lSpawnRot = playerStart.SpawnRotation;
            }

            lPlayerToSpawn.transform.position = lSpawnPos;
            lPlayerToSpawn.transform.rotation = lSpawnRot;
            lPlayerToSpawn.Spawn();
        }

        private void SpawnAllplayers()
        {
            if(currentCheckPointGlobal != null)
            {
                BigCheckpoint lGlobalCp = currentCheckPointGlobal as BigCheckpoint;

                _player0.transform.position = lGlobalCp.SpawnPosition;
                _player0.transform.rotation = lGlobalCp.SpawnRotation;
                _player0.gameObject.SetActive(true);

                _player1.transform.position = lGlobalCp.SecondSpawnPosition;
                _player1.transform.rotation = lGlobalCp.SecondSpawnRotation;
                _player1.gameObject.SetActive(true);
            }
        }

        public void SetCurrentSpawner(int pPlayerIndex, Checkpoint pCheckpoint)
        {
            if(pCheckpoint is BigCheckpoint)
                currentCheckPointGlobal = pCheckpoint;

            else
            {
                if (pPlayerIndex == 0) currentCheckPoint0 = pCheckpoint;
                else if (pPlayerIndex == 1) currentCheckPoint1 = pCheckpoint;
            }

            foreach (Checkpoint lCp in CheckPointList)
                if (lCp.CheckPointIndex < pCheckpoint.CheckPointIndex && lCp.enabled) lCp.enabled = false;
        }


        public static void AddEnemy(EnemyController pEnemy)
        {
            _stageEnemyList.Add(pEnemy);
        }
    }
}