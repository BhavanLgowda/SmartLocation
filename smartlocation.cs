using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartLocationApp
{
  public class smartlocation : UserControl
  {
    private IContainer components;
    private TextBox textBox1;
    private Label label1;
    private Button button4;
    private Button button1;

    public smartlocation() => this.InitializeComponent();

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void smartlocation_Load(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.label1 = new Label();
      this.button4 = new Button();
      this.button1 = new Button();
      this.SuspendLayout();
      this.textBox1.Font = new Font("Microsoft Sans Serif", 24f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBox1.Location = new Point(118, 184);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(788, 44);
      this.textBox1.TabIndex = 0;
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(429, 144);
      this.label1.Name = "label1";
      this.label1.Size = new Size(180, 24);
      this.label1.TabIndex = 1;
      this.label1.Text = "Barcode Number";
      this.button4.BackColor = Color.MediumSeaGreen;
      this.button4.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button4.ForeColor = SystemColors.ButtonHighlight;
      this.button4.Location = new Point(314, 661);
      this.button4.Name = "button4";
      this.button4.Size = new Size(394, 53);
      this.button4.TabIndex = 5;
      this.button4.Text = "New Customer Ticket";
      this.button4.UseVisualStyleBackColor = false;
      this.button1.Location = new Point(909, 35);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 57);
      this.button1.TabIndex = 6;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(0, 0, 64);
      this.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox1);
      this.Name = nameof (smartlocation);
      this.Size = new Size(1022, 766);
      this.Load += new EventHandler(this.smartlocation_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
