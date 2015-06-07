using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using AAShared;
using AAShared.Database;

using AAStats.Properties;

namespace AAStats
{
    public partial class UCCharacterBuff : MoveableUserControl
    {
        private CharacterBuff m_charBuff;


        public event EventHandler ValueChanged = delegate { };

        public UCCharacterBuff():this(null)
        {
        }

        public UCCharacterBuff(CharacterBuff _charBuff)
        {
            InitializeComponent();
            m_charBuff = _charBuff;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public BuffInfo Value
        {
            get
            {
                return m_charBuff.Info;
            }
            set
            {
                Reinitialize();
                ValueChanged(this, EventArgs.Empty);
            }
        }

        private void Reinitialize()
        {
            if (m_charBuff != null)
            {
                bgImg.Appearance.ImageBackground =  m_charBuff.Info.BuffIcon;
                lblDuration.Text = Utils.TimeSpanToHumanReadableString(m_charBuff.Duration);
                bgImg.Appearance.BorderColor = m_charBuff.Info.Kind == BuffKind.Debuff ? Color.Red : Color.Green;
            } else
            {
                bgImg.Appearance.ImageBackground = null;
                lblDuration.Text = null;
                bgImg.Appearance.BorderColor = Color.Transparent;
            }
        }

    }
}
