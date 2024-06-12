using Assets.Scripts.LevelManagerFolder;
using Assets.Scripts.Player;
using Cinemachine;
using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance => _instance ??= FindAnyObjectByType<GameManager>();

        [SerializeField] private LevelManager levelManager;
        [SerializeField] private PlayerSlideController playerPrefab;
        [SerializeField] private CinemachineVirtualCamera cinemachine;
        [SerializeField] private HUD hud;

        private PlayerObject player;


        private void Awake()
        {
            GameManager._instance = this;
            StartupPrcedures();
        }

        private void Start()
        {
            RestartLevel();
        }

        private void SetupPlayer()
        {
            var spawn = levelManager.LoadedLevel?.GetSpawnPoint();
            if (!player)
            {
                var obj = Instantiate(playerPrefab);
                player = obj.GetComponentInChildren<PlayerObject>();
            }
            player.PlayerSlideController.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);

            hud.DragControlls.SetupPlayer(player);
            player.PlayerSlideController.CanMove = false;
            player.FactoryReset();

            cinemachine.LookAt = player.transform;
            cinemachine.Follow = player.transform;

            startMovingSub = hud.DragControlls.DragStart.Where(x => x != false).Subscribe((x) =>
                {
                    player.StartGame();
                    startMovingSub?.Dispose();

                }).AddTo(gameObject);
        }

        private void StartupPrcedures()
        {

        }

        IDisposable startMovingSub;
        private void PreLevelSetup()
        {
            levelManager.RestartLevel();
            SetupPlayer();

            hud.SetInGameState(true);
            hud.PreLevelStart(LevelManager.CurrentLevel, player);
        }


        public void LevelFail()
        {
            player.Lose();
            hud.SetInGameState(false);
            hud.ShowFail(() => RestartLevel());
        }

        private void RestartLevel()
        {
            levelManager.RestartLevel();
            PreLevelSetup();
        }

        public void LevelComplete()
        {
            player?.Win();

            hud.SetInGameState(false);
            hud.ShowWin(() => LevelAdvance());
        }

        private void LevelAdvance()
        {
            levelManager.NextLevel();
            PreLevelSetup();
        }
    }

}