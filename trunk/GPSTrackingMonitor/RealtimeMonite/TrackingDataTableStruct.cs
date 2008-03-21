using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GPSTrackingMonitor.RealtimeMonite
{
    /// <summary>
    /// 
    /// </summary>
    class TrackingDataTableStruct :DataTable
    {
        #region constructor

        public TrackingDataTableStruct()
        {
            DataColumn colGeoId = new DataColumn("GeoId", typeof(string));
            colGeoId.Caption = "GeoId";

            DataColumn colCarNumber = new DataColumn("CarNumber", typeof(string));
            colCarNumber.Caption = "���ƺ�";

            DataColumn colPhone = new DataColumn("Phone", typeof(string));
            colPhone.Caption = "��ϵ�绰";

            DataColumn colX = new DataColumn("X", typeof(double));
            colX.Caption = "X ����";

            DataColumn colY = new DataColumn("Y", typeof(double));
            colY.Caption = "Y ����";

            DataColumn colDirection = new DataColumn("Direction", typeof(double));
            colDirection.Caption = "��ʻ����";

            DataColumn colCurrentTime = new DataColumn("CurrentTime", typeof(DateTime));
            colCurrentTime.Caption = "��ǰʱ��";

            colCarNumber.Unique = true;
            this.Columns.AddRange(new DataColumn[] { colGeoId, colCarNumber, colPhone, colX, colY, colDirection, colCurrentTime });
            this.PrimaryKey = new DataColumn[] { colCarNumber };
        }

        #endregion

        #region public methods

        public static void UpdateRow(DataRow newRow, ref TrackingDataTableStruct carTable)
        {  
            int iRowIndex = Find(newRow, carTable);

            if (iRowIndex == -1)
            {
                carTable.Rows.Add(newRow);
            }
            else
            {
                DataRow dr = carTable.Rows[iRowIndex];
                foreach (DataColumn col in carTable.Columns)
                {
                    if (col.Unique != true)
                        dr[col] = newRow[col];
                }
            }
        }

        private  static int Find(DataRow dr, DataTable carTable)
        {
            DataRow[] drRows = carTable.Select(string.Format("carNumber='{0}'", dr["carNumber"].ToString()));

            if(drRows.Length == 0)
                return -1;
            else
                return carTable.Rows.IndexOf(drRows[0]);
        }

        #endregion
    }
}
