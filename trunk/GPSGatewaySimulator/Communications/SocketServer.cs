using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using GPSGatewaySimulator.BaseHandler;

namespace GPSGatewaySimulator.Communications
{
    public class SocketServer
    {
        #region fields

        private Socket _socket;
        private Thread _thread;
        private int _remoteLiseningPort = 820527;
        private bool _connnectionClosed = false;
        private int _intervalue = 200;
        private Queue<CommnicationMessage.GPSTrackingMessage> _messageCollection;

        #endregion

        #region properties

        /// <summary>
        /// ��ȡ�����÷��ͼ��ʱ�䣨���룩��Ĭ��ֵΪ1000
        /// </summary>
        public int Intervalue
        {
            get{return this._intervalue;}
            set{this._intervalue = value;}
        }

        /// <summary>
        /// �ͷ��˼����˿�
        /// </summary>
        public int RemoteLiseningPort
        {
            get { return this._remoteLiseningPort; }
            set { this._remoteLiseningPort = value; }
        }

        #endregion

        #region constructors

        public SocketServer()
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="interval">��Ϣ���͵ļ��ʱ��</param>
        /// <param name="remoteListeningPort">�ͻ��˼����˿�</param>
        public SocketServer(int interval, int remoteListeningPort)
        {
            this._intervalue = interval;
            this._remoteLiseningPort = remoteListeningPort;
        }

        #endregion

        #region public methods

        /// <summary>
        /// ��������㲥��Ϣ
        /// </summary>
        /// <param name="remotePort">���ն˶˿�</param>
        /// <param name="interval">ÿ������ʱ�䷢��һ�Σ����룩</param>
        public void StartSendMessage()
        {
            try
            {
                this.InitSocket(this._remoteLiseningPort);

                if (this._thread == null || this._thread.ThreadState == ThreadState.Stopped)
                    this._thread = new Thread(new ThreadStart(SendMessage));

                if (this._thread != null && this._thread.ThreadState == ThreadState.Unstarted) 
                {
                    this._connnectionClosed = false;
                    this._thread.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ֹͣ��Ϣ����
        /// </summary>
        public void StopSendMessage()
        {
            try
            {
                this._connnectionClosed = true;

                if (this._thread != null && this._thread.IsAlive)
                {
                    this._thread.Abort();
                    this._thread.Join();
                }

                if (this._socket != null && this._socket.Connected)
                {
                    this._socket.Shutdown(SocketShutdown.Send);
                    this._socket.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// ��ʼ��Socket
        /// </summary>
        /// <param name="remotePort">���ն˶˿�</param>
        private void InitSocket(int remotePort)
        {
            try
            {
                IPEndPoint oIPEndPoint = new IPEndPoint(IPAddress.Broadcast, remotePort);
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                this._socket.Connect(oIPEndPoint);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void InitSocket(string remoteIP, int remotePort)
        {
            try
            {
                IPEndPoint oIPEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this._socket.Connect(oIPEndPoint);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ���ϵķ�����Ϣ
        /// </summary>
        private void SendMessage()
        {
            byte[] byteMessage;
            RandomPoints.GetRandomCarLocations oGetCarLocations = new GPSGatewaySimulator.RandomPoints.GetRandomCarLocations();

            do
            {
                if (this._connnectionClosed == true)
                {
                    byteMessage = CommnicationMessage.ObjectSerialize.SerializeObjectToBytes(new CommnicationMessage.GPSTrackingMessage(null, null, null, 0, 0, 0, DateTime.Now, true), CommnicationMessage.ObjectSerialize.SeralizeFormatType.BinaryFormat);
                    this._socket.Send(byteMessage);
                    break;
                }

                if (this._messageCollection == null)
                {
                    this._messageCollection = oGetCarLocations.GetCarLocationsQueue();
                    this._messageCollection.TrimExcess();
                }

                if (this._messageCollection.Count == 0)
                {
                    this._messageCollection = null;
                    continue;
                }

                byteMessage = CommnicationMessage.ObjectSerialize.SerializeObjectToBytes(this._messageCollection.Dequeue(), CommnicationMessage.ObjectSerialize.SeralizeFormatType.BinaryFormat);

                this._socket.Send(byteMessage);
                this._thread.Join(this._intervalue);
            }
            while (true);
        }

        #endregion

    }
}
