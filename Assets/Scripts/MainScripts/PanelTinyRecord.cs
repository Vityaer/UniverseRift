using System;
using UIController.MessagePanels;

namespace MainScripts
{
    public partial class MessageController
    {
        [Serializable]
        public class PanelTinyRecord
        {
            private TinyRewardPanel panel;
            private Action delOnpen;
            public PanelTinyRecord(TinyRewardPanel panel, Action delOnpen)
            {
                this.panel = panel;
                this.delOnpen = delOnpen;
            }
            public void Open() { delOnpen(); }
            public void Close()
            {
                panel.Close();
            }
        }
    }
}