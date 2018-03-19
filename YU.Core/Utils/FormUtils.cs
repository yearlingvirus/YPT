using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YU.Core.Attributes;
using YU.Core.Log;

namespace YU.Core.Utils
{
    public static class FormUtils
    {
        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        public static void ShowErrMessage(string msg, string title = "系统错误")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        public static void ShowInfoMessage(string msg, string title = "提示")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        /// <param name="gridView"></param>

        public static void InitDataGridView(DataGridView gridView)
        {
            gridView.AllowUserToAddRows = false;
            gridView.AutoGenerateColumns = false;

            gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridView.DefaultCellStyle.Font = new Font("微软雅黑", 9, FontStyle.Regular);

            gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            gridView.RowTemplate.Height = 36;

            gridView.ColumnHeadersDefaultCellStyle.Font = gridView.DefaultCellStyle.Font;
            gridView.RowHeadersDefaultCellStyle.Font = gridView.DefaultCellStyle.Font;
            gridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            gridView.AllowUserToResizeColumns = true;
            gridView.AllowUserToDeleteRows = false;
            gridView.ReadOnly = true;
            gridView.AllowUserToOrderColumns = true;
        }

        /// <summary>
        /// 自动根据带有GridViewAttribute属性的实体创建显示的
        /// </summary>
        /// <param name="gridView">要设置的GridView</param>
        /// <param name="typeOfEntity">带有GridViewAttribute属性的实体类型</param>
        public static void CreateDataGridColumns(DataGridView gridView, Type typeOfEntity)
        {
            int i = 0;
            System.Reflection.PropertyInfo[] props = typeOfEntity.GetProperties();
            foreach (var p in props)
            {
                object[] atts = p.GetCustomAttributes(typeof(GridViewAttribute), false);
                if (atts.Length == 0)
                    continue;
                var att = atts[0] as GridViewAttribute;
                if (att == null)
                    continue;
                DataGridViewColumn colNew = (DataGridViewColumn)Activator.CreateInstance(att.ColType);
                colNew.SortMode = DataGridViewColumnSortMode.Programmatic;
                if (colNew is DataGridViewImageColumn)
                {
                    colNew.DefaultCellStyle.NullValue = null;
                }
                colNew.Name = p.Name;
                colNew.DataPropertyName = p.Name;
                colNew.HeaderText = att.Caption;
                colNew.Visible = att.ColumnVisible;
                colNew.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (att.ColumnWidth != -1)
                {
                    colNew.Width = att.ColumnWidth;
                    //colNew.MinimumWidth = att.ColumnWidth / 2;
                    colNew.FillWeight = att.ColumnWidth;
                }
                else
                if (att.ColumnVisible)
                {
                    if (att.VisibleIndex >= 0)
                        colNew.DisplayIndex = att.VisibleIndex;
                    else
                        colNew.DisplayIndex = i++;
                }
                gridView.Columns.Add(colNew);
            }


        }

        /// <summary>
        /// 绑定控件值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="controlKvr"></param>
        public static void BindControlValue(object instance, IEnumerable<KeyValuePair<Control, string>> controlKvr)
        {
            Type t = instance.GetType();
            if (controlKvr != null && controlKvr.Count() > 0)
            {
                foreach (var kvr in controlKvr)
                {
                    PropertyInfo info = t.GetProperty(kvr.Value);
                    var o = info.GetValue(instance, null);
                    if (kvr.Key is CheckBox)
                        (kvr.Key as CheckBox).Checked = o.TryPareValue<bool>();
                    else if (kvr.Key is DateTimePicker)
                        (kvr.Key as DateTimePicker).Value = o.TryPareValue<DateTime>();
                    else if (kvr.Key is NumericUpDown)
                        (kvr.Key as NumericUpDown).Value = o.TryPareValue<decimal>();
                    else if (kvr.Key is TextBox)
                        (kvr.Key as TextBox).Text = o.TryPareValue<string>();
                }
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="controlKvr"></param>
        public static void BindControlDataSource(object instance, IEnumerable<KeyValuePair<Control, string>> controlKvr)
        {
            Type t = instance.GetType();
            if (controlKvr != null && controlKvr.Count() > 0)
            {
                foreach (var kvr in controlKvr)
                {
                    if (kvr.Key is CheckBox)
                        (kvr.Key as CheckBox).DataBindings.Add("Checked", instance, kvr.Value);
                    else if (kvr.Key is DateTimePicker)
                        (kvr.Key as DateTimePicker).DataBindings.Add("Value", instance, kvr.Value);
                    else if (kvr.Key is NumericUpDown)
                        (kvr.Key as NumericUpDown).DataBindings.Add("Value", instance, kvr.Value);
                    else if (kvr.Key is TextBox)
                        (kvr.Key as TextBox).DataBindings.Add("Text", instance, kvr.Value);
                }
            }
        }

        public static void BindControlDataChanged(IEnumerable<Control> controls, EventHandler dataChanged)
        {
            foreach (var control in controls)
            {
                if (control is CheckBox)
                    (control as CheckBox).CheckedChanged += dataChanged;
                else if (control is DateTimePicker)
                    (control as DateTimePicker).ValueChanged += dataChanged;
                else if (control is NumericUpDown)
                    (control as NumericUpDown).ValueChanged += dataChanged;
                else if (control is TextBox)
                    (control as TextBox).Text += dataChanged;
            }

        }
    }
}
