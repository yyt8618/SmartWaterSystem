using System;
using System.Windows.Forms;
using Entity;
using DevExpress.XtraEditors;
using BLL;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class TreeNodeInfoForm : DevExpress.XtraEditors.XtraForm
    {
        public static UniversalWayTypeEntity entity = null;
        public TreeNodeInfoForm(bool typeEnable, int selectedindex)
        {
            InitializeComponent();

            cbType.Enabled = typeEnable;
            cbType.SelectedIndex = selectedindex;
        }

        private void TreeNodeInfoForm_Load(object sender, EventArgs e)
        {
            if (cbType.Enabled)
                cbType.Focus();
            else
                txtName.Focus();
            cbPrecision.SelectedIndex = 2;

            entity = new UniversalWayTypeEntity();
        }

        public UniversalWayTypeEntity GetTypeEntity()
        {
            return entity;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                XtraMessageBox.Show("请输入采集名称!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return;
            }

            UniversalWayTypeBLL bll = new UniversalWayTypeBLL();
            int findresult=bll.TypeExist(((UniversalCollectType)cbType.SelectedIndex),txtName.Text);
            if (-1 == findresult)
            {
                XtraMessageBox.Show("检查采集名称发生异常,请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            else if (1 == findresult)
            {
                XtraMessageBox.Show("采集名称已存在，请重新输入!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus(); 
                return;
            }

            if (!cbType.Enabled && !Regex.IsMatch(txtMaxMeasureR.Text, @"^\d{1,2}$"))
            {
                XtraMessageBox.Show("请输入合法的帧宽度!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaxMeasureR.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtMaxMeasureR.Text))
            {
                if (!Regex.IsMatch(txtMaxMeasureR.Text, @"^\d+(\.\d+)?$"))
                {
                    XtraMessageBox.Show("请输入合法的最大测量范围!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaxMeasureR.Focus();
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtMaxMeasureRFlag.Text))
            {
                if (!Regex.IsMatch(txtMaxMeasureRFlag.Text, @"^\d+(\.\d+)?$"))
                {
                    XtraMessageBox.Show("请输入合法的仪表最大测量范围!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMaxMeasureRFlag.Focus();
                    return;
                }
            }

            int maxid = bll.GetMaxId();
            if (-1 != maxid)
            {
                entity.ID = maxid + 1;
            }
            else
            {
                XtraMessageBox.Show("读取数据库最大ID失败,请联系管理员!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!cbType.Enabled)
                entity.FrameWidth = Convert.ToInt32(txtFrameWidth.Text);
            if (!string.IsNullOrEmpty(txtMaxMeasureR.Text))
                entity.MaxMeasureRange = Convert.ToSingle(txtMaxMeasureR.Text);
            if (!string.IsNullOrEmpty(txtMaxMeasureRFlag.Text))
                entity.ManMeasureRangeFlag = Convert.ToSingle(txtMaxMeasureRFlag.Text);

            entity.ModifyTime = DateTime.Now;
            entity.Name = txtName.Text;
            entity.Precision = Convert.ToInt32(cbPrecision.Text);
            entity.Unit = txtUnit.Text;
            entity.WayType = (UniversalCollectType)cbType.SelectedIndex;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
