using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.ViewInfo;
using DevExpress.XtraTreeList.Nodes;
using System.Collections;
using DevExpress.XtraTreeList.Nodes.Operations;
using DevExpress.Utils;

namespace WindowsApplication1 {
    [System.ComponentModel.DesignerCategory("")]
    public class MyTreeList : TreeList {
        private AppearanceObject indexPaintAppearance = new AppearanceObject();
        private IndexesHelper _Helper;
        public IndexesHelper Helper {
            get {
                if(_Helper == null)
                    _Helper = new IndexesHelper(this);
                return _Helper;
            }
            set { _Helper = value; }
        }
        public MyTreeList() {
            indexPaintAppearance.ForeColor = Color.DarkBlue;
            indexPaintAppearance.Font = new Font(AppearanceObject.DefaultFont.FontFamily, 6, FontStyle.Bold);
            indexPaintAppearance.TextOptions.HAlignment = HorzAlignment.Far;
            indexPaintAppearance.TextOptions.VAlignment = VertAlignment.Center;
            PaintEx += MyTreeList_PaintEx;
        }

        void MyTreeList_PaintEx(object sender, TreeListPaintEventArgs e) {
            foreach(RowInfo rowInfo in ViewInfo.RowsInfo.Rows) {
                Rectangle bounds = rowInfo.Bounds;
                bounds.Width = 14;
                e.Cache.DrawString(Helper.GetIndexByNode(rowInfo.Node).ToString(), indexPaintAppearance.Font, indexPaintAppearance.ForeColor, bounds, indexPaintAppearance.GetStringFormat());
            }
        }

        protected MyTreeList(object ignore)
            : base(ignore) {

        }
        protected override void RaiseCustomDrawNodeIndicator(CustomDrawNodeIndicatorEventArgs e) {
            base.RaiseCustomDrawNodeIndicator(e);
            e.ImageIndex = -1;
        }

        public override void LayoutChanged() {
            base.LayoutChanged();
            if(IsInitialized)
                Helper.RefreshIndexesHash();
        }
    }

    public class IndexesHelper {

        public IndexesHelper(TreeList treeList) {
            _TreeList = treeList;
            RefreshIndexesHash();
        }

        Hashtable indexesHash = new Hashtable();
        private readonly TreeList _TreeList;

        public int GetIndexByNode(TreeListNode node) {
            if(!indexesHash.ContainsKey(node.Id))
                RefreshIndexesHash();
            return (int)indexesHash[node.Id];
        }
        public void RefreshIndexesHash() {
            indexesHash.Clear();
            _TreeList.NodesIterator.DoLocalOperation(new NodesIndexesHelperOperation(indexesHash), _TreeList.Nodes);
        }
    }

    public class NodesIndexesHelperOperation : TreeListOperation {
        int i = 0;
        private readonly Hashtable _Hash;
        public NodesIndexesHelperOperation(Hashtable hash) {
            _Hash = hash;
        }

        public override void Execute(TreeListNode node) {
            i++;
            _Hash.Add(node.Id, i);
        }
    }
}
