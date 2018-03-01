using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        /// 获取Url中的Image
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public static Image ImageFromWebTest(string imgUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                using (WebResponse response = request.GetResponse())
                {
                    Image img = Image.FromStream(response.GetResponseStream());
                    return img;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("远程获取图片失败，失败Url：{0}", imgUrl), ex);
                return null;
            }
        }

        public static Image GetImage(string path)
        {
            FileStream fs = new FileStream(path, System.IO.FileMode.Open);
            Image result = Image.FromStream(fs);
            fs.Close();
            return result;
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
    }
}
