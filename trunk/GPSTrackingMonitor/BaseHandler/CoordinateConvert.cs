using System;
using System.Collections.Generic;
using System.Text;

namespace GPSTrackingMonitor.BaseHandler
{
    class CoordinateConvert
    {

        /// <summary>
        /// ����������ת���ɸ�˹-������ͶӰ���꣨����ĵ��������õ���xian_1980��
        /// </summary>
        /// <param name="X">ͶӰ����x</param>
        /// <param name="Y">ͶӰ����y</param>
        /// <param name="B">��������γ��</param>
        /// <param name="L">�������꾭��</param>
        /// <param name="nCenterLongi">���뾭��</param>
        public static void ConvertLatLongCoordToGuss(ref double X, ref double Y, double B, double L, int nCenterLongi)
        {
            
            //��˹ͶӰ�ִ�
            int nzonenum;
            if (nCenterLongi == 0)
            {
                nzonenum = (int)L / 6 + 1;
                nCenterLongi = nzonenum * 6 - 3;
            }
            else
                nzonenum = (int)nCenterLongi / 6 + 1;

            //�Ի���Ϊ��λ�ľ�γ����ֵ
            double rB = B / 180 * 3.1415926;
            double rL = (L - nCenterLongi) / 180 * 3.1415926;		//ͬʱ���������뾭��
            //1980����ϵ����
            const double a = 6378245.00;		//����
            const double b = 6356863.50;		//����
            //			const double alpha = 0.0033523299;	//����
            double sqre1 = (a * a - b * b) / (a * a);		//��һƫ����ƽ��
            //			double sqre2 = (a*a-b*b)/(b*b);		//�ڶ�ƫ����ƽ��
            //B:γ��
            //L:����
            //����Ȧ���ʰ뾶
            double sinb = Math.Sin(rB);
            double cosb = Math.Cos(rB);
            double M = a * (1 - sqre1) / (1 - sqre1 * sinb * sinb) / Math.Sqrt(1 - sqre1 * sinb * sinb);
            //î��Ȧ���ʰ뾶
            double N = a / Math.Sqrt(1 - sqre1 * sinb * sinb);
            double sqrita = N / M - 1;

            //��γ�ȵ㵽����������߻���
            double s = a * (1 - sqre1) * (1.00505117739 * rB - 0.00506237764 / 2 * Math.Sin(2 * rB) +
                0.0000106245 / 4 * Math.Sin(4 * rB) - 0.00000002081 / 6 * Math.Sin(6 * rB));

            double tanb = Math.Tan(rB);
            X = s + rL * rL * N / 2 * sinb * cosb + rL * rL * rL * rL * N / 24 * sinb * cosb * cosb * cosb * (5 - tanb * tanb + 9 * sqrita * sqrita + 4 * sqrita);
            Y = rL * N * cosb + rL * rL * rL * N / 6 * cosb * cosb * cosb * (1 - tanb * tanb + sqrita) +
                rL * rL * rL * rL * rL * N / 120 * cosb * cosb * cosb * cosb * cosb * (5 - 18 * tanb * tanb + tanb * tanb * tanb * tanb);
            Y = Y + 500000 + nzonenum * 1.0e+6;
        }
        
        /// <summary>
        /// ����˹-������ͶӰ����ת���ɵ������꣨����ĵ��������õ���xian_1980��
        /// </summary>
        /// <param name="X">ͶӰ����x</param>
        /// <param name="Y">ͶӰ����y</param>
        /// <param name="L0">��Ⱦ���</param>
        /// <param name="B">��������γ��</param>
        /// <param name="L">�������꾭��</param>
        public static void ConvertGussCoordToLatLong(double X, double Y, double L0, ref double B, ref double L)
        {
            double p = 57.29577951472;
            const double a = 6.378245000e+06;
            const double e2 = 0.00669342162297;
            const double e12 = 0.00673852541468;
            const double c0 = 0.157046064172e-06;
            const double c1 = 0.005051773759;
            const double c2 = 0.000029837302;
            const double c3 = 0.000000238189;

            double bf0 = c0 * X;
            double bf0c = Math.Cos(bf0);
            double bf0s = Math.Sin(bf0);
            double bf = bf0 + bf0c * (c1 * bf0s - c2 * Math.Pow(bf0s, 3) + c3 * Math.Pow(bf0s, 5));
            double bt = Math.Tan(bf);
            double bc = Math.Cos(bf);
            double bs = Math.Sin(bf);
            double bi = e12 * Math.Pow(bc, 2);
            double v2 = 1.0e+0 + bi;
            double bn = a / Math.Sqrt(1.0 - e2 * Math.Pow(bs, 2));
            double yn = Y / bn;

            /*calculate lantitude b */

            double b1 = -v2 * bt * Math.Pow(yn, 2) / 2.0;
            double b2 = -(5.0 + 3.0 * Math.Pow(bt, 2) + bi - 9.0 * bi * Math.Pow(bt, 2)) * b1 * Math.Pow(yn, 2) / 12.0;
            double b3 = (61.0 + 90.0 * Math.Pow(bt, 2) + 45.0 * Math.Pow(bt, 4)) * b1 * Math.Pow(yn, 4) / 360.0;
            B = bf + b1 + b2 + b3;
            B = B * p;

            /* calculate the longitude l */

            double l1 = yn / bc;
            double l2 = -(1.0 + 2.0 * Math.Pow(bt, 2) + bi) * l1 * Math.Pow(yn, 2) / 6.0;
            double l3 = (5.0 + 28.0 * Math.Pow(bt, 2) + 24.0 * Math.Pow(bt, 4) + 6.0 * bi
                + 8.0 * bi * Math.Pow(bt, 2)) * l1 * Math.Pow(yn, 4) / 120.0;
            L = l1 + l2 + l3;
            L = L * p;

            L = L + L0;
            if (L > 360.0)
                L = L - 360.0;
        }

        /// <summary>
        ///  ����˹-������ͶӰ����ת���ɵ������꣨����ĵ��������õ���xian_1980��
        /// </summary>
        /// <param name="dX">ͶӰ����x</param>
        /// <param name="dY">ͶӰ����y</param>
        /// <param name="dLongitude">�������꾭��</param>
        /// <param name="dLatitude">��������γ��</param>
        public static void ConvertGussCoordToLatLong(double dX, double dY, ref double dLongitude, ref double dLatitude)
        {
            // TODO: Add your dispatch handler code here
            double L0;
            int nZoonNum;

            nZoonNum = (int)(dY / (1.0E+6));
            L0 = nZoonNum * 6 - 3;

            dY = dY - nZoonNum * 1.0E+6;
            ConvertGussCoordToLatLong(dX, dY - 500000, L0, ref dLatitude, ref dLongitude);
            dLongitude = dLongitude + nZoonNum * 6 - 3;
        }

        /// <summary>
        /// ����������ת���ɸ�˹-������ͶӰ���꣨����ĵ��������õ���xian_1980��
        /// </summary>
        /// <param name="dLongitude">�������꾭��</param>
        /// <param name="dLatitude">��������γ��</param>
        /// <param name="dX">ͶӰ����x</param>
        /// <param name="dY">ͶӰ����y</param>
        /// <param name="nCenterL"></param>
        public static  void ConvertLatLongCoordToGuss(double dLongitude, double dLatitude, ref double dX, ref double dY, long nCenterL)
        {
            // TODO: Add your dispatch handler code here
            int CenterL = (int)nCenterL;

            ConvertLatLongCoordToGuss(ref dX, ref dY, dLatitude, dLongitude, CenterL);

            nCenterL = (long)CenterL;
        }
    }
}
