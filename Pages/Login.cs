using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class Login : UserControl
  {
    private IContainer components;
    private TextBox textBox1;
    private TextBox textBox2;
    private ContextMenuStrip contextMenuStrip1;
    private Label label1;
    private Label label2;
    private Button button1;

    public Login() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.label1 = new Label();
      this.label2 = new Label();
      this.button1 = new Button();
      this.SuspendLayout();
      this.textBox1.Font = new Font("Arial Unicode MS", 24f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBox1.Location = new Point(259, 117);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(500, 50);
      this.textBox1.TabIndex = 0;
      this.textBox2.Font = new Font("Arial Unicode MS", 24f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.textBox2.Location = new Point(259, 209);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(500, 50);
      this.textBox2.TabIndex = 1;
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(61, 4);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial Unicode MS", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ControlLight;
      this.label1.Location = new Point(253, 78);
      this.label1.Name = "label1";
      this.label1.Size = new Size((int) sbyte.MaxValue, 33);
      this.label1.TabIndex = 3;
      this.label1.Text = "UserName";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial Unicode MS", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = SystemColors.ControlLight;
      this.label2.Location = new Point(253, 170);
      this.label2.Name = "label2";
      this.label2.Size = new Size(119, 33);
      this.label2.TabIndex = 4;
      this.label2.Text = "Password";
      this.button1.Font = new Font("Arial Unicode MS", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.Location = new Point(631, 295);
      this.button1.Name = "button1";
      this.button1.Size = new Size((int) sbyte.MaxValue, 50);
      this.button1.TabIndex = 5;
      this.button1.Text = nameof (Login);
      this.button1.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(12, 45, 78);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.textBox1);
      this.Name = nameof (Login);
      this.Size = new Size(1000, 600);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
