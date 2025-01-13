using City.Panels.BoxRewards;
using City.Panels.SubjectPanels.Common;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.Data.Rewards;
using System;
using System.Collections.Generic;
using System.Threading;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using Utils.AsyncUtils;

namespace UIController
{
    public class RewardUIController : MonoBehaviour
    {
        [SerializeField] private List<SubjectCell> Cells = new();
        [SerializeField] private float _delayCell;

        public Transform panelRewards;
        public GameObject btnAllReward;
        private GameReward _reward;

        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables = new();

        public void SetDetailsController(SubjectDetailController subjectDetailController)
        {
            foreach (var cell in Cells)
                cell.OnSelect.Subscribe(cell => subjectDetailController.ShowData(cell.Subject)).AddTo(_disposables);
        }

        public void ShowReward(RewardModel rewardData, CommonDictionaries commonDictionaries)
        {
            _reward = new GameReward(rewardData, commonDictionaries);
            ShowReward(_reward);
        }

        public void ShowAutoReward(AutoRewardData autoReward, CommonDictionaries commonDictionaries)
        {
            _reward = new GameReward(autoReward, commonDictionaries);
            ShowReward(_reward);
        }

        public void ShowReward(GameReward reward, bool lengthReward = false, bool fast = true)
        {

            _reward = reward;
            var count = 0;

            if (lengthReward)
            {
                count = reward.Objects.Count;
            }
            else
            {
                count = Mathf.Min(4, reward.Objects.Count);
            }

            if (btnAllReward != null)
                btnAllReward.SetActive(reward.Objects.Count > 4 && lengthReward == false);

            _cancellationTokenSource.TryCancel();

            if (fast)
            {
                for (int i = 0; i < count; i++)
                    Cells[i].SetData(reward.Objects[i]);

                for (int i = reward.Objects.Count; i < Cells.Count; i++)
                    Cells[i].Disable();

                panelRewards.gameObject.SetActive(reward.Objects.Count > 0);
            }
            else
            {
                _cancellationTokenSource = new();
                LazyRewardShow(count, _cancellationTokenSource.Token).Forget();
            }
        }

        private async UniTaskVoid LazyRewardShow(int count, CancellationToken token)
        {
            Cells.ForEach(cell => cell.Disable());

            for (int i = 0; i < count; i++)
            {
                Cells[i].SetData(_reward.Objects[i]);
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
            _disposables?.Dispose();
            _cancellationTokenSource.TryCancel();
        }
    }
}