namespace NoiseAnalysisSystem
{
    partial class NoiseMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraMap.InformationLayer informationLayer1 = new DevExpress.XtraMap.InformationLayer();
            DevExpress.XtraMap.ImageTilesLayer imageTilesLayer1 = new DevExpress.XtraMap.ImageTilesLayer();
            DevExpress.XtraMap.BingMapDataProvider bingMapDataProvider1 = new DevExpress.XtraMap.BingMapDataProvider();
            DevExpress.XtraMap.VectorFileLayer vectorFileLayer1 = new DevExpress.XtraMap.VectorFileLayer();
            DevExpress.XtraMap.KmlFileLoader kmlFileLoader1 = new DevExpress.XtraMap.KmlFileLoader();
            DevExpress.XtraMap.VectorItemsLayer vectorItemsLayer1 = new DevExpress.XtraMap.VectorItemsLayer();
            DevExpress.XtraMap.SizeLegend sizeLegend1 = new DevExpress.XtraMap.SizeLegend();
            DevExpress.XtraMap.ColorListLegend colorListLegend1 = new DevExpress.XtraMap.ColorListLegend();
            DevExpress.XtraMap.ColorScaleLegend colorScaleLegend1 = new DevExpress.XtraMap.ColorScaleLegend();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mapControl1 = new DevExpress.XtraMap.MapControl();
            ((System.ComponentModel.ISupportInitialize)(this.mapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.txt";
            this.openFileDialog.Filter = "txt文件|*.txt";
            // 
            // mapControl1
            // 
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            imageTilesLayer1.DataProvider = bingMapDataProvider1;
            vectorFileLayer1.FileLoader = kmlFileLoader1;
            this.mapControl1.Layers.Add(informationLayer1);
            this.mapControl1.Layers.Add(imageTilesLayer1);
            this.mapControl1.Layers.Add(vectorFileLayer1);
            this.mapControl1.Layers.Add(vectorItemsLayer1);
            this.mapControl1.Legends.Add(sizeLegend1);
            this.mapControl1.Legends.Add(colorListLegend1);
            this.mapControl1.Legends.Add(colorScaleLegend1);
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(701, 364);
            this.mapControl1.TabIndex = 0;
            // 
            // NoiseMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapControl1);
            this.Name = "NoiseMap";
            this.Size = new System.Drawing.Size(701, 364);
            this.Load += new System.EventHandler(this.TestChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraMap.MapControl mapControl1;

    }
}