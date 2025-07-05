using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionGUI = default;

        public void Awake()
        {
            versionGUI.text = Application.version;
        }
    }
}