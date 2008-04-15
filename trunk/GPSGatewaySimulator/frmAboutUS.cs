using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPSGatewaySimulator
{
    public partial class frmAboutUS : Form
    {
        public frmAboutUS()
        {
            InitializeComponent();
        }

        private void frmAboutUS_Load(object sender, EventArgs e)
        {
            this.txtIntroduction.Text += "  ������Ϊ��ԴGPS�������ϵͳ��GVMS���е�GPS����ģ��������ϵͳ����GISeeker�ŶӺ���������"
                                             + "������GPL 2.0Э�鷢���ò�Ʒ��\r\n\r\n"
                                             + " GISeeker�Ŷ���һȺ�����ڿ�ԴGIS����������ɣ����Ų�Ʒ�����ƣ��Ŷӳ�Ա����Ҳ�ڲ��ϵ�����֮�С�Ŀǰ�Ŷ��в���������Աֻ�����ˣ���������ļ������ּ������ǵĶ��顣\r\n\r\n"
                                             + "��ϵ��ʽ������281383656��qq�� ; massifor@hotmail(MSN) \r\n";

            this.txtIntroduction.Select(0, 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.lblURL.Text);
        }
    }
}