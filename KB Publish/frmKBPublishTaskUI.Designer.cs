namespace oh22is.SqlServer.DQS
{
    partial class frmKBPublishTaskUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKBPublishTaskUI));
            this.label1 = new System.Windows.Forms.Label();
            this.cbConnectionManager = new System.Windows.Forms.ComboBox();
            this.cbKnowledgeBase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.cbException = new System.Windows.Forms.CheckBox();
            this.cbEncryptConnection = new System.Windows.Forms.CheckBox();
            this.tpInformation = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCodeplex = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.linkOH22 = new System.Windows.Forms.LinkLabel();
            this.linkCodeplex = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.tpInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Quality Connection Manager:";
            // 
            // cbConnectionManager
            // 
            this.cbConnectionManager.FormattingEnabled = true;
            this.cbConnectionManager.Location = new System.Drawing.Point(182, 11);
            this.cbConnectionManager.Name = "cbConnectionManager";
            this.cbConnectionManager.Size = new System.Drawing.Size(343, 21);
            this.cbConnectionManager.TabIndex = 1;
            this.cbConnectionManager.SelectedIndexChanged += new System.EventHandler(this.cbConnectionManager_SelectedIndexChanged);
            // 
            // cbKnowledgeBase
            // 
            this.cbKnowledgeBase.FormattingEnabled = true;
            this.cbKnowledgeBase.Location = new System.Drawing.Point(182, 38);
            this.cbKnowledgeBase.Name = "cbKnowledgeBase";
            this.cbKnowledgeBase.Size = new System.Drawing.Size(343, 21);
            this.cbKnowledgeBase.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data Quality Knowledge Base:";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(531, 9);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(482, 226);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(563, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpProperties);
            this.tabControl.Controls.Add(this.tpInformation);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 208);
            this.tabControl.TabIndex = 7;
            // 
            // tpProperties
            // 
            this.tpProperties.BackColor = System.Drawing.SystemColors.Control;
            this.tpProperties.Controls.Add(this.cbException);
            this.tpProperties.Controls.Add(this.cbEncryptConnection);
            this.tpProperties.Controls.Add(this.label1);
            this.tpProperties.Controls.Add(this.cbConnectionManager);
            this.tpProperties.Controls.Add(this.cbKnowledgeBase);
            this.tpProperties.Controls.Add(this.btnNew);
            this.tpProperties.Controls.Add(this.label2);
            this.tpProperties.Location = new System.Drawing.Point(4, 22);
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tpProperties.Size = new System.Drawing.Size(618, 182);
            this.tpProperties.TabIndex = 0;
            this.tpProperties.Text = "Properties";
            // 
            // cbException
            // 
            this.cbException.AutoSize = true;
            this.cbException.Checked = true;
            this.cbException.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbException.Location = new System.Drawing.Point(182, 88);
            this.cbException.Name = "cbException";
            this.cbException.Size = new System.Drawing.Size(307, 17);
            this.cbException.TabIndex = 6;
            this.cbException.Text = "Throw exception if knowledge base could not be published.";
            this.cbException.UseVisualStyleBackColor = true;
            // 
            // cbEncryptConnection
            // 
            this.cbEncryptConnection.AutoSize = true;
            this.cbEncryptConnection.Location = new System.Drawing.Point(182, 65);
            this.cbEncryptConnection.Name = "cbEncryptConnection";
            this.cbEncryptConnection.Size = new System.Drawing.Size(119, 17);
            this.cbEncryptConnection.TabIndex = 5;
            this.cbEncryptConnection.Text = "Encrypt Connection";
            this.cbEncryptConnection.UseVisualStyleBackColor = true;
            // 
            // tpInformation
            // 
            this.tpInformation.BackColor = System.Drawing.SystemColors.Control;
            this.tpInformation.Controls.Add(this.label10);
            this.tpInformation.Controls.Add(this.pictureBox1);
            this.tpInformation.Controls.Add(this.label8);
            this.tpInformation.Controls.Add(this.lblTitle);
            this.tpInformation.Controls.Add(this.lblCodeplex);
            this.tpInformation.Controls.Add(this.label9);
            this.tpInformation.Controls.Add(this.linkOH22);
            this.tpInformation.Controls.Add(this.linkCodeplex);
            this.tpInformation.Controls.Add(this.label11);
            this.tpInformation.Location = new System.Drawing.Point(4, 22);
            this.tpInformation.Name = "tpInformation";
            this.tpInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tpInformation.Size = new System.Drawing.Size(618, 182);
            this.tpInformation.TabIndex = 1;
            this.tpInformation.Text = "Information";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(75, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "1.2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::oh22is.SqlServer.DQS.Properties.Resources.oh22is_200x50;
            this.pictureBox1.Location = new System.Drawing.Point(412, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Version";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.lblTitle.Location = new System.Drawing.Point(6, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 26);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Publish DQS Knowledge Base Task";
            // 
            // lblCodeplex
            // 
            this.lblCodeplex.AutoSize = true;
            this.lblCodeplex.Location = new System.Drawing.Point(8, 43);
            this.lblCodeplex.Name = "lblCodeplex";
            this.lblCodeplex.Size = new System.Drawing.Size(300, 13);
            this.lblCodeplex.TabIndex = 33;
            this.lblCodeplex.Text = "This project is hosted on Codeplex and licensed under MS-PL.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Contributor:";
            // 
            // linkOH22
            // 
            this.linkOH22.AutoSize = true;
            this.linkOH22.Location = new System.Drawing.Point(75, 93);
            this.linkOH22.Name = "linkOH22";
            this.linkOH22.Size = new System.Drawing.Size(99, 13);
            this.linkOH22.TabIndex = 30;
            this.linkOH22.TabStop = true;
            this.linkOH22.Text = "http://www.oh22.is";
            this.linkOH22.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOH22_LinkClicked);
            // 
            // linkCodeplex
            // 
            this.linkCodeplex.AutoSize = true;
            this.linkCodeplex.Location = new System.Drawing.Point(75, 68);
            this.linkCodeplex.Name = "linkCodeplex";
            this.linkCodeplex.Size = new System.Drawing.Size(205, 13);
            this.linkCodeplex.TabIndex = 29;
            this.linkCodeplex.TabStop = true;
            this.linkCodeplex.Text = "https://domainvalueimport.codeplex.com/";
            this.linkCodeplex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCodeplex_LinkClicked);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Project:";
            // 
            // frmKBPublishTaskUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 261);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKBPublishTaskUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish DQS Knowledge Base Task";
            this.Load += new System.EventHandler(this.KBPublishUI_Load);
            this.tabControl.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.tpProperties.PerformLayout();
            this.tpInformation.ResumeLayout(false);
            this.tpInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbConnectionManager;
        private System.Windows.Forms.ComboBox cbKnowledgeBase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.TabPage tpInformation;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox cbEncryptConnection;
        private System.Windows.Forms.CheckBox cbException;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label lblCodeplex;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkOH22;
        private System.Windows.Forms.LinkLabel linkCodeplex;
        private System.Windows.Forms.Label label11;
    }
}