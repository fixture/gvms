using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapObjects2;

namespace GPSGatewaySimulator
{
    public partial class frmSetLoadParas : Form
    {
        #region constructors

        public frmSetLoadParas()
        {
            InitializeComponent();

            this.rdbAllPoints.Checked = true;
        }

        #endregion 

        #region fields

        private MapLayer _layer;
        private DataTable _result = new DataTable();

        #endregion

        #region properties

        public DataTable RandomPointsTable
        {
            get { return this._result; }
        }

        #endregion

        #region methods

        private void btnBrowseLayerPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog oOpenFileDialog = new OpenFileDialog();

            oOpenFileDialog.Filter = "ShapeFile(*.shp) | *.shp";

            if (oOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtLayerPath.Text = oOpenFileDialog.FileName;

                BaseHandler.MapManager oMapManager = new GPSGatewaySimulator.BaseHandler.MapManager();
                this._layer = oMapManager.GetLayer(this.txtLayerPath.Text);

                if (this._layer == null)
                {
                    MessageBox.Show("����ָ���ĵ�ͼ�ļ�����ȷ.");
                    this.txtLayerPath.Clear();
                    return;
                }

                if (this._layer.shapeType != ShapeTypeConstants.moShapeTypeLine)
                {
                    MessageBox.Show("����ָ���ĵ�ͼ���������ͣ�������ѡ��.");
                    this.txtLayerPath.Clear();
                    return;
                }
            }
        }

        private void btnGetRandomPoints_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLayerPath.Text))
            {
                MessageBox.Show("��ָ����ͼ�ļ�·����");
                return;
            }

            if (rdbSetCountPoints.Checked && string.IsNullOrEmpty(txtPointCount.Text))
            {
                MessageBox.Show("��ָ����Ҫ�����Ĺ켣���Ŀ����");
                return;
            }

            int iInterval = 0;

            if (!int.TryParse(this.txtInterval.Text, out iInterval))
            {
                MessageBox.Show("��ȷ����������ȷ��ʱ����.");
                return;
            }

            if (this._layer != null)
            {
                int iPointCount = rdbAllPoints.Checked ? this._layer.Records.Count : Int32.Parse(this.txtPointCount.Text.Trim());

                GPSGatewaySimulator.RandomPoints.GeneryRandomPoints oRandomPoints = new GPSGatewaySimulator.RandomPoints.GeneryRandomPoints();
                this._result = oRandomPoints.GetPointsFromRoadLayer(iPointCount,iInterval, this._layer);

                this.DialogResult = DialogResult.OK;
            }
        }

        private void rdbAllPoints_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPointCount.Enabled = false;
        }

        private void rdbSetCountPoints_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPointCount.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}