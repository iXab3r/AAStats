using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AAShared;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using XMLib;
using XMLib.Extensions;

namespace AAStats
{
    public partial class UCBuffsList : MoveableUserControl
    {
        private Character m_char;

        private string m_filter;

        public UCBuffsList()
        {
            InitializeComponent();
        }

        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                gBuffs.DisplayLayout.Appearance.BackColor = value;
            }
        }

        public bool ShowId {
            get
            {
                return lblCharName.Visible;
            }
            set
            {
                lblCharName.Visible = value;
                gBuffs.Rows.Refresh(RefreshRow.FireInitializeRow);
            }
        }

        public void Initialize(Character _char)
        {
	        if (_char == null)
	        {
		        throw new ArgumentNullException(nameof(_char));
	        }
	        if (_char != m_char)
            {
                this.InvokeSafe(() => InitializeInternal(_char));
            }
            this.InvokeSafe(() => RefreshState());
        }

        private void InitializeInternal(Character _char)
        {
            m_char = _char;
            lblCharName.Text = _char != null ? String.Format("0x{0:X6}", m_char.Id) : "null";
            RefreshState();
        }

        public void RefreshState()
        {
            if (m_char == null)
            {
                gBuffs.SetDataBinding(null, null);
                return;
            }
            else
            {
                gBuffs.SetDataBinding(m_char.Buffs, null);
            }
            gBuffs.Selected.Rows.Clear();
            gBuffs.ActiveRow = null;
        }

        private void gBuffs_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            var grid = e.Layout.Grid;
            var layout = e.Layout;
            if (layout.Bands.Count > 0)
            {
                var band = layout.Bands[0];

                layout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
                layout.Override.BorderStyleRow = UIElementBorderStyle.None;
                layout.Override.BorderStyleCell = UIElementBorderStyle.None;
                layout.Override.RowAppearance = layout.Appearance;
                layout.Override.ActiveRowAppearance = layout.Override.SelectedRowAppearance = layout.Override.RowAppearance;
                band.HeaderVisible = false;
                band.ColHeadersVisible = false;
                layout.Override.DefaultRowHeight = Properties.Settings.Default.IconsSize.Height;
                layout.Override.RowSpacingAfter = 5;
                foreach (var column in band.Columns)
                {
                    column.Hidden = true;
                }

                var columnIndex = 0;
                {
                    var column = band.Columns["Info"];
                    column.MaxWidth = column.Width = Properties.Settings.Default.IconsSize.Width;
                    column.Hidden = false;
                    column.Header.VisiblePosition = columnIndex++;
                }

                {
                    var column = band.Columns["Id"];
                    column.Hidden = !lblCharName.Visible;
                    column.MaxWidth = column.Width = 70;
                    column.CellAppearance.TextVAlign = VAlign.Middle;
                    column.CellAppearance.TextHAlign = HAlign.Center;
                    column.Format = "X4";
                    column.Header.VisiblePosition = columnIndex++;
                }

                ApplyFilter(m_filter);
            }
        }

        public string Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                if (System.String.Compare(m_filter, value, System.StringComparison.OrdinalIgnoreCase) != 0)
                {
                    m_filter = value;
                    ApplyFilter(m_filter);
                }
            }
        }

        private void ApplyFilter(string _filter)
        {
            var grid = gBuffs;
            var layout = grid.DisplayLayout;
            if (layout.Bands.Count > 0)
            {
                var band = layout.Bands[0];


                if (_filter != null)
                {
                    var splittedFilter = _filter.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                    if (splittedFilter.Any(x => x == "*"))
                    {
                        // не добавляем фильтры
                    }
                    else
                    {

                        foreach (var column in band.Columns.All.OfType<UltraGridColumn>())
                        {
                            var columnFilter = band.ColumnFilters[column];
                            columnFilter.ClearFilterConditions();
                            columnFilter.LogicalOperator = FilterLogicalOperator.Or;
                        }

                        foreach (var filter in splittedFilter.Where(x => !String.IsNullOrWhiteSpace(x)))
                        {
                            try
                            {
                                if (filter.StartsWith("0x"))
                                {
                                    var skillIdHEX = filter.Substring(2);
                                    var skillId = Convert.ToUInt32(skillIdHEX, 16);
                                    // фильтр по SkillId
                                    foreach (var column in band.Columns.All.OfType<UltraGridColumn>().Where(x => x.DataType == typeof(uint) || x.DataType == typeof(int)))
                                    {

                                        var columnFilter = band.ColumnFilters[column];
                                        columnFilter.FilterConditions.Add(FilterComparisionOperator.Equals, skillId);

                                    }
                                }
                                else
                                {
                                    foreach (var column in band.Columns.All.OfType<UltraGridColumn>().Where(x => x.DataType == typeof(string)))
                                    {
                                        var columnFilter = band.ColumnFilters[column];
                                        columnFilter.FilterConditions.Add(FilterComparisionOperator.Like, String.Format("*{0}*", filter));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(String.Format("Could not apply filter '{0}'", filter), ex);
                            }
                        }
                    }
                }
            }
        }

        private void gBuffs_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e == null || e.Row == null)
            {
                return;
            }
            var row = e.Row;
            var buff = row.ListObject as CharacterBuff;
            if (buff != null && row.Cells.Exists("Info"))
            {
                {
                    var cell = row.Cells["Info"];
                    cell.Editor = new ControlContainerEditor()
                    {
                        RenderingControl = new UCCharacterBuff(buff), 
                        RenderingControlPropertyName = "Value",
                    };
                }

                {
                    var cell = row.Cells["Id"];
                    cell.Hidden = !lblCharName.Visible;
                    cell.Editor = new ControlContainerEditor()
                    {
                        RenderingControl = new UCOutlinedLabel(){ForeColor = Color.White, OutlineForeColor = Color.Black}, 
                        RenderingControlPropertyName = "Text",
                    };
                }
            }

        }

        private void tmrUpdater_Tick(object sender, EventArgs e)
        {
            gBuffs.Rows.Refresh(RefreshRow.RefreshDisplay);
        }
    }

}
