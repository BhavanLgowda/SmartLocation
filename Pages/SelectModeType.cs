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
  public class SelectModeType : UserControl
  {
    private PageRouter router;
    private IContainer components;
    private Button photoTakenPlace;
    private Button PhotoSaleButton;
    private Button LocalServerButton;
    private Label label1;
    private Button buttonGalacticTvMode;
    private Button buttonBarcodePrintMode;

    public void init(PageRouter _router) => this.router = _router;

    public void InitSelectModeType(PageRouter _router) => this.router = _router;

    public SelectModeType()
    {
      this.InitializeComponent();
    } 

    private void photoTakenPlace_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(0);
      this.router.goPhototakenPlaceSetting(this.router);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(1);
      this.router.goPhotoSaleSetting(this.router);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(2);
      this.router.goLocalPhotoSetting(this.router);
    }

    private void buttonGalacticTvMode_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(3);
      this.router.goGalacticTvSetting(this.router);
    }

    private void buttonBarcodePrintMode_Click(object sender, EventArgs e)
    {
      this.UpdateModeType(4);
      this.router.goBarcodePrintSettings(this.router);
    }

    public void UpdateModeType(int Mode)
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
      this.photoTakenPlace = new Button();
      this.PhotoSaleButton = new Button();
      this.LocalServerButton = new Button();
      this.label1 = new Label();
      this.buttonGalacticTvMode = new Button();
      this.buttonBarcodePrintMode = new Button();
      this.SuspendLayout();
      this.photoTakenPlace.BackColor = Color.DodgerBlue;
      this.photoTakenPlace.Cursor = Cursors.Hand;
      this.photoTakenPlace.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.photoTakenPlace.ForeColor = Color.White;
      this.photoTakenPlace.Location = new Point(75, 150);
      this.photoTakenPlace.Name = "photoTakenPlace";
      this.photoTakenPlace.Size = new Size(850, 57);
      this.photoTakenPlace.TabIndex = 0;
      this.photoTakenPlace.Text = "Photo Taken Place";
      this.photoTakenPlace.UseVisualStyleBackColor = false;
      this.photoTakenPlace.Click += new EventHandler(this.photoTakenPlace_Click);
      this.PhotoSaleButton.BackColor = Color.DodgerBlue;
      this.PhotoSaleButton.Cursor = Cursors.Hand;
      this.PhotoSaleButton.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.PhotoSaleButton.ForeColor = Color.White;
      this.PhotoSaleButton.Location = new Point(75, 250);
      this.PhotoSaleButton.Name = "PhotoSaleButton";
      this.PhotoSaleButton.Size = new Size(850, 57);
      this.PhotoSaleButton.TabIndex = 1;
      this.PhotoSaleButton.Text = "Photo Sales";
      this.PhotoSaleButton.UseVisualStyleBackColor = false;
      this.PhotoSaleButton.Click += new EventHandler(this.button1_Click);
      this.LocalServerButton.BackColor = Color.DodgerBlue;
      this.LocalServerButton.Cursor = Cursors.Hand;
      this.LocalServerButton.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.LocalServerButton.ForeColor = Color.White;
      this.LocalServerButton.Location = new Point(75, 350);
      this.LocalServerButton.Name = "LocalServerButton";
      this.LocalServerButton.Size = new Size(850, 57);
      this.LocalServerButton.TabIndex = 2;
      this.LocalServerButton.Text = "Local Server";
      this.LocalServerButton.UseVisualStyleBackColor = false;
      this.LocalServerButton.Click += new EventHandler(this.button2_Click);
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(0, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1000, 36);
      this.label1.TabIndex = 3;
      this.label1.Text = "SELECT MODE TYPE";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonGalacticTvMode.BackColor = Color.DodgerBlue;
      this.buttonGalacticTvMode.Cursor = Cursors.Hand;
      this.buttonGalacticTvMode.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonGalacticTvMode.ForeColor = Color.White;
      this.buttonGalacticTvMode.Location = new Point(75, 450);
      this.buttonGalacticTvMode.Name = "buttonGalacticTvMode";
      this.buttonGalacticTvMode.Size = new Size(850, 57);
      this.buttonGalacticTvMode.TabIndex = 4;
      this.buttonGalacticTvMode.Text = "Video Upload";
      this.buttonGalacticTvMode.UseVisualStyleBackColor = false;
      this.buttonGalacticTvMode.Click += new EventHandler(this.buttonGalacticTvMode_Click);
      this.buttonBarcodePrintMode.BackColor = Color.DodgerBlue;
      this.buttonBarcodePrintMode.Cursor = Cursors.Hand;
      this.buttonBarcodePrintMode.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarcodePrintMode.ForeColor = Color.White;
      this.buttonBarcodePrintMode.Location = new Point(75, 550);
      this.buttonBarcodePrintMode.Name = "buttonBarcodePrintMode";
      this.buttonBarcodePrintMode.Size = new Size(850, 57);
      this.buttonBarcodePrintMode.TabIndex = 5;
      this.buttonBarcodePrintMode.Text = "Barcode && Print";
      this.buttonBarcodePrintMode.UseVisualStyleBackColor = false;
      this.buttonBarcodePrintMode.Click += new EventHandler(this.buttonBarcodePrintMode_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Desktop;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.buttonBarcodePrintMode);
      this.Controls.Add((Control) this.buttonGalacticTvMode);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.LocalServerButton);
      this.Controls.Add((Control) this.PhotoSaleButton);
      this.Controls.Add((Control) this.photoTakenPlace);
      this.Margin = new Padding(0);
      this.Name = nameof (SelectModeType);
      this.Size = new Size(1024, 656);
      this.ResumeLayout(false);
    }
  }
}
