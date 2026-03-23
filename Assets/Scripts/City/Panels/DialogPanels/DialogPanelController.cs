using System.Threading;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using LocalizationSystems;
using Models.Dialogs;
using Plugins.VContainerUi.Runtime.Abstraction;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;
using VContainerUi.Abstraction;

namespace City.Panels.DialogPanels
{
    public class DialogPanelController : UiController<DialogPanelView>
    {
        private const string ZERO_ALPHA = "<alpha=#00>";

        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        
        private CancellationTokenSource m_cancellationTokenSource;
        private DialogModel m_currentDialog;
        private int m_currentStatementIndex;
        private Tween m_tween;

        private bool m_isFinish;

        public async UniTask ShowDialogWithoutBlock(string dialogId, CancellationToken cancellationToken)
        {
            if (!m_commonDictionaries.Dialogs.TryGetValue(dialogId, out DialogModel dialog))
            {
                return;
            }

            await ShowDialog(dialog, cancellationToken);
        }
        
        public async UniTask ShowDialog(string dialogId, CancellationToken cancellationToken)
        {
            if (dialogId == null)
            {
                Debug.LogError("dialogId is null");
                return;
            }

            if (!m_commonDictionaries.Dialogs.TryGetValue(dialogId, out DialogModel dialog))
            {
                return;
            }

            await ShowDialog(dialog, cancellationToken);
        }

        private async UniTask ShowDialog(DialogModel dialog, CancellationToken cancellationToken)
        {
            if (dialog == null)
            {
                StopDialog();
                return;
            }

            if (dialog.Statements == null)
            {
                StopDialog();
                return;
            }
            
            if(dialog.Statements.Count == 0)
            {
                StopDialog();
                return;
            }

            if (m_currentDialog != null)
                StopDialog();
            
            m_isFinish = false;

            m_currentDialog = dialog;
            m_currentStatementIndex = -1;
            m_cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Show();
            ShowNextStatement();
            
            await UniTask.WaitUntil(() => m_isFinish, cancellationToken: m_cancellationTokenSource.Token);
        }
        
        private void ShowNextStatement()
        {
            if(m_cancellationTokenSource == null || m_cancellationTokenSource.Token.IsCancellationRequested)
                return;

            m_currentStatementIndex += 1;
            ShowStatement(m_currentStatementIndex, m_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid ShowStatement(int currentStatementIndex, CancellationToken token)
        {
            PersonalStatement statement = m_currentDialog.Statements[currentStatementIndex];
            if (!string.IsNullOrEmpty(statement.PersonId))
            {
                View.NPCName.StringReference = m_localizationSystem.LocalizationUiContainer
                    .GetLocalizedContainer(statement.PersonId);
            }

            if (statement.PhraseIds.Count > currentStatementIndex)
            {
                string sentence = await m_localizationSystem.LocalizationUiContainer.GetStringAsync(statement.PhraseIds[currentStatementIndex]); 
                await PrintText(sentence, token);
                await View.NextSentenceButton.OnClickAsAsyncEnumerable().FirstAsync(token);
            }

            if (m_currentStatementIndex < m_currentDialog.Statements.Count - 1)
            {
                ShowNextStatement();
            }
            else
            {
                m_isFinish = true;
                Hide();
            }
        }

        private async UniTask PrintText(string text, CancellationToken token)
        {
            int index = 0;
            int textLength = text.Length;
            int delay = Mathf.RoundToInt(View.PrintSymbolAnimationTime * 1000);
            
            while (!token.IsCancellationRequested && index <= textLength)
            {
                await UniTask.Delay(delay, cancellationToken: token);
                View.Sentence.text = string.Concat(text.Substring(0, index), ZERO_ALPHA, text.Substring(index));
                index++;
            }
        }
        
        private void StopDialog()
        {
            m_isFinish = true;
            m_currentDialog = null;
            m_cancellationTokenSource.TryCancel();
        }
    }
}