using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionGUI = default;

        public void Awake()
        {
#if DEMO
            versionGUI.text = Application.version + "_D";
#else
            versionGUI.text = Application.version;
#endif
        }
    }
}