using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInputUI : MonoBehaviour
{
    public Button buttonCreatePlayer;
    public Button buttonStart;
    public TMP_InputField inputField;
    public Dropdown playerDropdownList;
    public TextMeshProUGUI bestScoreText;


    // Start is called before the first frame update
    void Start()
    {
        buttonCreatePlayer.onClick.AddListener(ButtonCreatePlayerClicked);
        buttonStart.onClick.AddListener(ButtonStartClicked);
        playerDropdownList.onValueChanged.AddListener(delegate {
            OnPlayerSelected(playerDropdownList);
        });

        RefreshDropdownList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonCreatePlayerClicked()
    {
        string name = inputField.text;
        GameManager.Instance.PlayerManager.SetActivePlayer(name);
        RefreshDropdownList();
        inputField.text = "";
    }

    private void ButtonStartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayerSelected(Dropdown dropdown)
    {
        GameManager.Instance.PlayerManager.SelectActivePlayer(dropdown.value);
    }

    private void SelectPlayerInDropdownList()
    {
        int index = GameManager.Instance.PlayerManager.GetActivePlayerIndex();
        if (index != -1)
        {
            playerDropdownList.value = index;
        }
    }

    private void RefreshDropdownList()
    {
        List<Player> players = GameManager.Instance.PlayerManager.Players;
        if (players != null)
        {
            playerDropdownList.options.Clear();
            foreach (Player player in players)
            {
                playerDropdownList.options.Add(new Dropdown.OptionData() { text = player.Name });
            }
            SelectPlayerInDropdownList();

            Player best = GameManager.Instance.PlayerManager.GetBestPlayer();
            if (best != null)
            {
                bestScoreText.text = best.GetNameAndScore();
            }
            else
            {
                bestScoreText.text = "BEST SCORE: 0";
            }
        }
    }
}
