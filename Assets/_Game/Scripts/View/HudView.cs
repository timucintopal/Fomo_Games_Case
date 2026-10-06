using ColorBlocks.Core;
using TMPro;
using UnityEngine;

namespace ColorBlocks.View
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI movesText;
        [SerializeField] private TextMeshProUGUI endGameText;

        public void SetLevel(int levelNumber)
        {
            levelText.text = Constants.LevelLabel + levelNumber;
        }

        public void SetMoves(int movesLeft)
        {
            movesText.gameObject.SetActive(true);
            movesText.text = Constants.MovesLabel + movesLeft;
        }

        public void HideMoves()
        {
            movesText.gameObject.SetActive(false);
        }
        
        public void ShowSuccess()
        {
            endGameText.gameObject.SetActive(true);
            endGameText.text = Constants.SuccessLabel;
        }

        public void ShowFail()
        {
            endGameText.gameObject.SetActive(true);
            endGameText.text = Constants.FailLabel;
        }

        public void HideResult()
        {
            endGameText.gameObject.SetActive(false);
        }
    }
}