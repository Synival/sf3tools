namespace SF3.Win.Controls {
    partial class StatGrowthChartControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            var chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            var legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            cbCurveGraphCharacter = new DarkModeComboBox();
            CurveGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize) CurveGraph).BeginInit();
            SuspendLayout();
            // 
            // cbCurveGraphCharacter
            // 
            cbCurveGraphCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbCurveGraphCharacter.FormattingEnabled = true;
            cbCurveGraphCharacter.Location = new System.Drawing.Point(4, 3);
            cbCurveGraphCharacter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbCurveGraphCharacter.Name = "cbCurveGraphCharacter";
            cbCurveGraphCharacter.Size = new System.Drawing.Size(226, 23);
            cbCurveGraphCharacter.TabIndex = 0;
            cbCurveGraphCharacter.SelectedIndexChanged += CurveGraphCharacterComboBox_SelectedIndexChanged;
            // 
            // CurveGraph
            // 
            CurveGraph.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            chartArea1.AxisX.Minimum = 1D;
            chartArea1.Name = "ChartArea1";
            CurveGraph.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            CurveGraph.Legends.Add(legend1);
            CurveGraph.Location = new System.Drawing.Point(4, 32);
            CurveGraph.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CurveGraph.Name = "CurveGraph";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.FromArgb(  192,   192,   0);
            series1.Legend = "Legend1";
            series1.Name = "HP";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.FromArgb(  192,   0,   192);
            series2.Legend = "Legend1";
            series2.Name = "MP";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.Legend = "Legend1";
            series3.Name = "Atk";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Green;
            series4.Legend = "Legend1";
            series4.Name = "Def";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(  0,   0,   192);
            series5.Legend = "Legend1";
            series5.Name = "Agi";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series6.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series6.BorderWidth = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.FromArgb(  192,   192,   0);
            series6.Legend = "Legend1";
            series6.Name = "Likely HP";
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series7.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series7.BorderWidth = 2;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.FromArgb(  192,   0,   192);
            series7.Legend = "Legend1";
            series7.Name = "Likely MP";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series7.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series8.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series8.BorderWidth = 2;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Color = System.Drawing.Color.Red;
            series8.Legend = "Legend1";
            series8.Name = "Likely Atk";
            series8.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series8.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series9.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series9.BorderWidth = 2;
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Color = System.Drawing.Color.Green;
            series9.Legend = "Legend1";
            series9.Name = "Likely Def";
            series9.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series9.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series10.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series10.BorderWidth = 2;
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Color = System.Drawing.Color.FromArgb(  128,   0,   0,   192);
            series10.Legend = "Legend1";
            series10.Name = "Likely Agi";
            series10.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series10.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series11.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series11.Color = System.Drawing.Color.FromArgb(  128,   192,   192,   0);
            series11.Legend = "Legend1";
            series11.Name = "HP Range 1 (50% Likely)";
            series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series11.YValuesPerPoint = 2;
            series11.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series12.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series12.Color = System.Drawing.Color.FromArgb(  128,   192,   0,   192);
            series12.Legend = "Legend1";
            series12.Name = "MP Range 1 (50% Likely)";
            series12.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series12.YValuesPerPoint = 2;
            series12.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series13.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
            series13.ChartArea = "ChartArea1";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series13.Color = System.Drawing.Color.FromArgb(  128,   255,   0,   0);
            series13.Legend = "Legend1";
            series13.Name = "Atk Range 1 (50% Likely)";
            series13.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series13.YValuesPerPoint = 2;
            series13.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series14.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
            series14.ChartArea = "ChartArea1";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series14.Color = System.Drawing.Color.FromArgb(  128,   0,   128,   0);
            series14.Legend = "Legend1";
            series14.Name = "Def Range 1 (50% Likely)";
            series14.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series14.YValuesPerPoint = 2;
            series14.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series15.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent50;
            series15.ChartArea = "ChartArea1";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series15.Color = System.Drawing.Color.FromArgb(  128,   0,   0,   192);
            series15.Legend = "Legend1";
            series15.Name = "Agi Range 1 (50% Likely)";
            series15.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series15.YValuesPerPoint = 2;
            series15.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series16.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent70;
            series16.ChartArea = "ChartArea1";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series16.Color = System.Drawing.Color.FromArgb(  128,   192,   192,   0);
            series16.Legend = "Legend1";
            series16.Name = "HP Range 2 (99% Likely)";
            series16.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series16.YValuesPerPoint = 2;
            series16.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series17.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent70;
            series17.ChartArea = "ChartArea1";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series17.Color = System.Drawing.Color.FromArgb(  128,   192,   0,   192);
            series17.Legend = "Legend1";
            series17.Name = "MP Range 2 (99% Likely)";
            series17.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series17.YValuesPerPoint = 2;
            series17.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series18.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent70;
            series18.ChartArea = "ChartArea1";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series18.Color = System.Drawing.Color.FromArgb(  128,   255,   0,   0);
            series18.Legend = "Legend1";
            series18.Name = "Atk Range 2 (99% Likely)";
            series18.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series18.YValuesPerPoint = 2;
            series18.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series19.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent70;
            series19.ChartArea = "ChartArea1";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series19.Color = System.Drawing.Color.FromArgb(  128,   0,   128,   0);
            series19.Legend = "Legend1";
            series19.Name = "Def Range 2 (99% Likely)";
            series19.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series19.YValuesPerPoint = 2;
            series19.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series20.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent70;
            series20.ChartArea = "ChartArea1";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series20.Color = System.Drawing.Color.FromArgb(  128,   0,   0,   192);
            series20.Legend = "Legend1";
            series20.Name = "Agi Range 2 (99% Likely)";
            series20.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series20.YValuesPerPoint = 2;
            series20.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            CurveGraph.Series.Add(series1);
            CurveGraph.Series.Add(series2);
            CurveGraph.Series.Add(series3);
            CurveGraph.Series.Add(series4);
            CurveGraph.Series.Add(series5);
            CurveGraph.Series.Add(series6);
            CurveGraph.Series.Add(series7);
            CurveGraph.Series.Add(series8);
            CurveGraph.Series.Add(series9);
            CurveGraph.Series.Add(series10);
            CurveGraph.Series.Add(series11);
            CurveGraph.Series.Add(series12);
            CurveGraph.Series.Add(series13);
            CurveGraph.Series.Add(series14);
            CurveGraph.Series.Add(series15);
            CurveGraph.Series.Add(series16);
            CurveGraph.Series.Add(series17);
            CurveGraph.Series.Add(series18);
            CurveGraph.Series.Add(series19);
            CurveGraph.Series.Add(series20);
            CurveGraph.Size = new System.Drawing.Size(653, 412);
            CurveGraph.TabIndex = 1;
            CurveGraph.Text = "chartCurveGraph";
            CurveGraph.MouseMove += CurveGraph_MouseMove;
            // 
            // StatGrowthChartControl
            // 
            Controls.Add(CurveGraph);
            Controls.Add(cbCurveGraphCharacter);
            Name = "StatGrowthChartControl";
            Size = new System.Drawing.Size(661, 447);
            ((System.ComponentModel.ISupportInitialize) CurveGraph).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DarkModeComboBox cbCurveGraphCharacter;
        private System.Windows.Forms.DataVisualization.Charting.Chart CurveGraph;
    }
}
