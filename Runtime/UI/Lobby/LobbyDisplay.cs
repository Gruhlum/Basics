using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class LobbyDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text lobbyNameGUI = default;
        [SerializeField] private TMP_Text hostNameGUI = default;
        [SerializeField] private TMP_Text playerCountGUI = default;

        public void Setup(string lobbyName, string hostName, int currentPlayers, int maxPlayers)
        {
            lobbyNameGUI.text = lobbyName;
            hostNameGUI.text = hostName;
            playerCountGUI.text = GeneratePlayerCountText(currentPlayers, maxPlayers);
        }
        public void OnJoinButtonPressed()
        {

        }
        private string GeneratePlayerCountText(int currentPlayers, int maxPlayers)
        {
            return $"{currentPlayers}/{maxPlayers}";
        }
    }
}