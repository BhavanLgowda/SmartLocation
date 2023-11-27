using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class ModeChooser : UserControl
  {
    private PageRouter router;
    private IContainer components;
    private Label label1;
    private Button LocalServerButton;
    private Button PhotoSaleButton;
    private Button photoTakenPlace;

    public void init(PageRouter _router) => this.router = _router;

    public ModeChooser() => this.InitializeComponent();

    private void photoTakenPlace_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(0);
      this.router.goPhototakenPlaceSetting(this.router);
    }

    private void PhotoSaleButton_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(1);
      this.router.goPhotoSaleSetting(this.router);
    }

    private void LocalServerButton_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(2);
      this.router.goLocalPhotoSetting(this.router);
    }

    private void UpdateModeType(int Mode)
    {
      List<Datas> objectToWrite = !new FileInfo(ReadWrite.dbPath).Exists ? new List<Datas>() : ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath);
      Datas datas = objectToWrite.Count <= 0 ? new Datas() : objectToWrite[0];
      datas.Mode_Type = Mode;
      objectToWrite.Clear();
      objectToWrite.Add(datas);
      ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, objectToWrite);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.LocalServerButton = new Button();
      this.PhotoSaleButton = new Button();
      this.photoTakenPlace = new Button();
      this.SuspendLayout();
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Century Gothic", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(0, 150);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1000, 36);
      this.label1.TabIndex = 7;
      this.label1.Text = "SELECT MODE TYPE";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.LocalServerButton.BackColor = Color.DodgerBlue;
      this.LocalServerButton.Cursor = Cursors.Hand;
      this.LocalServerButton.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.LocalServerButton.ForeColor = Color.White;
      this.LocalServerButton.Location = new Point(75, 493);
      this.LocalServerButton.Name = "LocalServerButton";
      this.LocalServerButton.Size = new Size(850, 57);
      this.LocalServerButton.TabIndex = 6;
      this.LocalServerButton.Text = "Local Server";
      this.LocalServerButton.UseVisualStyleBackColor = false;
      this.LocalServerButton.Click += new EventHandler(this.LocalServerButton_Click);
      this.PhotoSaleButton.BackColor = Color.DodgerBlue;
      this.PhotoSaleButton.Cursor = Cursors.Hand;
      this.PhotoSaleButton.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.PhotoSaleButton.ForeColor = Color.White;
      this.PhotoSaleButton.Location = new Point(75, 384);
      this.PhotoSaleButton.Name = "PhotoSaleButton";
      this.PhotoSaleButton.Size = new Size(850, 57);
      this.PhotoSaleButton.TabIndex = 5;
      this.PhotoSaleButton.Text = "Photo Sales";
      this.PhotoSaleButton.UseVisualStyleBackColor = false;
      this.PhotoSaleButton.Click += new EventHandler(this.PhotoSaleButton_Click);
      this.photoTakenPlace.BackColor = Color.DodgerBlue;
      this.photoTakenPlace.Cursor = Cursors.Hand;
      this.photoTakenPlace.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.photoTakenPlace.ForeColor = Color.White;
      this.photoTakenPlace.Location = new Point(75, 275);
      this.photoTakenPlace.Name = "photoTakenPlace";
      this.photoTakenPlace.Size = new Size(850, 57);
      this.photoTakenPlace.TabIndex = 4;
      this.photoTakenPlace.Text = "Photo Taken Place";
      this.photoTakenPlace.UseVisualStyleBackColor = false;
      this.photoTakenPlace.Click += new EventHandler(this.photoTakenPlace_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources._999;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.LocalServerButton);
      this.Controls.Add((Control) this.PhotoSaleButton);
      this.Controls.Add((Control) this.photoTakenPlace);
      this.Name = nameof (ModeChooser);
      this.Size = new Size(1000, 700);
      this.ResumeLayout(false);
    }
  }
}
