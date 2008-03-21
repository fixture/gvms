using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GPSGatewaySimulator.HistoryTrakings
{
    class TrackingDataTableStruct:DataTable
    {
        #region constructors

        public TrackingDataTableStruct()
        {
            DataColumn colGeoId = new DataColumn("GeoId", typeof(string));
            colGeoId.Caption = "GeoId";

            DataColumn colCarNumber = new DataColumn("CarNumber", typeof(string));
            colCarNumber.Caption = "���ƺ�";

            DataColumn colPhone = new DataColumn("Phone", typeof(string));
            colPhone.Caption = "��ϵ�绰";

            DataColumn colX = new DataColumn("X",typeof(double));
            colX.Caption = "X ����";

            DataColumn colY = new DataColumn("Y", typeof(double));
            colY.Caption = "Y ����";

            DataColumn colDirection = new DataColumn("Direction", typeof(double));
            colDirection.Caption = "��ʻ����";

            DataColumn colCurrentTime = new DataColumn("CurrentTime", typeof(DateTime));
            colCurrentTime.Caption = "��ǰʱ��";

            DataColumn colOrderIndex = new DataColumn("OrderIndex", typeof(int));
            colOrderIndex.Caption = "��Ϣ���";

            this.Columns.AddRange(new DataColumn[] { colGeoId,colCarNumber,colPhone,colX, colY,colDirection,colCurrentTime,colOrderIndex });
        }

        #endregion

    }
}
