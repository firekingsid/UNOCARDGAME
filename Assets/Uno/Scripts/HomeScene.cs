using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour {
    [Header ("Mainscreen")]
    public Image playerAvatar1;
    public Image playerAvatar2;
    public Text playerName;
    public Toggle soundToggle;
    [Header ("AvatarSetting")]
    public GameObject avatarSetting;
    public Transform chooseAvatarPanel;
    public Toggle avatarOptionPrefab;
    public InputField playerNameInput;

    private List<Toggle> toggleList;

    void Start () {
        Time.timeScale = 1f;
        SetupUI ();
        if (GameManager.IsFirstOpen) {
            ShowProfileChooser ();
        }
    }

    void SetupUI () {
        soundToggle.isOn = GameManager.IsSound;

        soundToggle.onValueChanged.RemoveAllListeners ();
        soundToggle.onValueChanged.AddListener ((arg0) => {
            GameManager.PlayButton ();
            GameManager.IsSound = arg0;
        });

        toggleList = new List<Toggle> ();
        for (int i = 0; i < GameManager.TOTAL_AVATAR; i++) {
            Toggle temp = Instantiate<Toggle> (avatarOptionPrefab, chooseAvatarPanel);
            temp.group = chooseAvatarPanel.GetComponent<ToggleGroup> ();
            temp.GetComponentsInChildren<Image> () [2].sprite = Resources.Load<Sprite> ("Avatar/" + i);
            int index = i;
            temp.onValueChanged.AddListener ((arg0) => {
                if (arg0) {
                    GameManager.PlayerAvatarIndex = index;
                    UpdateUI ();
                }
            });
            toggleList.Add (temp);
        }
        UpdateUI ();
    }

    void UpdateUI () {
        playerAvatar1.sprite = Resources.Load<Sprite> ("Avatar/" + GameManager.PlayerAvatarIndex);
        playerAvatar2.sprite = Resources.Load<Sprite> ("Avatar/" + GameManager.PlayerAvatarIndex);
        playerName.text = GameManager.PlayerAvatarName;
        playerName.GetComponent<EllipsisText> ().UpdateText ();

    }
    public void ShowProfileChooser () {
        avatarSetting.SetActive (true);
        playerNameInput.text = GameManager.PlayerAvatarName;
        toggleList[GameManager.PlayerAvatarIndex].isOn = true;
    }

    public void OnContine () {
        var inputName = playerNameInput.text.Trim ();
        if (inputName.Length == 0) {
            Toast.instance.ShowMessage ("You need to enter your name");
            return;
        }

        GameManager.IsFirstOpen = false;
        GameManager.PlayerAvatarName = inputName;
        avatarSetting.SetActive (false);
        UpdateUI ();
        GameManager.PlayButton ();
    }

    public void OnComputerPlay () {
        GameManager.currentGameMode = GameMode.Computer;
        GameManager.PlayButton ();
        UnityEngine.SceneManagement.SceneManager.LoadScene ("GameScene");
    }

    public void OnMultiPlayerPlay () {
        EnterMultiplayer ();
        GameManager.PlayButton ();
    }

    private void EnterMultiplayer () {
        GameManager.currentGameMode = GameMode.MultiPlayer;
        UnityEngine.SceneManagement.SceneManager.LoadScene ("GameScene");
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            Application.Quit ();
        }
    }
}