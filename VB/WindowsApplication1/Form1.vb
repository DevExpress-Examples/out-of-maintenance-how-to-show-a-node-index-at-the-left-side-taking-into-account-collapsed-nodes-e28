Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraTreeList.Nodes.Operations
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.XtraTreeList
Imports DevExpress.Utils
Imports DevExpress.XtraTreeList.ViewInfo

Namespace WindowsApplication1
    Partial Public Class Form1
        Inherits Form

        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As New DataTable()
            tbl.Columns.Add("Name", GetType(String))
            tbl.Columns.Add("ID", GetType(Integer))
            tbl.Columns.Add("Number", GetType(Integer))
            tbl.Columns.Add("Date", GetType(Date))
            tbl.Columns.Add("ParentID", GetType(Integer))
            For i As Integer = 0 To RowCount - 1
                tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i + 1, 3 - i, Date.Now.AddDays(i), i Mod 3 })
            Next i
            Return tbl
        End Function

        Public Sub New()
            InitializeComponent()
            myTreeList1.DataSource = CreateTable(20)

        End Sub
    End Class
End Namespace