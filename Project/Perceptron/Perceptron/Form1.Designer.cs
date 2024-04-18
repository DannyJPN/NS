
namespace Perceptron
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.BChooseSet = new System.Windows.Forms.Button();
            this.BStartLearn = new System.Windows.Forms.Button();
            this.BStartTest = new System.Windows.Forms.Button();
            this.NUDSpeedController = new System.Windows.Forms.NumericUpDown();
            this.LSpeed = new System.Windows.Forms.Label();
            this.PVGraph = new OxyPlot.WindowsForms.PlotView();
            this.DGVValues = new System.Windows.Forms.DataGridView();
            this.OFDOpener = new System.Windows.Forms.OpenFileDialog();
            this.LIterations = new System.Windows.Forms.Label();
            this.NUDIterations = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NUDSpeedController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDIterations)).BeginInit();
            this.SuspendLayout();
            // 
            // BChooseSet
            // 
            this.BChooseSet.Location = new System.Drawing.Point(12, 26);
            this.BChooseSet.Name = "BChooseSet";
            this.BChooseSet.Size = new System.Drawing.Size(124, 23);
            this.BChooseSet.TabIndex = 0;
            this.BChooseSet.Text = "CHoose Set";
            this.BChooseSet.UseVisualStyleBackColor = true;
            this.BChooseSet.Click += new System.EventHandler(this.ChooseSet);
            // 
            // BStartLearn
            // 
            this.BStartLearn.Location = new System.Drawing.Point(12, 55);
            this.BStartLearn.Name = "BStartLearn";
            this.BStartLearn.Size = new System.Drawing.Size(124, 23);
            this.BStartLearn.TabIndex = 2;
            this.BStartLearn.Text = "Start Learning";
            this.BStartLearn.UseVisualStyleBackColor = true;
            this.BStartLearn.Click += new System.EventHandler(this.BStartLearn_Click);
            // 
            // BStartTest
            // 
            this.BStartTest.Location = new System.Drawing.Point(12, 84);
            this.BStartTest.Name = "BStartTest";
            this.BStartTest.Size = new System.Drawing.Size(124, 23);
            this.BStartTest.TabIndex = 3;
            this.BStartTest.Text = "Start Testing";
            this.BStartTest.UseVisualStyleBackColor = true;
            this.BStartTest.Click += new System.EventHandler(this.BStartTest_Click);
            // 
            // NUDSpeedController
            // 
            this.NUDSpeedController.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUDSpeedController.Location = new System.Drawing.Point(16, 127);
            this.NUDSpeedController.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.NUDSpeedController.Name = "NUDSpeedController";
            this.NUDSpeedController.Size = new System.Drawing.Size(120, 20);
            this.NUDSpeedController.TabIndex = 4;
            this.NUDSpeedController.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.NUDSpeedController.ValueChanged += new System.EventHandler(this.NUDSpeedController_ValueChanged);
            // 
            // LSpeed
            // 
            this.LSpeed.AutoSize = true;
            this.LSpeed.Location = new System.Drawing.Point(12, 110);
            this.LSpeed.Name = "LSpeed";
            this.LSpeed.Size = new System.Drawing.Size(60, 13);
            this.LSpeed.TabIndex = 5;
            this.LSpeed.Text = "Speed (ms)";
            // 
            // PVGraph
            // 
            this.PVGraph.Location = new System.Drawing.Point(264, 1);
            this.PVGraph.Name = "PVGraph";
            this.PVGraph.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.PVGraph.Size = new System.Drawing.Size(711, 445);
            this.PVGraph.TabIndex = 6;
            this.PVGraph.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.PVGraph.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.PVGraph.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // DGVValues
            // 
            this.DGVValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVValues.Location = new System.Drawing.Point(10, 464);
            this.DGVValues.Name = "DGVValues";
            this.DGVValues.RowHeadersWidth = 51;
            this.DGVValues.Size = new System.Drawing.Size(561, 70);
            this.DGVValues.TabIndex = 7;
            // 
            // OFDOpener
            // 
            this.OFDOpener.FileName = "OFDopener";
            // 
            // LIterations
            // 
            this.LIterations.AutoSize = true;
            this.LIterations.Location = new System.Drawing.Point(12, 154);
            this.LIterations.Name = "LIterations";
            this.LIterations.Size = new System.Drawing.Size(50, 13);
            this.LIterations.TabIndex = 9;
            this.LIterations.Text = "Iterations";
            // 
            // NUDIterations
            // 
            this.NUDIterations.Location = new System.Drawing.Point(16, 170);
            this.NUDIterations.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.NUDIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUDIterations.Name = "NUDIterations";
            this.NUDIterations.Size = new System.Drawing.Size(120, 20);
            this.NUDIterations.TabIndex = 8;
            this.NUDIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 544);
            this.Controls.Add(this.LIterations);
            this.Controls.Add(this.NUDIterations);
            this.Controls.Add(this.DGVValues);
            this.Controls.Add(this.PVGraph);
            this.Controls.Add(this.LSpeed);
            this.Controls.Add(this.NUDSpeedController);
            this.Controls.Add(this.BStartTest);
            this.Controls.Add(this.BStartLearn);
            this.Controls.Add(this.BChooseSet);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.NUDSpeedController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDIterations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BChooseSet;
        private System.Windows.Forms.Button BStartLearn;
        private System.Windows.Forms.Button BStartTest;
        private System.Windows.Forms.NumericUpDown NUDSpeedController;
        private System.Windows.Forms.Label LSpeed;
        private OxyPlot.WindowsForms.PlotView PVGraph;
        private System.Windows.Forms.DataGridView DGVValues;
        private System.Windows.Forms.OpenFileDialog OFDOpener;
        private System.Windows.Forms.Label LIterations;
        private System.Windows.Forms.NumericUpDown NUDIterations;
    }
}

