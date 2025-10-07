namespace StrukturovanaData
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (components != null)) components.Dispose();
            }
            catch { }
            finally { base.Dispose(disposing); }
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cboFilterColumn = new System.Windows.Forms.ComboBox();
            this.cboFilterMode = new System.Windows.Forms.ComboBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.btnFilterApply = new System.Windows.Forms.Button();
            this.btnFilterClear = new System.Windows.Forms.Button();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();

            this.cmsColumn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAkce = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.miInsertBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.miInsertAfter = new System.Windows.Forms.ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsColumn.SuspendLayout();
            this.SuspendLayout();

            // dataGridView1
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1100, 558);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.AllowUserToAddRows = true;
            this.dataGridView1.AllowUserToDeleteRows = true;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // panelTop
            this.panelTop.Controls.Add(this.lblFilter);
            this.panelTop.Controls.Add(this.cboFilterColumn);
            this.panelTop.Controls.Add(this.cboFilterMode);
            this.panelTop.Controls.Add(this.txtFilterValue);
            this.panelTop.Controls.Add(this.btnFilterApply);
            this.panelTop.Controls.Add(this.btnFilterClear);
            this.panelTop.Controls.Add(this.btnAddColumn);
            this.panelTop.Controls.Add(this.btnReload);
            this.panelTop.Controls.Add(this.btnSave);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1100, 72);
            this.panelTop.TabIndex = 1;

            // lblFilter
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(12, 12);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(33, 15);
            this.lblFilter.Text = "Filtr:";

            // cboFilterColumn
            this.cboFilterColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterColumn.Location = new System.Drawing.Point(55, 8);
            this.cboFilterColumn.Name = "cboFilterColumn";
            this.cboFilterColumn.Size = new System.Drawing.Size(160, 23);

            // cboFilterMode
            this.cboFilterMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterMode.Location = new System.Drawing.Point(221, 8);
            this.cboFilterMode.Name = "cboFilterMode";
            this.cboFilterMode.Size = new System.Drawing.Size(120, 23);

            // txtFilterValue
            this.txtFilterValue.Location = new System.Drawing.Point(347, 9);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(200, 23);

            // btnFilterApply
            this.btnFilterApply.Location = new System.Drawing.Point(553, 8);
            this.btnFilterApply.Name = "btnFilterApply";
            this.btnFilterApply.Size = new System.Drawing.Size(85, 24);
            this.btnFilterApply.TabIndex = 4;
            this.btnFilterApply.Text = "Filtrovat";
            this.btnFilterApply.UseVisualStyleBackColor = true;
            this.btnFilterApply.Click += new System.EventHandler(this.btnFilterApply_Click);

            // btnFilterClear
            this.btnFilterClear.Location = new System.Drawing.Point(644, 8);
            this.btnFilterClear.Name = "btnFilterClear";
            this.btnFilterClear.Size = new System.Drawing.Size(95, 24);
            this.btnFilterClear.TabIndex = 5;
            this.btnFilterClear.Text = "Zrušit filtr";
            this.btnFilterClear.UseVisualStyleBackColor = true;
            this.btnFilterClear.Click += new System.EventHandler(this.btnFilterClear_Click);

            // btnAddColumn
            this.btnAddColumn.Location = new System.Drawing.Point(12, 40);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(110, 24);
            this.btnAddColumn.TabIndex = 6;
            this.btnAddColumn.Text = "Přidat sloupec";
            this.btnAddColumn.UseVisualStyleBackColor = true;
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);

            // btnReload
            this.btnReload.Location = new System.Drawing.Point(128, 40);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(95, 24);
            this.btnReload.TabIndex = 7;
            this.btnReload.Text = "Načíst z disku";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);

            // btnSave
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(993, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 24);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Uložit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 630);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip1.TabIndex = 2;

            // toolStripStatusLabel1
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(8, 3, 0, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(147, 17);
            this.toolStripStatusLabel1.Text = "Připraveno k editaci CSV...";

            // cmsColumn (context menu)
            this.cmsColumn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.miAkce
            });
            this.cmsColumn.Name = "cmsColumn";
            this.cmsColumn.Size = new System.Drawing.Size(108, 26);

            // miAkce (parent)
            this.miAkce.Name = "miAkce";
            this.miAkce.Size = new System.Drawing.Size(107, 22);
            this.miAkce.Text = "Akce";
            this.miAkce.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.miDeleteColumn,
                this.miInsertBefore,
                this.miInsertAfter
            });

            // miDeleteColumn
            this.miDeleteColumn.Name = "miDeleteColumn";
            this.miDeleteColumn.Size = new System.Drawing.Size(190, 22);
            this.miDeleteColumn.Text = "Smazat";
            this.miDeleteColumn.Click += new System.EventHandler(this.miDeleteColumn_Click);

            // miInsertBefore
            this.miInsertBefore.Name = "miInsertBefore";
            this.miInsertBefore.Size = new System.Drawing.Size(190, 22);
            this.miInsertBefore.Text = "Vložit sloupec před";
            this.miInsertBefore.Click += new System.EventHandler(this.miInsertBefore_Click);

            // miInsertAfter
            this.miInsertAfter.Name = "miInsertAfter";
            this.miInsertAfter.Size = new System.Drawing.Size(190, 22);
            this.miInsertAfter.Text = "Vložit sloupec za";
            this.miInsertAfter.Click += new System.EventHandler(this.miInsertAfter_Click);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 652);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "CSV Editor (Data.csv)";
            this.Load += new System.EventHandler(this.Form1_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsColumn.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnAddColumn;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

        // Filtrace
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cboFilterColumn;
        private System.Windows.Forms.ComboBox cboFilterMode;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Button btnFilterApply;
        private System.Windows.Forms.Button btnFilterClear;

        // Context menu
        private System.Windows.Forms.ContextMenuStrip cmsColumn;
        private System.Windows.Forms.ToolStripMenuItem miAkce;
        private System.Windows.Forms.ToolStripMenuItem miDeleteColumn;
        private System.Windows.Forms.ToolStripMenuItem miInsertBefore;
        private System.Windows.Forms.ToolStripMenuItem miInsertAfter;
    }
}
