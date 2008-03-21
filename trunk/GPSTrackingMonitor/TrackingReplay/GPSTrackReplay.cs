using System;
using System.Collections.Generic;
using System.Text;

namespace GPSTrackingMonitor.TrackingReplay
{
    public  class GPSTrackReplay
    {
        #region fields

        private TrackPoints _inputData = null;
        private TrackSegments _outputData = null;
        private double _baseIntervalLength = 200;
        private double _baseIntervalTime = 0;
        private double _smoothFactor = 0;
        private double _speedScaleFactor = 0;

        #endregion

        #region public properties

        /// <summary>
        /// ������ݣ���������
        /// </summary>
        public TrackSegments OutputData
        {
            get { return this._outputData; }          
        }

        ///// <summary>
        ///// �����֮��Ļ�����࣬������Խ��ƽ����ԽС�������ʱ������趨��Ĭ��ֵΪ200m
        ///// </summary>
        //public double BaseIntervalLength
        //{
        //    set { this._baseIntervalLength = value; }
        //}

        ///// <summary>
        ///// �����֮���ʱ��������Խ��ƽ����ԽС��Ĭ��ֵΪ���й켣������ʱ������Сֵ����1�룩
        ///// </summary>
        //public double BaseIntervalTime
        //{
        //    set { this._baseIntervalTime = value; }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public double SmoothFactor
        //{
        //    set { this._smoothFactor = value;}
        //}

        /// <summary>
        /// 
        /// </summary>
        public double SpeedScaleFactor
        {
            set { this._speedScaleFactor = value; }
        }

        #endregion      

        #region public methods

        public GPSTrackReplay(TrackPoints inputPoints)
        {
            this._inputData = inputPoints;
        }

        public GPSTrackReplay()
        {
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="inputPoints">����켣�㼯��</param>
        /// <param name="baseIntervalTime">����ֵ�켣��֮���ʱ����</param>
        /// <returns>��ֵ��Ĺ켣�㼯��</returns>
        public TrackPoints GetResultByInterpolateTime(TrackPoints inputPoints, double baseIntervalTime)
        {
            TrackPoints oResultPoint = null;

            try
            {
                TrackInterpolate oTrackInter = new TrackInterpolate();
                oTrackInter.BaseIntervalTime = baseIntervalTime;

                oResultPoint = oTrackInter.InterpolateProcess(inputPoints,);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

            return oResultPoint;
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns>��ֵ��Ĺ켣�㼯��</returns>
        public TrackPoints GetResultByInterpolateTime()
        {
            if (this._inputData == null)
                throw new Exception("please check that you had initilized the property  'InputValue'.");

            TrackPoints oResultPoints = null;

            try
            {
                oResultPoints = this.GetResultByInterpolateTime(this._inputData, this._baseIntervalTime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

            return oResultPoints;
        }

        ///// <summary>
        ///// ��ȡ������
        ///// </summary>
        ///// <param name="inputPoints">����켣�㼯��</param>
        ///// <param name="baseIntervalTime">����ֵ�켣��֮���ʱ����</param>
        ///// <returns>��ֵ��Ĺ켣�㼯��</returns>
        //public TrackPoints GetResultByInterpolateLength(TrackPoints inputPoints, double baseIntervalLength,double smoothFactor,double speedScaleFactor)
        //{
        //    TrackPoints oResultPoint = null;

        //    try
        //    {
        //        TrackInterpolate oTrackInter = new TrackInterpolate();
        //        //oTrackInter.BaseIntevalLength = baseIntervalLength;
        //        oTrackInter.SmoothFactor = smoothFactor;
        //        //oTrackInter.SpeedScaleFactor = speedScaleFactor;

        //        oResultPoint = oTrackInter.InterpolateProcess(inputPoints);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.StackTrace);
        //    }

        //    return oResultPoint;
        //}

        ///// <summary>
        ///// ��ȡ������
        ///// </summary>
        ///// <returns>��ֵ��Ĺ켣�㼯��</returns>
        //public TrackPoints GetResultByInterpolateLength()
        //{
        //    if (this._inputData == null)
        //        throw new Exception("please check that you had initilized the property  'InputValue'.");

        //    TrackPoints oResultPoints = null;

        //    try
        //    {
        //        oResultPoints = this.GetResultByInterpolateLength(this._inputData, this._baseIntervalLength,this._smoothFactor,this._speedScaleFactor);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.StackTrace);
        //    }

        //    return oResultPoints;
        //}
   
        #endregion
    }
}
