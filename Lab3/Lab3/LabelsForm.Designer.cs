namespace Lab3
{
	partial class LabelsForm
	{
		private System.ComponentModel.IContainer components = null;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.Labels = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// Labels
			// 
			this.Labels.ForeColor = System.Drawing.Color.Black;
			this.Labels.FormattingEnabled = true;
			this.Labels.ItemHeight = 20;
			this.Labels.Location = new System.Drawing.Point(0, 0);
			this.Labels.Name = "Labels";
			this.Labels.Size = new System.Drawing.Size(668, 564);
			this.Labels.TabIndex = 0;
			// 
			// MessageLabels
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(669, 560);
			this.Controls.Add(this.Labels);
			this.Name = "MessageLabels";
			this.Text = "Message Labels";
			this.Load += new System.EventHandler(this.Form2_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox Labels;
	}
}