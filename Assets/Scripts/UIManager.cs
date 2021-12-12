using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Button _restartButton;
    private Button _playButton;
    [SerializeField] private Text _gemCountText;

    private void Start()
    {
        Button[] buttonList = GetComponentsInChildren<Button>(true);
        _playButton = buttonList[0];
        _restartButton = buttonList[1];
        _gemCountText.text = StoreManager.Instance.GemCount.ToString();
        PlayUI();
    }
    public void RestartUI()
    {
        _restartButton.gameObject.SetActive(true);
        _playButton.gameObject.SetActive(false);
    }

    public void RunningUI()
    {
        _restartButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
    }

    public void PlayUI()
    {
        _restartButton.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(true);
    }

    public void SetGemCount()
    {
        _gemCountText.text = StoreManager.Instance.GemCount.ToString();
    }
}