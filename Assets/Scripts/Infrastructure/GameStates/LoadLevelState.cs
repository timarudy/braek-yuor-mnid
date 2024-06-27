using Extensions;
using Infrastructure.AssetManagement;
using Inputs.Services;
using Levels.Common;
using Player;
using Services.Cursor;
using Services.Factory;
using Services.Progress;
using Services.SceneServices;
using Services.StateMachine;
using SoundManagement;
using UnityEngine;

namespace Infrastructure.GameStates
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPlayerPoint = "PlayerStartPoint";
        private const string BlockPosition = "BlockPosition";
        private const string MonitorPosition = "MonitorPosition";

        private readonly ISceneLoader _sceneLoader;
        private readonly IUIInputService _uiInputService;
        private readonly CursorService _cursorService;
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;

        public LoadLevelState(IGameStateMachine stateMachine, ISceneLoader sceneLoader, IUIInputService uiInputService,
            CursorService cursorService, IGameFactory gameFactory, IPersistentProgressService progressService,
            IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _uiInputService = uiInputService;
            _cursorService = cursorService;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
        }

        public void Enter(string sceneName)
        {
            Debug.Log($"Loading {sceneName}");

            SetAudio(sceneName);
            _cursorService.LockCursor();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            _uiInputService.EnableWindows();
            _uiInputService.EnableNotepad();
            _uiInputService.EnableWorldInput();
            _stateMachine.Enter<GameLoopState>();
        }

        private static void SetAudio(string sceneName)
        {
            MusicManager.Instance.AudioSource.clip = MusicManager.Instance.AudioClips
                .Find(clip => clip.Level == sceneName.ToLevel())?.AudioClip;
            MusicManager.Instance.AudioSource.Play();
        }
    }
}