using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class photoSalesSettingcs : UserControl
  {
    private PageRouter router;
    private Datas configs;
    private IContainer components;
    private ComboBox comboBox2;
    private Label label1;
    private Label SettingPhotoTaken;
    private Button button1;
    private BackgroundWorker ServiceCaller;

    public photoSalesSettingcs() => this.InitializeComponent();

    public void isLoaded(PageRouter _router, Datas _data)
    {
      Console.WriteLine("isLoaded Cagrildi");
      this.router = _router;
      this.configs = _data;
      Animation.AnimationAdd((UserControl) this);
      if (this.ServiceCaller.IsBusy)
        return;
      this.ServiceCaller.RunWorkerAsync();
    }

    private void ServiceCaller_DoWork(object sender, DoWorkEventArgs e)
    {
      RestClient restClient = new RestClient(Animation.Url + "locations", HttpVerb.GET, "{'someValueToPost': 'The Value being Posted'}");
      e.Result = (object) restClient.MakeRequest();
    }

    private void ServiceCaller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      if (e.Error != null || e.Cancelled)
        return;
      Locations locations = new JavaScriptSerializer().Deserialize<Locations>((string) e.Result);
      if (locations == null || !locations.status.Equals("SUCCESS"))
        return;
      this.comboBox2.DataSource = (object) locations.items;
      this.comboBox2.DisplayMember = "title";
      this.comboBox2.ValueMember = "id";
      for (int index = 0; index < this.comboBox2.Items.Count; ++index)
      {
        if (this.configs.Location == ((Item) this.comboBox2.Items[index]).id)
        {
          this.comboBox2.SelectedIndex = index;
          break;
        }
      }
    }

    private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
    {
      Item selectedItem = (Item) this.comboBox2.SelectedItem;
      if (selectedItem == null)
        return;
      Console.WriteLine(selectedItem.id + ":" + selectedItem.title);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Item selectedItem = (Item) this.comboBox2.SelectedItem;
      List<Datas> objectToWrite = new FileInfo(ReadWrite.dbPath).Exists ? ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath) : new List<Datas>();
      Datas data = objectToWrite.Count > 0 ? objectToWrite[0] : new Datas();
      if (selectedItem != null)
      {
        if (objectToWrite != null)
        {
          data.Location = selectedItem.id;
          objectToWrite.Clear();
          objectToWrite.Add(data);
          ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, objectToWrite);
        }
        else
        {
          data.Location = selectedItem.id;
          objectToWrite.Clear();
          ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, new List<Datas>()
          {
            data
          });
        }
        this.router.goPhotoSaleHomePage(this.router, data);
      }
      else
      {
        int num = (int) MessageBox.Show("Please Select Location");
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.comboBox2 = new ComboBox();
      this.label1 = new Label();
      this.SettingPhotoTaken = new Label();
      this.button1 = new Button();
      this.ServiceCaller = new BackgroundWorker();
      this.SuspendLayout();
      this.comboBox2.Cursor = Cursors.Hand;
      this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new Point(239, 212);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new Size(686, 32);
      this.comboBox2.TabIndex = 6;
      this.comboBox2.SelectedValueChanged += new EventHandler(this.comboBox2_SelectedValueChanged);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(73, 219);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 25);
      this.label1.TabIndex = 7;
      this.label1.Text = "Select Location";
      this.SettingPhotoTaken.BackColor = Color.Transparent;
      this.SettingPhotoTaken.Font = new Font("Century Gothic", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.SettingPhotoTaken.ForeColor = SystemColors.ButtonFace;
      this.SettingPhotoTaken.Location = new Point(0, 28);
      this.SettingPhotoTaken.Name = "SettingPhotoTaken";
      this.SettingPhotoTaken.Size = new Size(1000, 36);
      this.SettingPhotoTaken.TabIndex = 8;
      this.SettingPhotoTaken.Text = "PHOTO SALE SETTINGS";
      this.SettingPhotoTaken.TextAlign = ContentAlignment.MiddleCenter;
      this.button1.BackColor = Color.RoyalBlue;
      this.button1.Cursor = Cursors.Hand;
      this.button1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.button1.ForeColor = Color.White;
      this.button1.Location = new Point(725, 503);
      this.button1.Name = "button1";
      this.button1.Size = new Size(200, 57);
      this.button1.TabIndex = 9;
      this.button1.Text = "Next";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.ServiceCaller.WorkerSupportsCancellation = true;
      this.ServiceCaller.DoWork += new DoWorkEventHandler(this.ServiceCaller_DoWork);
      this.ServiceCaller.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ServiceCaller_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Desktop;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.SettingPhotoTaken);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.comboBox2);
      this.Margin = new Padding(0);
      this.Name = nameof (photoSalesSettingcs);
      this.Size = new Size(1024, 656);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
