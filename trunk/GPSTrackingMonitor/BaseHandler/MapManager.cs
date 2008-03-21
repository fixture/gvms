using System;
using System.Collections.Generic;
using System.Text;
using MapObjects2;
using System.Collections;

namespace GPSTrackingMonitor.BaseHandler
{
    public class MapManager
    {
        #region public methods

        #region about featureLayer and imageLayer

        /// <summary>
        /// �����ļ����õ�ʸ��ͼ��
        /// </summary>
        /// <param name="layerPath">ͼ���ļ�·��</param>
        /// <returns></returns>
        public MapLayer GetSingleVectorLayer(string layerPath)
        {
            if (!System.IO.File.Exists(layerPath))
                return null;

            GeoDataset oGeoDataset = null;
            DataConnection oConn = new DataConnectionClass();
            MapLayer oLayer = null;

            oConn.Database = System.IO.Path.GetDirectoryName(layerPath);

            if (oConn.Connect())
            {
                oGeoDataset = oConn.FindGeoDataset(System.IO.Path.GetFileNameWithoutExtension(layerPath));

                if (!oGeoDataset.Equals(null))
                {
                    oLayer = new MapLayerClass();
                    oLayer.GeoDataset = oGeoDataset;
                    oConn.Disconnect();
                }
            }

            return oLayer;
        }

        /// <summary>
        /// �����ļ����õ�դ��ͼ��
        /// </summary>
        /// <param name="layerPath">ͼ���ļ�·��</param>
        /// <returns></returns>
        public ImageLayer GetSingleImageLayer(string layerPath)
        {
            if (!System.IO.File.Exists(layerPath))
                return null;

            ImageLayer oLayer = new ImageLayerClass();
            oLayer.File = layerPath;

            return oLayer;
        }

        
        /// <summary>
        /// �����ļ���չ������ȡͼ������
        /// </summary>
        /// <param name="fileExtent">�ļ���չ��</param>
        /// <returns></returns>
        public LayerTypeConstants GetLayerTypeByFileExtent(string fileExtent)
        {
            fileExtent = fileExtent.Trim().ToLower().Replace(".",string.Empty);
            ArrayList arSupportRasterFormat = new ArrayList(SupportRasterFormat);

            if (fileExtent.Equals("shp"))
            {
                return LayerTypeConstants.moMapLayer;
            }
            else if (arSupportRasterFormat.Contains(fileExtent))
            {   
                return LayerTypeConstants.moImageLayer;
            }

            return (LayerTypeConstants) (- 1);
        }

        

        private void SortLayersByShapeType(ref MapClass map )
        {   
        }


        public static string[] SupportRasterFormat
        {
            get
            {
                return new string[]{
                    "bmp","gif","tif","tiff","tff","jpg","jpeg","img","ovr","rlc", "l","bil","bip","dib","rs",
                    "bsq","gis","lan","bil","svf","gp2","sid","ntf","rs","sun", "ras"
                };
            }
        }

        #endregion

       

      
        #endregion
    }
}
