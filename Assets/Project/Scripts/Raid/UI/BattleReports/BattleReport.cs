using System.Collections.Generic;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.BattleReports
{
    public class BattleReport : MonoBehaviour, IEditable
    {
        [SerializeField] private Image fadePanel;
        [SerializeField] private GameObject notification;
        [SerializeField] private TextMeshProUGUI notificationTextMesh;
        [SerializeField] private GameObject reportButtonObject;
        [SerializeField] private RewardPanel rewardPanel;

        private Sequence effects;
        private Tween fadeTween;
        private Tween notificationTween;
        
        private readonly Color transParentColor = new (0f, 0f, 0f, 0f);
        private static List<IReward> RewardList => RaidDirector.Battle.RewardList;


        public void OnRaidWin()
        {
            PlayFlag("Victory");
        }

        public void OnRaidDefeat()
        {
            PostProcessingManager.Monochrome(1f);
            PlayFlag("Defeat");
        }

        public void ActiveRewardPanel()
        {
            rewardPanel.gameObject.SetActive(true);
            rewardPanel.Initialize(RewardList);
            notification.SetActive(false);
            reportButtonObject.SetActive(false);
            
            Camp.CollectRewards(RewardList);
        }
        

        private void PlayFlag(string notificationText)
        {
            fadePanel.color           = transParentColor;
            notificationTextMesh.text = notificationText;
            effects                   = DOTween.Sequence();
            
            effects.PrependInterval(1f)
                   .AppendCallback(() =>
                   {
                       fadePanel.gameObject.SetActive(true);
                       notification.gameObject.SetActive(true);
                   })
                   .Append(FadeOut())
                   .Join(Notification())
                   .AppendInterval(2.0f)
                   .AppendCallback(() => reportButtonObject.SetActive(true))
                   .SetLink(gameObject);
        }

        private Tween FadeOut()
        {
            var fadeColor = new Color(0f, 0f, 0f, 0.8f);

            fadeTween = fadePanel.DOColor(fadeColor, 0.35f)
                                 .SetLink(fadePanel.gameObject);

            return fadeTween;
        }

        private Tween Notification()
        {
            notificationTween = notification.transform
                                            .DOScale(1.2f, 0.25f)
                                            .SetLoops(2, LoopType.Yoyo)
                                            .SetLink(notification.gameObject);

            return notificationTween;
        }

        private void OnDestroy()
        {
            if (effects is null) return;
            
            effects.Kill();
            effects = null;
            
            fadeTween?.Kill();
            notificationTween?.Kill();
            
            fadeTween         = null;
            notificationTween = null;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            notification         = transform.Find("Notification").gameObject;
            fadePanel            = transform.Find("FadePanel").GetComponent<Image>();
            notificationTextMesh = notification.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();
            reportButtonObject   = transform.Find("ReportButton").gameObject;
            rewardPanel          = transform.Find("RewardPanel").GetComponent<RewardPanel>();
        }
#endif
    }
}
