using SmartLocationApp.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace SmartLocationApp.Router
{
  public static class Animation
  {
    public static string Url = "";
    public static string[] servers = new string[5]
    {
      "c-stpreport.com",
      "ne-stpreport.com",
      "nw-stpreport.com",
      "se-stpreport.com",
      "uae-stpreport.com"
    };

    public static string GetUrl(int server, string mode) => "http://" + Animation.servers[server] + (mode == "regular" ? "/Api/" : "/test/Api/");

    public static void AnimationAdd(UserControl myUserControl)
    {
      myUserControl.SuspendLayout();
      Panel panel = new Panel();
      PictureBox pictureBox = new PictureBox();
      pictureBox.Image = (Image) Resources.Preloader;
      pictureBox.Location = new Point((myUserControl.Width - 75) / 2, (myUserControl.Height - 75) / 2);
      pictureBox.Name = "pictureBox1";
      pictureBox.BackColor = Color.White;
      pictureBox.Size = new Size(75, 75);
      pictureBox.TabIndex = 0;
      pictureBox.TabStop = false;
      panel.Location = new Point(0, 0);
      panel.Name = "panel1";
      panel.Size = new Size(myUserControl.Width, myUserControl.Height);
      panel.TabIndex = 10;
      panel.BackColor = Color.White;
      panel.SuspendLayout();
      panel.Controls.Add((Control) pictureBox);
      panel.ResumeLayout();
      myUserControl.Controls.Add((Control) panel);
      panel.BringToFront();
    }

    public static void AnimationRemove(UserControl myUserControl) => myUserControl.Controls.RemoveByKey("panel1");

    public static void AnimationAdd(Form myUserControl, int imageWidth)
    {
      myUserControl.SuspendLayout();
      PictureBox pictureBox = new PictureBox();
      pictureBox.Image = (Image) Resources.Preloader;
      pictureBox.Location = new Point((myUserControl.Width - imageWidth) / 2, (myUserControl.Height - imageWidth) / 2);
      pictureBox.Name = "pictureBox1";
      pictureBox.Size = new Size(imageWidth, imageWidth);
      pictureBox.TabIndex = 0;
      pictureBox.TabStop = false;
      Panel panel = new Panel();
      panel.Location = new Point(0, 0);
      panel.Name = "panel1";
      panel.Size = new Size(myUserControl.Width, myUserControl.Height);
      panel.TabIndex = 10;
      panel.BackColor = Color.White;
      panel.SuspendLayout();
      panel.Controls.Add((Control) pictureBox);
      panel.ResumeLayout();
      myUserControl.Controls.Add((Control) panel);
      panel.BringToFront();
    }

    public static void AnimationAdd(Form myUserControl)
    {
      myUserControl.SuspendLayout();
      PictureBox pictureBox = new PictureBox();
      pictureBox.Image = (Image) Resources.Preloader;
      pictureBox.Location = new Point((myUserControl.Width - 75) / 2, (myUserControl.Height - 75) / 2);
      pictureBox.Name = "pictureBox1";
      pictureBox.Size = new Size(75, 75);
      pictureBox.TabIndex = 0;
      pictureBox.TabStop = false;
      Panel panel = new Panel();
      panel.Location = new Point(0, 0);
      panel.Name = "panel1";
      panel.Size = new Size(myUserControl.Width, myUserControl.Height);
      panel.TabIndex = 10;
      panel.BackColor = Color.White;
      panel.SuspendLayout();
      panel.Controls.Add((Control) pictureBox);
      panel.ResumeLayout();
      myUserControl.Controls.Add((Control) panel);
      panel.BringToFront();
    }

    public static void AnimationRemove(Form myUserControl) => myUserControl.Controls.RemoveByKey("panel1");
  }
}
