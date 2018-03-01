using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YU.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GridViewAttribute : Attribute
    {

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="caption">显示的列名</param>
        /// <param name="columnVisible">是否可见</param>
        public GridViewAttribute(string caption, bool columnVisible)
            : this(caption, columnVisible, 100)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="caption">显示的列名</param>
        /// <param name="columnVisible">是否可见</param>
        /// <param name="columnWidth">列宽</param>
        public GridViewAttribute(string caption, bool columnVisible, int columnWidth)
            : this(caption, columnVisible, columnWidth, -1, typeof(DataGridViewTextBoxColumn))
        {

        }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="caption">显示的列名</param>
        /// <param name="columnVisible">是否可见</param>
        /// <param name="columnWidth">列宽</param>
        /// <param name="visibleIndex">显示的列位置</param>
        public GridViewAttribute(string caption, bool columnVisible, int columnWidth, int visibleIndex, Type colType)
        {
            this._caption = caption;
            this._columnVisible = columnVisible;
            this._columnWidth = columnWidth;
            this._visibleIndex = visibleIndex;
            this._colType = colType;
        }

        protected string _caption;
        /// <summary>
        /// 显示的列名
        /// </summary>
        public string Caption
        {
            get
            {
                return this._caption;
            }
        }

        protected bool _columnVisible;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool ColumnVisible
        {
            get
            {
                return this._columnVisible;
            }
        }

        protected int _columnWidth;
        /// <summary>
        /// 列宽
        /// </summary>
        public int ColumnWidth
        {
            get
            {
                return this._columnWidth;
            }
        }

        protected int _visibleIndex;

        /// <summary>
        /// 显示的位置
        /// </summary>
        public int VisibleIndex
        {
            get
            {
                return this._visibleIndex;
            }
        }


        protected Type _colType;

        /// <summary>
        /// 显示的位置
        /// </summary>
        public Type ColType
        {
            get
            {
                return this._colType;
            }
        }
    }
}
