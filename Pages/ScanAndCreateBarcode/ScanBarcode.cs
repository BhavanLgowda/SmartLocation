using SmartLocationApp.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartLocationApp.Pages.ScanAndCreateBarcode
{
  public class ScanBarcode : UserControl
  {
    private IContainer components;
    private TextBox textBox1;
    private PictureBox pictureBox1;
    private Label label1;
    private Button button1;
    private Button button2;

    public ScanBarcode() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ScanBarcode));
      this.textBox1 = new TextBox();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.textBox1.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBox1.Location = new Point(125, 114);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(726, 45);
      this.textBox1.TabIndex = 0;
      this.pictureBox1.Image = (Image) Resources.matImage;
      this.pictureBox1.Location = new Point(298, 190);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(413, 165);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonHighlight;
      this.label1.Location = new Point(3, 65);
      this.label1.Name = "label1";
      this.label1.Size = new Size(994, 32);
      this.label1.TabIndex = 3;
      this.label1.Text = "Scan Barcode";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.button1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.Location = new Point(273, 468);
      this.button1.Name = "button1";
      this.button1.Size = new Size(470, 50);
      this.button1.TabIndex = 1;
      this.button1.Text = "New  Ticket";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.button2.Location = new Point(858, 114);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 45);
      this.button2.TabIndex = 4;
      this.button2.Text = "Web Cam";
      this.button2.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.textBox1);
      this.Name = "ScanBarcode";
      this.Size = new Size(1000, 600);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
