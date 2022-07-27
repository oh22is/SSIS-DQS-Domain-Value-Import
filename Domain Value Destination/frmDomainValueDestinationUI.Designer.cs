namespace oh22is.SqlServer.DQS
{
    partial class frmDomainValueDestinationUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDomainValueDestinationUI));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.cbPublish = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.cbIncorrectValues = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLeadingValue = new System.Windows.Forms.ComboBox();
            this.cbValueType = new System.Windows.Forms.ComboBox();
            this.cbSynonymValue = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEncryptConnection = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbConnectionManager = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbKnowledgeBase = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDomain = new System.Windows.Forms.ComboBox();
            this.tpInformation = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCodeplex = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.linkCodeplex = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.linkOH22 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(466, 395);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(547, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpProperties);
            this.tabControl.Controls.Add(this.tpInformation);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(610, 377);
            this.tabControl.TabIndex = 19;
            // 
            // tpProperties
            // 
            this.tpProperties.BackColor = System.Drawing.SystemColors.Control;
            this.tpProperties.Controls.Add(this.cbPublish);
            this.tpProperties.Controls.Add(this.label12);
            this.tpProperties.Controls.Add(this.cbLog);
            this.tpProperties.Controls.Add(this.cbIncorrectValues);
            this.tpProperties.Controls.Add(this.label7);
            this.tpProperties.Controls.Add(this.groupBox2);
            this.tpProperties.Controls.Add(this.groupBox1);
            this.tpProperties.Location = new System.Drawing.Point(4, 22);
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tpProperties.Size = new System.Drawing.Size(602, 351);
            this.tpProperties.TabIndex = 0;
            this.tpProperties.Text = "Properties";
            // 
            // cbPublish
            // 
            this.cbPublish.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPublish.FormattingEnabled = true;
            this.cbPublish.Items.AddRange(new object[] {
            "Never Publish",
            "Always Publish",
            "Publish When There Is No Error"});
            this.cbPublish.Location = new System.Drawing.Point(264, 312);
            this.cbPublish.Name = "cbPublish";
            this.cbPublish.Size = new System.Drawing.Size(238, 21);
            this.cbPublish.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 315);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(226, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Publish DQS Knowledge Base after execution:";
            // 
            // cbLog
            // 
            this.cbLog.AutoSize = true;
            this.cbLog.Enabled = false;
            this.cbLog.Location = new System.Drawing.Point(264, 289);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(214, 17);
            this.cbLog.TabIndex = 23;
            this.cbLog.Text = "Write every error as a warning to the log";
            this.cbLog.UseVisualStyleBackColor = true;
            // 
            // cbIncorrectValues
            // 
            this.cbIncorrectValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIncorrectValues.FormattingEnabled = true;
            this.cbIncorrectValues.Items.AddRange(new object[] {
            "Fail Component",
            "Ignore Failure",
            "Redirect rows to error output"});
            this.cbIncorrectValues.Location = new System.Drawing.Point(264, 262);
            this.cbIncorrectValues.Name = "cbIncorrectValues";
            this.cbIncorrectValues.Size = new System.Drawing.Size(238, 21);
            this.cbIncorrectValues.TabIndex = 22;
            this.cbIncorrectValues.SelectedIndexChanged += new System.EventHandler(this.cbIncorrectValues_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(240, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Specify how to handle rows with incorrect values:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbLeadingValue);
            this.groupBox2.Controls.Add(this.cbValueType);
            this.groupBox2.Controls.Add(this.cbSynonymValue);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(12, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(577, 109);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input Columns";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Type (optional):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Leading Value:";
            // 
            // cbLeadingValue
            // 
            this.cbLeadingValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLeadingValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLeadingValue.FormattingEnabled = true;
            this.cbLeadingValue.Location = new System.Drawing.Point(182, 19);
            this.cbLeadingValue.Name = "cbLeadingValue";
            this.cbLeadingValue.Size = new System.Drawing.Size(308, 21);
            this.cbLeadingValue.TabIndex = 8;
            this.cbLeadingValue.Click += new System.EventHandler(this.cbLeadingValue_Click);
            // 
            // cbValueType
            // 
            this.cbValueType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbValueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValueType.FormattingEnabled = true;
            this.cbValueType.Location = new System.Drawing.Point(182, 46);
            this.cbValueType.Name = "cbValueType";
            this.cbValueType.Size = new System.Drawing.Size(308, 21);
            this.cbValueType.TabIndex = 10;
            this.cbValueType.Click += new System.EventHandler(this.cbValueType_Click);
            // 
            // cbSynonymValue
            // 
            this.cbSynonymValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSynonymValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSynonymValue.FormattingEnabled = true;
            this.cbSynonymValue.Location = new System.Drawing.Point(182, 73);
            this.cbSynonymValue.Name = "cbSynonymValue";
            this.cbSynonymValue.Size = new System.Drawing.Size(308, 21);
            this.cbSynonymValue.TabIndex = 12;
            this.cbSynonymValue.Click += new System.EventHandler(this.cbSynonymValue_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Synonym (optional):";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbEncryptConnection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbConnectionManager);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbKnowledgeBase);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbDomain);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 131);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DQS Connection";
            // 
            // cbEncryptConnection
            // 
            this.cbEncryptConnection.AutoSize = true;
            this.cbEncryptConnection.Location = new System.Drawing.Point(182, 102);
            this.cbEncryptConnection.Name = "cbEncryptConnection";
            this.cbEncryptConnection.Size = new System.Drawing.Size(119, 17);
            this.cbEncryptConnection.TabIndex = 7;
            this.cbEncryptConnection.Text = "Encrypt Connection";
            this.cbEncryptConnection.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Quality Connection Manager:";
            // 
            // cbConnectionManager
            // 
            this.cbConnectionManager.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConnectionManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnectionManager.FormattingEnabled = true;
            this.cbConnectionManager.Location = new System.Drawing.Point(182, 21);
            this.cbConnectionManager.Name = "cbConnectionManager";
            this.cbConnectionManager.Size = new System.Drawing.Size(308, 21);
            this.cbConnectionManager.TabIndex = 1;
            this.cbConnectionManager.SelectedIndexChanged += new System.EventHandler(this.cbConnectionManager_SelectedIndexChanged);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(496, 19);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data Quality Knowledge Base:";
            // 
            // cbKnowledgeBase
            // 
            this.cbKnowledgeBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKnowledgeBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKnowledgeBase.FormattingEnabled = true;
            this.cbKnowledgeBase.Location = new System.Drawing.Point(182, 48);
            this.cbKnowledgeBase.Name = "cbKnowledgeBase";
            this.cbKnowledgeBase.Size = new System.Drawing.Size(308, 21);
            this.cbKnowledgeBase.TabIndex = 4;
            this.cbKnowledgeBase.SelectedIndexChanged += new System.EventHandler(this.cbKnowledgeBase_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Domain:";
            // 
            // cbDomain
            // 
            this.cbDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDomain.FormattingEnabled = true;
            this.cbDomain.Location = new System.Drawing.Point(182, 75);
            this.cbDomain.Name = "cbDomain";
            this.cbDomain.Size = new System.Drawing.Size(308, 21);
            this.cbDomain.TabIndex = 6;
            // 
            // tpInformation
            // 
            this.tpInformation.BackColor = System.Drawing.SystemColors.Control;
            this.tpInformation.Controls.Add(this.label10);
            this.tpInformation.Controls.Add(this.label8);
            this.tpInformation.Controls.Add(this.lblTitle);
            this.tpInformation.Controls.Add(this.lblCodeplex);
            this.tpInformation.Controls.Add(this.label9);
            this.tpInformation.Controls.Add(this.linkCodeplex);
            this.tpInformation.Controls.Add(this.label11);
            this.tpInformation.Controls.Add(this.linkOH22);
            this.tpInformation.Controls.Add(this.pictureBox1);
            this.tpInformation.Location = new System.Drawing.Point(4, 22);
            this.tpInformation.Name = "tpInformation";
            this.tpInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tpInformation.Size = new System.Drawing.Size(602, 351);
            this.tpInformation.TabIndex = 1;
            this.tpInformation.Text = "Information";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(75, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "1.2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Version";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.lblTitle.Location = new System.Drawing.Point(6, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(353, 29);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "DQS Domain Value Destination ";
            // 
            // lblCodeplex
            // 
            this.lblCodeplex.AutoSize = true;
            this.lblCodeplex.Location = new System.Drawing.Point(8, 43);
            this.lblCodeplex.Name = "lblCodeplex";
            this.lblCodeplex.Size = new System.Drawing.Size(300, 13);
            this.lblCodeplex.TabIndex = 26;
            this.lblCodeplex.Text = "This project is hosted on Codeplex and licensed under MS-PL.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Contributor:";
            // 
            // linkCodeplex
            // 
            this.linkCodeplex.AutoSize = true;
            this.linkCodeplex.Location = new System.Drawing.Point(75, 68);
            this.linkCodeplex.Name = "linkCodeplex";
            this.linkCodeplex.Size = new System.Drawing.Size(205, 13);
            this.linkCodeplex.TabIndex = 22;
            this.linkCodeplex.TabStop = true;
            this.linkCodeplex.Text = "https://domainvalueimport.codeplex.com/";
            this.linkCodeplex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCodeplex_LinkClicked_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Project:";
            // 
            // linkOH22
            // 
            this.linkOH22.AutoSize = true;
            this.linkOH22.Location = new System.Drawing.Point(75, 93);
            this.linkOH22.Name = "linkOH22";
            this.linkOH22.Size = new System.Drawing.Size(99, 13);
            this.linkOH22.TabIndex = 23;
            this.linkOH22.TabStop = true;
            this.linkOH22.Text = "http://www.oh22.is";
            this.linkOH22.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOH22_LinkClicked_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::oh22is.SqlServer.DQS.Properties.Resources.oh22is_200x50;
            this.pictureBox1.Location = new System.Drawing.Point(396, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // frmDomainValueDestinationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 433);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDomainValueDestinationUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQS Domain Value Import";
            this.Load += new System.EventHandler(this.frmDomainValueDestinationUI_Load);
            this.tabControl.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.tpProperties.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpInformation.ResumeLayout(false);
            this.tpInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbLeadingValue;
        private System.Windows.Forms.ComboBox cbValueType;
        private System.Windows.Forms.ComboBox cbSynonymValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbConnectionManager;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbKnowledgeBase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDomain;
        private System.Windows.Forms.TabPage tpInformation;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cbIncorrectValues;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbEncryptConnection;
        private System.Windows.Forms.CheckBox cbLog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label lblCodeplex;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkCodeplex;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel linkOH22;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbPublish;
    }
}