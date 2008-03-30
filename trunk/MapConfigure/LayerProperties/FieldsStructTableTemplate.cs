using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MapConfigure.LayerProperties
{
    class FieldsStructTableTemplate : System.Data.DataTable
    {
        public FieldsStructTableTemplate()
        {
            DataColumn colFieldName = new DataColumn("FieldName", typeof(String));
            colFieldName.Caption = "�ֶ�����";

            DataColumn colFieldLength = new DataColumn("FieldLength", typeof(Int32));
            colFieldLength.Caption = "�ֶγ���";

            DataColumn colFieldType = new DataColumn("FieldType", typeof(String));
            colFieldType.Caption = "�ֶ�����";

            this.Columns.AddRange(new DataColumn[] { colFieldName, colFieldLength, colFieldType });
        }
    }
}
