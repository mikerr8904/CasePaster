using System.Windows.Forms;

namespace CasePaster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxClipBoard = new System.Windows.Forms.CheckBox();
            this.checkBox_eTicket = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "eTicket Case #";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // checkBoxClipBoard
            // 
            this.checkBoxClipBoard.AutoSize = true;
            this.checkBoxClipBoard.Location = new System.Drawing.Point(30, 46);
            this.checkBoxClipBoard.Name = "checkBoxClipBoard";
            this.checkBoxClipBoard.Size = new System.Drawing.Size(109, 17);
            this.checkBoxClipBoard.TabIndex = 0;
            this.checkBoxClipBoard.Text = "Copy to Clipboard";
            this.checkBoxClipBoard.UseVisualStyleBackColor = true;
            // 
            // checkBox_eTicket
            // 
            this.checkBox_eTicket.AutoSize = true;
            this.checkBox_eTicket.Location = new System.Drawing.Point(30, 68);
            this.checkBox_eTicket.Name = "checkBox_eTicket";
            this.checkBox_eTicket.Size = new System.Drawing.Size(96, 17);
            this.checkBox_eTicket.TabIndex = 1;
            this.checkBox_eTicket.Text = "Add to eTicket";
            this.checkBox_eTicket.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Case # Behavior";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(121, 87);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(64, 20);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 117);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_eTicket);
            this.Controls.Add(this.checkBoxClipBoard);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "DR# Copy Paste";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NotifyIcon notifyIcon1;
        private CheckBox checkBoxClipBoard;
        private CheckBox checkBox_eTicket;
        private Label label1;
        private Button buttonSave;
    }
}