/***********************************************
 * 
 * 
 * 
 * 
 * 
 * 
 * Use:
 *      1���Ե�ǰ����еĳ�����Ϊ����Դ�����ݹؼ��ֶλ����������Ϣ����ѯ������������Ϣ
 *      2���ܶԲ�ѯ�������ͼ�ζ�λ
 * 
 * *********************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPSTrackingMonitor
{
    public partial class frmCarSearcher : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public frmCarSearcher()
        {
            InitializeComponent();
        }

        private void frmCarSearcher_Load(object sender, EventArgs e)
        {
            this.cbbSearchType.Items.Clear();

            this.pnlSeachedByCustomFilter.Visible = false;
            this.pnlSearchedByCarNumber.Visible = false;
            this.pnlSearchedByLocation.Visible = false;

            this.cbbSearchType.Items.Add("���ݳ��ƺ����ѯ");
            this.cbbSearchType.Items.Add("����λ�÷�Χ��ѯ");
            this.cbbSearchType.Items.Add("�Զ����ѯ����");
            this.cbbSearchType.SelectedIndex = 0;

            frmMap.Instance.PointFetchedEvent += new frmMap.PointFetchedEventHandler(Instance_PointFetchedEvent);
        }

        void Instance_PointFetchedEvent(object sender, MapObjects2.Point e)
        {
            this.txtCoordX.Text = e.X.ToString();
            this.txtCoordY.Text = e.Y.ToString();
        }

        private void cbbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ResetPanelPosition(cbbSearchType.SelectedIndex);
        }

        private void ResetPanelPosition(int selectedSearchType)
        {
            Point oPanelLocation = new Point();
            Point oDataGridViewLocation = new Point();

            oPanelLocation.X = this.pnlSearchType.Location.X;
            oPanelLocation.Y = this.pnlSearchType.Location.Y + this.pnlSearchType.Height;

            switch (selectedSearchType)
            {
                case 0:
                    this.pnlSearchedByCarNumber.Visible = true;
                    this.pnlSeachedByCustomFilter.Visible = false;
                    this.pnlSearchedByLocation.Visible = false;
                    
                    this.pnlSearchedByCarNumber.Location = oPanelLocation;

                    oDataGridViewLocation.X = this.pnlSearchedByCarNumber.Location.X;
                    oDataGridViewLocation.Y = this.pnlSearchedByCarNumber.Location.Y + this.pnlSearchedByCarNumber.Height;                   
                    this.dgvSearchResult.Location = oDataGridViewLocation;
                    this.dgvSearchResult.Height = this.groupBox1.Height - this.pnlSearchType.Height - this.pnlSearchedByCarNumber.Height - 25;
                    break;
                case 1:
                    this.pnlSearchedByCarNumber.Visible = false;
                    this.pnlSeachedByCustomFilter.Visible = false;
                    this.pnlSearchedByLocation.Visible = true;
                    
                    this.pnlSearchedByLocation.Location = oPanelLocation;

                    oDataGridViewLocation.X = this.pnlSearchedByLocation.Location.X;
                    oDataGridViewLocation.Y = this.pnlSearchedByLocation.Location.Y + this.pnlSearchedByLocation.Height;
                    this.dgvSearchResult.Location = oDataGridViewLocation;
                    this.dgvSearchResult.Height = this.groupBox1.Height - this.pnlSearchType.Height - this.pnlSearchedByLocation.Height - 25;
                    break;
                case 2:
                    this.pnlSearchedByCarNumber.Visible = false;
                    this.pnlSeachedByCustomFilter.Visible = true;
                    this.pnlSearchedByLocation.Visible = false;

                    this.pnlSeachedByCustomFilter.Location = oPanelLocation;

                    oDataGridViewLocation.X = this.pnlSeachedByCustomFilter.Location.X;
                    oDataGridViewLocation.Y = this.pnlSeachedByCustomFilter.Location.Y + this.pnlSeachedByCustomFilter.Height;                    
                    this.dgvSearchResult.Location = oDataGridViewLocation;
                    this.dgvSearchResult.Height = this.groupBox1.Height - this.pnlSearchType.Height - this.pnlSeachedByCustomFilter.Height - 25;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgvSearchResult.DataSource = null;

            Utilities.CarSearch oSearchCar = new GPSTrackingMonitor.Utilities.CarSearch(GlobeVariables.RealtimeCarInfosTable.Copy());

            switch (this.cbbSearchType.SelectedIndex)
            {
                case  0:
                    this.dgvSearchResult.DataSource = oSearchCar.SearchByCarNumber(this.txtCarNumber.Text.Trim(), this.ckbAllMatched.Checked);
                    
                    break;
                case 1:
                    double dSearchRaduis = 0;
                    double dCoordX = 0;
                    double dCoordY = 0;

                    if (!double.TryParse(this.txtCoordX.Text.Trim(), out dCoordX)
                        || !double.TryParse(this.txtCoordY.Text.Trim(), out dCoordY)
                        || !double.TryParse(this.txtSearchRaduis.Text.Trim(), out dSearchRaduis))
                    {
                        MessageBox.Show("��ȷ����������ȷ�Ĳ���.", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MapObjects2.Point oCenterPoint = new MapObjects2.PointClass();
                    oCenterPoint.X = dCoordX;
                    oCenterPoint.Y = dCoordY;

                    this.dgvSearchResult.DataSource = oSearchCar.SearchByCarLocation(oCenterPoint, dSearchRaduis);

                    break;
                case 2:
                    try
                    {
                        this.dgvSearchResult.DataSource = oSearchCar.SearchByCustomFilter(this.txtCustomFilter.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("��ȷ������������ȷ�Ĺ���������\r\n\r\n������Ϣ����:\r\n\r\n" + ex.StackTrace);
                    }
                    break;
            }
        }

        private void btnAdvanceFilter_Click(object sender, EventArgs e)
        {
            frmAdvanceFilter oFrmAdvanceFilter = new frmAdvanceFilter(GlobeVariables.RealtimeCarInfosTable.Copy());

            if (oFrmAdvanceFilter.ShowDialog() == DialogResult.OK)
            {
                this.txtCustomFilter.Text = oFrmAdvanceFilter.FilterString;
            }            
        }    

        private void btnFetchPoint_Click(object sender, EventArgs e)
        {
            GlobeVariables.CurrentOperation = GPSTrackingMonitor.MapUtil.MapOperationType.FetchPoint;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvSearchResult.DataSource != null
                      && this.dgvSearchResult.RowCount > 0
                      && this.dgvSearchResult.SelectedRows.Count > 0)
            {
                string sCarNumber = this.dgvSearchResult.SelectedRows[0].Cells["CarNumber"].Value.ToString();

                MapObjects2.GeoEvent oGeoEvent = GlobeVariables.MainMapControl.TrackingLayer.FindEvent(sCarNumber);
                MapObjects2.Point oCarLocation = new MapObjects2.PointClass();

                oCarLocation.X = oGeoEvent.X;
                oCarLocation.Y = oGeoEvent.Y;

                MapUtil.MapOperation oMapOper = new GPSTrackingMonitor.MapUtil.MapOperation();
                oMapOper.ZoomAndCenterMap(0.02, oCarLocation, ref GlobeVariables.MainMapControl);
                GlobeVariables.MainMapControl.Refresh();

                GlobeVariables.MainMapControl.FlashShape(oCarLocation, 5);
            }

            
        }



        



        

        
    }
}