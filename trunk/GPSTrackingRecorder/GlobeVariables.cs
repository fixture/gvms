using System;
using System.Collections.Generic;
using System.Text;

namespace GPSTrackingRecorder
{
    class GlobeVariables
    {    
        /// <summary>
        /// ������ʷλ����Ϣ���ݿ��·��
        /// </summary>
        public static string HistoryPointsDBPath = System.Windows.Forms.Application.StartupPath + @"\data\historypoints.mdb";

        /// <summary>
        /// Ĭ�ϼ����˿�
        /// </summary>
        public static readonly int DefaultLiseningPort = 8205;

        /// <summary>
        /// Ĭ��gps���ط�������ַ
        /// </summary>
        public static readonly string DefaultServerIP = "10.10.10.213";

        /// <summary>
        /// ����Ĭ������
        /// </summary>
        public static readonly int DefaultQueueMax = 100;
    }
}
