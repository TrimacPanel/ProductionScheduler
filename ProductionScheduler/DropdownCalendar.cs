using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;
using System.Drawing;
using System.Drawing.Text;

namespace ProductionScheduler
{
    public class DropdownCalendar : IiGDropDownControl
    {
        MonthCalendar m_calMain;
        iGrid m_grdMain;


        private MonthCalendar Calendar
        {
            get
            {
                if (m_calMain == null)
                {
                    m_calMain = new MonthCalendar();
                    m_calMain.DateSelected += m_calMain_DateSelected;
                    m_calMain.KeyDown += m_calMain_KeyDown;
                    m_calMain.MaxSelectionCount = 1;
                    m_calMain.CreateControl();
                }

                return m_calMain;

            }
        }

        private void m_calMain_DateSelected(object sender, DateRangeEventArgs e)
        {
            m_grdMain.CommitEditCurCell();
        }

        private void m_calMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    m_grdMain.CommitEditCurCell();
                    break;
                case Keys.Escape:
                    m_grdMain.CancelEditCurCell();
                    break;
            }
        }

        object IiGDropDownControl.GetItemByValue(object value, bool firstByOrder)
        {
            return value;
        }

        object IiGDropDownControl.GetItemByText(string text)
        {
            try
            {
                return DateTime.Parse(text);
            }
            catch (Exception e)
            {
                return null;
            }

        }


        int IiGDropDownControl.GetItemImageIndex(object item)
        {
            return -1;
        }

        object IiGDropDownControl.GetItemValue(object item)
        {
            return item;
        }

        Control IiGDropDownControl.GetDropDownControl(iGrid grid, System.Drawing.Font font, Type interfaceType)
        {
            m_grdMain = grid;
            Calendar.Font = font;
            Calendar.RightToLeft = grid.RightToLeft;

            return Calendar;
        }

        void IiGDropDownControl.OnHide()
        {
        }

        void IiGDropDownControl.OnShow()
        {
        }

        void IiGDropDownControl.OnShowing()
        {
        }

        object IiGDropDownControl.SelectedItem
        {
            get
            {
                return m_calMain.SelectionStart.ToString("d");
            }

            set
            {
                if (value == null)
                {
                    value = DateTime.Today.Date;
                }
                //if (value.GetType() != typeof(DateTime))
                //{
                //    throw new ArgumentException();
                //}

                DateTime d = DateTime.Parse(value.ToString());
                m_calMain.SetDate(d);

            }
        }

        int IiGDropDownControl.Width
        {
            get
            {
                return m_calMain.SingleMonthSize.Width + 2;
            }
        }

        int IiGDropDownControl.Height
        {
            get
            {
                return m_calMain.SingleMonthSize.Height + 2;
            }
        }

        ImageList IiGDropDownControl.ImageList
        {
            get
            {
                return null;
            }
        }

        bool IiGDropDownControl.Sizeable
        {
            get
            {
                return false;
            }
        }

        bool IiGDropDownControl.CommitOnHide
        {
            get
            {
                return false;
            }
        }

        bool IiGDropDownControl.AutoSubstitution
        {
            get
            {
                return false;
            }
        }

        bool IiGDropDownControl.CloseButton
        {
            get
            {
                return false;
            }
        }

        string IiGDropDownControl.Text
        {
            get
            {
                return null;
            }
        }

        bool IiGDropDownControl.HideColHdrDropDown
        {
            get
            {
                return true;
            }
        }

        void IiGDropDownControl.SetTextRenderingHint(TextRenderingHint textRenderingHint)
        {
        }

    }



}
