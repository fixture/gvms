using System;
using System.Collections.Generic;
using System.Text;

namespace GPSGatewaySimulator
{
    class GlobeVariables
    {
        /// <summary>
        /// �������λ����Ϣ���ݿ��·��
        /// </summary>
        public static string RandomPointsDBPath = System.Windows.Forms.Application.StartupPath + @"\data\randompoints.mdb";

        /// <summary>
        /// socket�ͻ��˽������ݵ�Ĭ�϶˿�
        /// </summary>
        public static readonly int DefaultReceivePort = 8205;

        /// <summary>
        /// socket����˷�����Ϣ��Ĭ�ϼ��ʱ��
        /// </summary>
        public static readonly int DefaultSendInterval = 1000;

        /// <summary>
        /// ����ģ��ĳ�����Ŀ
        /// </summary>
        public static readonly int DefaultCarNumber = 1000;

    }
}
