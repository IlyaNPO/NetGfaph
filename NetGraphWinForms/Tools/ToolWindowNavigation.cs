using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGraphWinForms
{
    public partial class ToolWindowNavigation : Form
    {
        VoidDelegate invalidateAction = null;
        GraphDrawer gDrawer = null;

        public ToolWindowNavigation(VoidDelegate _action, GraphDrawer _gDrawer)
        {
            InitializeComponent();
            this.invalidateAction = _action;
            this.gDrawer = _gDrawer;
        }

        #region Scrolling
        private void btnMooveDown_Click(object sender, EventArgs e)
        {
            gDrawer.Y0 += 10;
            invalidateAction();
        }

        private void btnDefaultLocation_Click(object sender, EventArgs e)
        {
            gDrawer.Y0 = 10;
            gDrawer.X0 = 10;
            invalidateAction();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            gDrawer.Y0 -= 10;
            invalidateAction();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            gDrawer.X0 -= 10;
            invalidateAction();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            gDrawer.X0 += 10;
            invalidateAction();
        }
        #endregion Scrolling


        #region Zoom
        private void btnZoomPlus_Click(object sender, EventArgs e)
        {
            gDrawer.ScaleCoeff += 0.3f;
            invalidateAction();
        }

        private void btnZoomMinus_Click(object sender, EventArgs e)
        {
            gDrawer.ScaleCoeff -= 0.3f;
            invalidateAction();
        }

        private void btnZoomDefault_Click(object sender, EventArgs e)
        {
            gDrawer.ScaleCoeff = 1f;
            invalidateAction();
        }
        #endregion Zoom
    }
}
