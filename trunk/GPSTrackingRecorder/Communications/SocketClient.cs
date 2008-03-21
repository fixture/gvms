using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GPSTrackingRecorder.Communications 
{
    class SocketClient:IDisposable
    {
        #region fields

        private Queue<CommnicationMessage.GPSTrackingMessage> _messageCollection;
        private int _queueMaxCount = 50;


        private Thread _thread;
        private Socket _socket;
        private int _listeningPort = 8205;
        private string _serverIP = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
        private bool _connectionClosed = false;

        #endregion

        #region properties

        /// <summary>
        /// ��Ϣ���е��������������дﵽ���ֵʱ�������е����ݽ����浽���ݿ���ȥ��
        /// </summary>
        public int QueueMaxCount
        {
            get { return this._queueMaxCount; }
            set { this._queueMaxCount = value; }
        }


        /// <summary>
        /// ���ؼ����˿�
        /// </summary>
        public int ListenigPort
        {
            get { return this._listeningPort; }
            set { this._listeningPort = value; }
        }

        /// <summary>
        /// ������IP
        /// </summary>
        public string ServerIP
        {
            get { return this._serverIP; }
            set { this._serverIP = value; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// ��ʼ������Ϣ
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="listeningPort"></param>
        public void StartReceiveMessage()
        {
            try
            {
                this.InitSocket(this._serverIP, this._listeningPort);

                if (this._thread == null || this._thread.ThreadState == ThreadState.Stopped)
                    this._thread = new Thread(new ThreadStart(ReceiveMessages));

                if (this._thread != null && this._thread.ThreadState == ThreadState.Unstarted)
                {
                    this._connectionClosed = false;
                    this._messageCollection = new Queue<CommnicationMessage.GPSTrackingMessage>();

                    this._thread.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ֹͣ������Ϣ
        /// </summary>
        public void StopReceiveMessage()
        {
            try
            {
                this._connectionClosed = true;

                if (this._thread != null && this._thread.IsAlive)
                {
                    this._thread.Abort();
                    this._thread.Join();
                }

                if (this._socket != null && this._socket.IsBound)
                {
                    this._socket.Shutdown(SocketShutdown.Receive);
                    this._socket.Close();

                    if(this._messageCollection != null && this._messageCollection.Count > 0)
                        HistoryTrackings.SavePointsToDB.SaveToDB(this._messageCollection);   
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
        /// ��ʼ��socket
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="listeningPort"></param>
        private void InitSocket(string serverIP,int listeningPort)
        {
            IPEndPoint oIPEndPoint = new IPEndPoint(this.ParseIP(serverIP), listeningPort);
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this._socket.Bind(oIPEndPoint);
        }

        /// <summary>
        /// �ڼ����˿���ѭ����ȡ��Ϣ
        /// </summary>
        private void ReceiveMessages()
        {  
            //do
            //{
            //    byte[] oBuffer = new byte[10240];
            //    int iMessageLength = this._socket.Receive(oBuffer);
            //    CommnicationMessage.GPSTrackingMessage oMessage = (CommnicationMessage.GPSTrackingMessage)CommnicationMessage.ObjectSerialize.DeserializeBytesToObject(oBuffer, iMessageLength, CommnicationMessage.ObjectSerialize.SeralizeFormatType.BinaryFormat);

            //    this.OnProcessMessage(new MessageArguments(oMessage));
            //    //this._messageCollection.Enqueue(oMessage);


            //    if (this._messageCollection.Count == this._queueMaxCount)
            //        HistoryTrakings.SavePointsToDB.SaveToDB(this._messageCollection);                

            //    //if (this._messageCollection.Count == this._queueMaxCount)
            //    //    HistoryTrakings.SavePointsToDB.SaveToDB(this._messageCollection);                

            //}
            //while (true);

            do
            {
                //����ȡ�������ʱ����ֹѭ��������Ŀǰ������ͨ��Զ�̷���������Ϣָ������ֹѭ����
                if (this._connectionClosed == true) break;

                byte[] oBuffer = new byte[10240];
                int iMessageLength = this._socket.Receive(oBuffer);
                CommnicationMessage.GPSTrackingMessage oMessage = (CommnicationMessage.GPSTrackingMessage)CommnicationMessage.ObjectSerialize.DeserializeBytesToObject(oBuffer, iMessageLength, CommnicationMessage.ObjectSerialize.SeralizeFormatType.BinaryFormat);

                this.OnProcessMessage(new MessageArguments(oMessage));
                this._messageCollection.Enqueue(oMessage);

                if (this._messageCollection.Count == this._queueMaxCount)
                    HistoryTrackings.SavePointsToDB.SaveToDB(this._messageCollection);   
            }
            while (true);
        }

        /// <summary>
        /// ����IP��ַ
        /// </summary>
        /// <param name="ipString">ip�ַ���</param>
        /// <returns></returns>
        private IPAddress ParseIP(string ipString)
        {
            try
            {
                return Dns.GetHostAddresses(ipString)[0];
            }
            catch
            {
                throw new Exception("��ȷ���������IP��ַ��ȷ.");
            }
        }

        #endregion

        #region events


        /// <summary>
        /// ��Ϣ����ί���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ProcessMessageHandler(object sender, MessageArguments e);

        /// <summary>
        /// ��Ϣ����ί���¼�ʵ��
        /// </summary>
        public event ProcessMessageHandler ProcessMessageEvent;

        /// <summary>
        /// �׳���Ϣ�����¼�
        /// </summary>
        /// <param name="e"></param>
        protected void OnProcessMessage(MessageArguments e)
        {
            if (this.ProcessMessageEvent != null && e.Message != null)
                ProcessMessageEvent(null, e);
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            this.StopReceiveMessage();

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispoisng)
        {
            if (dispoisng)
            {
                if (this._thread != null)
                {
                    this._thread = null;
                }

                if (this._socket != null)
                    this._socket = null;
            }
        }

        #endregion

    }
}
