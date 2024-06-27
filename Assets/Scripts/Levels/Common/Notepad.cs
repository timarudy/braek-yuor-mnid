using System;
using System.Text.RegularExpressions;
using Configs;
using Inputs.Services;
using Services.LevelAccess;
using Services.Progress;
using Services.SceneServices;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Levels.Common
{
    public class Notepad : MonoBehaviour, IVisibility
    {
        [SerializeField] private TextMeshProUGUI Text;
        [SerializeField] private TextMeshProUGUI Code;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI PageNumber;
        [SerializeField] private Button PreviousPageButton;
        [SerializeField] private Button NextPageButton;

        private SONotepad _notepadData;
        private ILevelDataService _levelData;
        private bool _isCodeGiven;
        private int _currentPageNumber;
        private string _currentPageText;
        private IUIInputService _uiInputService;
        private ICodeAccessChecker _codeAccessChecker;

        [Inject]
        private void Construct(ILevelDataService levelData, IUIInputService uiInputService,
            ICodeAccessChecker codeAccessChecker)
        {
            _uiInputService = uiInputService;
            _levelData = levelData;
            _codeAccessChecker = codeAccessChecker;
        }

        private void Awake()
        {
            _notepadData = GetNotepadData();
            _currentPageNumber = _notepadData.NotepadData[0].PageNumber;
            _currentPageText = _notepadData.NotepadData[0].Text;
            Title.text = FormatLevelTitle(_levelData.GetLevelData(SceneManager.GetActiveScene().name).LevelName);
            Code.text = _levelData.GetLevelData(SceneManager.GetActiveScene().name).LevelCode;

            InitPage(_currentPageText, _currentPageNumber);

            // For SaveLoad
            if (_isCodeGiven)
                Code.gameObject.SetActive(true);

            SetNextPageButton();
            SetPreviousPageButton();
            
            PreviousPageButton.onClick.AddListener(ToPreviousPage);
            NextPageButton.onClick.AddListener(ToNextPage);
        }

        private void InitPage(string text, int pageNumber)
        {
            if (_notepadData.NotepadData[pageNumber - 1].PageType == PageType.TITLE)
            {
                if (_isCodeGiven)
                    Code.gameObject.SetActive(true);
            }
            else
            {
                Code.gameObject.SetActive(false);
            }

            Text.text = $"    {text}";
            PageNumber.text = pageNumber.ToString();
        }

        private void ToPreviousPage()
        {
            _currentPageNumber--;
            _currentPageText = _notepadData.NotepadData[_currentPageNumber - 1].Text;
            InitPage(_currentPageText, _currentPageNumber);
            SetNextPageButton();
            SetPreviousPageButton();
        }

        private void ToNextPage()
        {
            _currentPageNumber++;
            _currentPageText = _notepadData.NotepadData[_currentPageNumber - 1].Text;
            InitPage(_currentPageText, _currentPageNumber);
            SetNextPageButton();
            SetPreviousPageButton();
        }

        private void SetPreviousPageButton()
        {
            if (_currentPageNumber == 1)
            {
                PreviousPageButton.gameObject.SetActive(false);
            }
            else
            {
                PreviousPageButton.gameObject.SetActive(true);
            }
        }

        private void SetNextPageButton()
        {
            if (_notepadData.NotepadData.Count > _currentPageNumber)
            {
                NextPageButton.gameObject.SetActive(true);
            }
            else
            {
                NextPageButton.gameObject.SetActive(false);
            }
        }

        public void Open()
        {
            TryGiveCode(_codeAccessChecker.CheckAccess());
            _uiInputService.DisableWorldInput();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            _uiInputService.EnableWorldInput();
            gameObject.SetActive(false);
        }

        private void TryGiveCode(bool access)
        {
            Debug.Log("Access Given");
            if (access && !_isCodeGiven)
            {
                Code.gameObject.SetActive(true);
                _isCodeGiven = true;
            }
        }

        private SONotepad GetNotepadData() =>
            _levelData.GetLevelData(SceneManager.GetActiveScene().name).NotepadData;

        private string FormatLevelTitle(string input)
        {
            input = input.Substring(0, Math.Max(0, input.Length - 5));
            string output = Regex.Replace(input, @"(\p{Lu})", " $1");

            return output;
        }
    }
}