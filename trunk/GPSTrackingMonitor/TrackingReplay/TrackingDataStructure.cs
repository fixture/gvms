

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Reflection;

namespace GPSTrackingMonitor.TrackingReplay
{
    #region trackpoint class

    /// <summary>
    /// �켣����Ϣ
    /// </summary>
    public class TrackPoint
    {
        private double _x;
        private double _y;
        private DateTime _timeStamp;
        private int _id;

        /// <summary>
        /// 
        /// </summary>
        public TrackPoint()
        {
        }

        /// <summary>
        /// �½�TrackPoint����
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="TimeStamp">ʱ���</param>
        public TrackPoint(int ID, double X, double Y, DateTime TimeStamp)
        {
            this._id = ID;
            this._x = X;
            this._y = Y;
            this._timeStamp = TimeStamp;
        }

        /// <summary>
        /// �����Xֵ
        /// </summary>
        public double X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        /// <summary>
        /// �����Yֵ
        /// </summary>
        public double Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        /// <summary>
        /// �켣���ʱ���
        /// </summary>
        public DateTime TimeStamp
        {
            get { return this._timeStamp; }
            set { this._timeStamp = value; }
        }

        /// <summary>
        /// �켣���ID��
        /// </summary>
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
    }

    #endregion

    #region list trackpoints class
    /// <summary>
    /// �켣�㼯��
    /// </summary>
    public class TrackPoints : List<TrackPoint>
    {
    }

    #endregion

    #region tracksegment class

    /// <summary>
    /// �켣����Ϣ
    /// </summary>
    public class TrackSegment : Object, ICloneable
    {
        private TrackPoint _startTrackPoint;
        private TrackPoint _endTrackPoint;
        private double _segLength;
        private double _speed;
        private double _courseTime;

        /// <summary>
        /// �ֶεĿ�ʼ�ڵ�
        /// </summary>
        public TrackPoint StartTrackPoint
        {
            get { return this._startTrackPoint; }
            set { this._startTrackPoint = value; }
        }


        /// <summary>
        /// �ֶεĽ����ڵ�
        /// </summary>
        public TrackPoint EndTrackPoint
        {
            get { return this._endTrackPoint; }
            set { this._endTrackPoint = value; }
        }

        /// <summary>
        /// �ֶγ��ȣ���λ���ף�
        /// </summary>
        public double SegmentLenth
        {
            get { return this._segLength; }
            set { this._segLength = value; }
        }

        /// <summary>
        /// �ٶȣ���λ����/���룩
        /// </summary>
        public double Speed
        {
            get { return this._speed; }
            set { this._speed = value; }
        }

        /// <summary>
        /// ����ʱ�䣨��λ:����)
        /// </summary>
        public double CourseTime
        {
            get { return this._courseTime; }
            set { this._courseTime = value; }
        }

        /// <summary>
        /// ��ȸ���
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            object obj = this.MemberwiseClone();
            TrackSegment oSeg = obj as TrackSegment;
            oSeg.CourseTime = this.CourseTime;
            oSeg.EndTrackPoint = this.EndTrackPoint;
            oSeg.SegmentLenth = this.SegmentLenth;
            oSeg.Speed = this.Speed;
            oSeg.StartTrackPoint = this.StartTrackPoint;

            return obj;
        }
    }

    #endregion

    #region list tracksegments class

    /// <summary>
    /// �켣�μ���
    /// </summary>
    public class TrackSegments : List<TrackSegment>
    {
    }

    #endregion
}
