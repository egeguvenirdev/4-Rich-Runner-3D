using Ali.Helper;
using UnityEngine;

namespace Ali.Helper
{
    public class HCLevelManager : GenericSingleton<HCLevelManager>
    {
        [SerializeField] private GameObject[] _levelPrefabs;
        [SerializeField] private int _levelIndex = 0;
        [SerializeField] private bool _forceLevel = false;
        private Bermuda.Runner.BermudaRunnerCharacter bermudaRunnerCharacter;

        private int _globalLevelIndex = 0;
        private bool _inited = false;
        private GameObject _currentLevel;

        private void Start()
        {
            //Init();
            GenerateCurrentLevel();
            bermudaRunnerCharacter.Init();
        }

        public void Init()
        {
            if (_inited)
            {
                return;
            }
            _inited = true;
            //PlayerPrefs.DeleteAll();
            _globalLevelIndex = PlayerPrefs.GetInt("HCLevel", 0);

            if (_forceLevel)
            {
                _globalLevelIndex = _levelIndex;
                return;
            }
            _levelIndex = _globalLevelIndex;

            if (_levelIndex >= _levelPrefabs.Length)
            {
                _levelIndex = GameUtility.RandomIntExcept(_levelPrefabs.Length, _levelIndex, 0);
            }
        }

        public void GenerateCurrentLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel);
            }
            _currentLevel = Instantiate(_levelPrefabs[_levelIndex]);

            bermudaRunnerCharacter = FindObjectOfType<Bermuda.Runner.BermudaRunnerCharacter>();
            bermudaRunnerCharacter.Init();
        }

        public GameObject GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void LevelUp()
        {
            if (_forceLevel)
            {
                return;
            }
            _globalLevelIndex++;
            PlayerPrefs.SetInt("HCLevel", _globalLevelIndex);
            _levelIndex = _globalLevelIndex;
            if (_levelIndex >= _levelPrefabs.Length)
            {
                _levelIndex = GameUtility.RandomIntExcept(_levelPrefabs.Length, _levelIndex);
            }

        }
        public int GetGlobalLevelIndex()
        {
            return _globalLevelIndex;
        }
    }
}