Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.ViewInfo
Imports DevExpress.XtraTreeList.Nodes
Imports System.Collections
Imports DevExpress.XtraTreeList.Nodes.Operations
Imports DevExpress.Utils

Namespace WindowsApplication1
    <System.ComponentModel.DesignerCategory("")> _
    Public Class MyTreeList
        Inherits TreeList

        Private indexPaintAppearance As New AppearanceObject()
        Private _Helper As IndexesHelper
        Public Property Helper() As IndexesHelper
            Get
                If _Helper Is Nothing Then
                    _Helper = New IndexesHelper(Me)
                End If
                Return _Helper
            End Get
            Set(ByVal value As IndexesHelper)
                _Helper = value
            End Set
        End Property
        Public Sub New()
            indexPaintAppearance.ForeColor = Color.DarkBlue
            indexPaintAppearance.Font = New Font(AppearanceObject.DefaultFont.FontFamily, 6, FontStyle.Bold)
            indexPaintAppearance.TextOptions.HAlignment = HorzAlignment.Far
            indexPaintAppearance.TextOptions.VAlignment = VertAlignment.Center
            AddHandler PaintEx, AddressOf MyTreeList_PaintEx
        End Sub

        Private Sub MyTreeList_PaintEx(ByVal sender As Object, ByVal e As TreeListPaintEventArgs)
            For Each rowInfo As RowInfo In ViewInfo.RowsInfo.Rows
                Dim bounds As Rectangle = rowInfo.Bounds
                bounds.Width = 14
                e.Cache.DrawString(Helper.GetIndexByNode(rowInfo.Node).ToString(), indexPaintAppearance.Font, indexPaintAppearance.ForeColor, bounds, indexPaintAppearance.GetStringFormat())
            Next rowInfo
        End Sub

        Protected Sub New(ByVal ignore As Object)
            MyBase.New(ignore)

        End Sub
        Protected Overrides Sub RaiseCustomDrawNodeIndicator(ByVal e As CustomDrawNodeIndicatorEventArgs)
            MyBase.RaiseCustomDrawNodeIndicator(e)
            e.ImageIndex = -1
        End Sub

        Public Overrides Sub LayoutChanged()
            MyBase.LayoutChanged()
            If IsInitialized Then
                Helper.RefreshIndexesHash()
            End If
        End Sub
    End Class

    Public Class IndexesHelper

        Public Sub New(ByVal treeList As TreeList)
            _TreeList = treeList
            RefreshIndexesHash()
        End Sub

        Private indexesHash As New Hashtable()
        Private ReadOnly _TreeList As TreeList

        Public Function GetIndexByNode(ByVal node As TreeListNode) As Integer
            If Not indexesHash.ContainsKey(node.Id) Then
                RefreshIndexesHash()
            End If
            Return DirectCast(indexesHash(node.Id), Integer)
        End Function
        Public Sub RefreshIndexesHash()
            indexesHash.Clear()
            _TreeList.NodesIterator.DoLocalOperation(New NodesIndexesHelperOperation(indexesHash), _TreeList.Nodes)
        End Sub
    End Class

    Public Class NodesIndexesHelperOperation
        Inherits TreeListOperation

        Private i As Integer = 0
        Private ReadOnly _Hash As Hashtable
        Public Sub New(ByVal hash As Hashtable)
            _Hash = hash
        End Sub

        Public Overrides Sub Execute(ByVal node As TreeListNode)
            i += 1
            _Hash.Add(node.Id, i)
        End Sub
    End Class
End Namespace
