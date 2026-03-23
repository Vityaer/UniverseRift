using System.Collections.Generic;
using System.Threading;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Models.Data.Rewards;
using UIController.ItemVisual;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.AsyncUtils;
using Utils.DataCheckUtils;

namespace UIController
{
    public class RewardUIController : MonoBehaviour
    {
        [SerializeField] private List<SubjectCell> Cells = new();
        [SerializeField] private float _delayCell;

        public Transform panelRewards;
        public GameObject btnAllReward;
        private GameReward m_reward;

        private ScrollRect m_scrollRect;
        private CommonDictionaries m_commonDictionaries;
        private CancellationTokenSource m_cancellationTokenSource;
        private readonly CompositeDisposable m_disposables = new();

        public void SetDetailsController(SubjectDetailController subjectDetailController)
        {
            subjectDetailController.ThrowIfArgumentNull();
            foreach (SubjectCell cell in Cells)
                cell.OnSelect.Subscribe(cell => subjectDetailController.ShowData(cell.Subject)).AddTo(m_disposables);
        }

        public void ShowReward(RewardModel rewardData, CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_reward = new GameReward(rewardData, commonDictionaries);
            ShowReward(m_reward, commonDictionaries);
        }

        public void ShowAutoReward(AutoRewardData autoReward, CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_reward = new GameReward(autoReward, commonDictionaries);
            ShowReward(m_reward, commonDictionaries);
        }

        public void Init(ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            m_scrollRect = scrollRect;
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            foreach (SubjectCell cell in Cells)
                cell.Init(scrollRect, m_commonDictionaries);
        }

        public void ShowReward(GameReward reward, CommonDictionaries commonDictionaries, bool lengthReward = false, bool fast = true)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_reward = reward;
            int count = 0;

            if (lengthReward)
                count = reward.Objects.Count;
            else
                count = Mathf.Min(4, reward.Objects.Count);

            if (btnAllReward != null)
                btnAllReward.SetActive(reward.Objects.Count > 4 && lengthReward == false);

            m_cancellationTokenSource.TryCancel();

            if (fast)
            {
                for (int i = 0; i < count; i++)
                {
                    Cells[i].Init(m_scrollRect, m_commonDictionaries);
                    Cells[i].SetData(reward.Objects[i]);
                }


                for (int i = reward.Objects.Count; i < Cells.Count; i++)
                    Cells[i].Disable();

                panelRewards.gameObject.SetActive(reward.Objects.Count > 0);
            }
            else
            {
                m_cancellationTokenSource = new();
                LazyRewardShow(count, m_cancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid LazyRewardShow(int count, CancellationToken token)
        {
            Cells.ForEach(cell => cell.Disable());

            for (int i = 0; i < count; i++)
            {
                Cells[i].SetData(m_reward.Objects[i]);
                await UniTask.Delay(Mathf.RoundToInt(_delayCell * 1000), cancellationToken: token);
            }
        }

        public void OpenAllReward()
        {
            //BoxRewardsPanelController.Instance.ShowAll(reward);
        }

        public void CloseReward()
        {
            gameObject.SetActive(false);
        }

        public void OpenReward()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            m_disposables?.Dispose();
            m_cancellationTokenSource.TryCancel();
        }
    }
}