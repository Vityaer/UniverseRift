using System;
using UIController.MessagePanels;

namespace MainScripts
{
    public partial class MessageController
    {
        [Serializable]
        public class PanelRecord
        {
            private RewardPanel panel;
            private Action delOnpen, delOnClose;
            public PanelRecord(RewardPanel panel, Action delOnpen, Action delOnClose = null)
            {
                this.panel = panel;
                this.delOnpen = delOnpen;
                this.delOnClose = delOnClose;
            }
            public void Open() { delOnpen(); }
            public void OnClose()
            {
                if (delOnClose != null)
                    delOnClose();
            }
        }
    }
}