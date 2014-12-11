using System;
using System.Windows.Forms;
using System.Collections;

namespace NoiseAnalysisSystem
{
	/// <summary>
	/// A simple form to list out all parameters passed to ClickHotSpot event
	/// handler for demo purposes.
	/// </summary>
    public partial class FrmParamViewer : DevExpress.XtraEditors.XtraForm
	{
		/// <summary>
		/// ParamViewer Constructor
		/// </summary>
		public FrmParamViewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// ParamViewer Constructor
		/// </summary>
		public void Display(object sender, ChartDirector.WinHotSpotEventArgs e)
		{
			// Add the name of the ChartViewer control that is being clicked
			//listView.Items.Add(new ListViewItem(new string[] {"source", 
			//    ((ChartDirector.WinChartViewer)sender).Name}));

			Hashtable entry = e.GetAttrValues();

			// List out the parameters of the hot spot
			foreach (DictionaryEntry key in entry)
			{
				if (key.Key.ToString() == "噪声幅度")
					listView.Items.Add(new ListViewItem(
						new string[] { (string)key.Key, (string)key.Value + "%" }));
			    else if (key.Key.ToString() == "噪声频率")
					listView.Items.Add(new ListViewItem(
						new string[] { (string)key.Key, (string)key.Value + "Hz" }));
				else if (key.Key.ToString() == "采集时间")
					listView.Items.Add(new ListViewItem(
						new string[] { (string)key.Key, (string)key.Value }));
				else if (key.Key.ToString() == "数据值")
					listView.Items.Add(new ListViewItem(
						new string[] { (string)key.Key, (string)key.Value }));
				else if (key.Key.ToString() == "数据序号")
					listView.Items.Add(new ListViewItem(
						new string[] { (string)key.Key, (string)key.Value }));
				//else if (key.Key.ToString() == "coords")
				//    listView.Items.Add(new ListViewItem(
				//        new string[] { "鼠标位置", (string)key.Value }));
			}

			// Display the form
			ShowDialog();
		}
		
		/// <summary>
		/// Handler for the OK button
		/// </summary>
		private void OKPB_Click(object sender, System.EventArgs e)
		{
			// Just close the Form
			Close();
		}
	}
}
