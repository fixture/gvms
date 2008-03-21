using System;
using System.Collections.Generic;
using System.Text;

namespace GPSTrackingMonitor.RealtimeMonite
{
    class DataStructConverter
    {
        public static CommnicationMessage.GPSTrackingMessage DataRowToCommInfos(System.Data.DataRow carRowInfos)
        {
            CommnicationMessage.GPSTrackingMessage oCommInfos = new CommnicationMessage.GPSTrackingMessage();
            
            try
            {
                oCommInfos.GeoId = carRowInfos["GeoId"].ToString();
                oCommInfos.CarNumber = carRowInfos["CarNumber"].ToString();
                oCommInfos.Phone = carRowInfos["phone"].ToString();
                oCommInfos.X = Convert.ToDouble(carRowInfos["X"]);
                oCommInfos.Y = Convert.ToDouble(carRowInfos["Y"]);
                oCommInfos.Direction = Convert.ToDouble(carRowInfos["Direction"]);
                oCommInfos.TimeStamp = Convert.ToDateTime(carRowInfos["CurrentTime"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return oCommInfos;
        }

        public static void CommInfosToDataRow(CommnicationMessage.GPSTrackingMessage carCommInfos, ref System.Data.DataRow carRowInfos)
        {
            try
            {
                carRowInfos["GeoId"] = carCommInfos.GeoId;
                carRowInfos["CarNumber"] = carCommInfos.CarNumber;
                carRowInfos["Phone"] = carCommInfos.Phone;
                carRowInfos["X"] = carCommInfos.X;
                carRowInfos["Y"] = carCommInfos.Y;
                carRowInfos["Direction"] = carCommInfos.Direction;
                carRowInfos["CurrentTime"] = carCommInfos.TimeStamp.ToString() + "." + carCommInfos.TimeStamp.Millisecond.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
