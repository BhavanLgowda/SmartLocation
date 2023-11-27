using Newtonsoft.Json;
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

namespace SmartLocationApp.Pages.Setting
{
  public class BarcodePrintSettings : UserControl
  {
    private static int COUNT = 6;
    public List<OutputFoldersStructure>[] opFolders;
    private Button[] BarCodePressDirectory_BTNS;
    private Button[] PPrintSucces_BTNS;
    private Button[] PPrintFailed_BTNS;
    private CheckBox[] BarcodePressActive_CBOXS;
    private CheckBox[] Horizontal_CB;
    private CheckBox[] Prefix_CB;
    private TextBox[] BarcodeW_TBOXS;
    private TextBox[] BarcodeH_TBOXS;
    private TextBox[] BarcodeX_TBOXS;
    private TextBox[] BarcodeY_TBOXS;
    private bool[] BarCodePressActives;
    private bool[] Horizontal;
    private bool[] Prefix;
    private string[] BarCodePress_Directories;
    private string[] PrePrintSuccess_Directories;
    private string[] PrePrintFailed_Directories;
    private string[] OutputFoldersJson_Directoreis;
    private int[] BarcodeWs;
    private int[] BarcodeHs;
    private int[] BarcodeXs;
    private int[] BarcodeYs;
    private PageRouter router;
    private Datas configs;
    private IContainer components;
    private Label label6;
    private ComboBox comboBoxLocation;
    private Label SettingPhotoTaken;
    private Button buttonBarcodePrintSave;
    private Label label2;
    private BackgroundWorker servisCallLocations1;
    private TabControl tabControlBarcodeSettings;
    private TabPage tabSize1;
    private Button buttonOutputFolders1;
    private Label label10;
    private Label label4;
    private Label label9;
    private Label label3;
    private TextBox barcodeH1;
    private TextBox barcodeW1;
    private TextBox barcodeY1;
    private TextBox barcodeX1;
    private Label label7;
    private Label label1;
    private Label label5;
    private Label label8;
    private Button butonPPrintFailed1;
    private Button butonPPrintSucces1;
    private Button buttonBarCodePressDirectory1;
    private TabPage tabSize2;
    private TabPage tabSize3;
    private TabPage tabSize4;
    private CheckBox checkBoxBarcodePressActive1;
    private Label label11;
    private CheckBox checkBoxBarcodePressActive2;
    private Label label12;
    private Button buttonOutputFolders2;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label16;
    private TextBox barcodeH2;
    private TextBox barcodeW2;
    private TextBox barcodeY2;
    private TextBox barcodeX2;
    private Label label17;
    private Label label18;
    private Label label19;
    private Label label20;
    private Button butonPPrintFailed2;
    private Button butonPPrintSucces2;
    private Button buttonBarCodePressDirectory2;
    private CheckBox checkBoxBarcodePressActive3;
    private Label label21;
    private Button buttonOutputFolders3;
    private Label label22;
    private Label label23;
    private Label label24;
    private Label label25;
    private TextBox barcodeH3;
    private TextBox barcodeW3;
    private TextBox barcodeY3;
    private TextBox barcodeX3;
    private Label label26;
    private Label label27;
    private Label label28;
    private Label label29;
    private Button butonPPrintFailed3;
    private Button butonPPrintSucces3;
    private Button buttonBarCodePressDirectory3;
    private CheckBox checkBoxBarcodePressActive4;
    private Label label30;
    private Button buttonOutputFolders4;
    private Label label31;
    private Label label32;
    private Label label33;
    private Label label34;
    private TextBox barcodeH4;
    private TextBox barcodeW4;
    private TextBox barcodeY4;
    private TextBox barcodeX4;
    private Label label35;
    private Label label36;
    private Label label37;
    private Label label38;
    private Button butonPPrintFailed4;
    private Button butonPPrintSucces4;
    private Button buttonBarCodePressDirectory4;
    private TabPage tabSize5;
    private CheckBox checkBoxBarcodePressActive5;
    private Label label39;
    private Button buttonOutputFolders5;
    private Label label40;
    private Label label41;
    private Label label42;
    private Label label43;
    private TextBox barcodeH5;
    private TextBox barcodeW5;
    private TextBox barcodeY5;
    private TextBox barcodeX5;
    private Label label44;
    private Label label45;
    private Label label46;
    private Label label47;
    private Button butonPPrintFailed5;
    private Button butonPPrintSucces5;
    private Button buttonBarCodePressDirectory5;
    private TabPage tabSize6;
    private CheckBox checkBoxBarcodePressActive6;
    private Label label48;
    private Button buttonOutputFolders6;
    private Label label49;
    private Label label50;
    private Label label51;
    private Label label52;
    private TextBox barcodeH6;
    private TextBox barcodeW6;
    private TextBox barcodeY6;
    private TextBox barcodeX6;
    private Label label53;
    private Label label54;
    private Label label55;
    private Label label56;
    private Button butonPPrintFailed6;
    private Button butonPPrintSucces6;
    private Button buttonBarCodePressDirectory6;
    private CheckBox cb_horizontal1;
    private CheckBox cb_horizontal2;
    private CheckBox cb_horizontal3;
    private CheckBox cb_horizontal4;
    private CheckBox cb_horizontal5;
    private CheckBox cb_horizontal6;
    private CheckBox checkBoxPrefix1;
    private CheckBox checkBoxPrefix2;
    private CheckBox checkBoxPrefix3;
    private CheckBox checkBoxPrefix4;
    private CheckBox checkBoxPrefix5;
    private CheckBox checkBoxPrefix6;

    public BarcodePrintSettings()
    {
      this.InitializeComponent();
      this.BarCodePressDirectory_BTNS = new Button[6]
      {
        this.buttonBarCodePressDirectory1,
        this.buttonBarCodePressDirectory2,
        this.buttonBarCodePressDirectory3,
        this.buttonBarCodePressDirectory4,
        this.buttonBarCodePressDirectory5,
        this.buttonBarCodePressDirectory6
      };
      this.PPrintSucces_BTNS = new Button[6]
      {
        this.butonPPrintSucces1,
        this.butonPPrintSucces2,
        this.butonPPrintSucces3,
        this.butonPPrintSucces4,
        this.butonPPrintSucces5,
        this.butonPPrintSucces6
      };
      this.PPrintFailed_BTNS = new Button[6]
      {
        this.butonPPrintFailed1,
        this.butonPPrintFailed2,
        this.butonPPrintFailed3,
        this.butonPPrintFailed4,
        this.butonPPrintFailed5,
        this.butonPPrintFailed6
      };
      this.BarcodePressActive_CBOXS = new CheckBox[6]
      {
        this.checkBoxBarcodePressActive1,
        this.checkBoxBarcodePressActive2,
        this.checkBoxBarcodePressActive3,
        this.checkBoxBarcodePressActive4,
        this.checkBoxBarcodePressActive5,
        this.checkBoxBarcodePressActive6
      };
      this.BarcodeW_TBOXS = new TextBox[6]
      {
        this.barcodeW1,
        this.barcodeW2,
        this.barcodeW3,
        this.barcodeW4,
        this.barcodeW5,
        this.barcodeW6
      };
      this.BarcodeH_TBOXS = new TextBox[6]
      {
        this.barcodeH1,
        this.barcodeH2,
        this.barcodeH3,
        this.barcodeH4,
        this.barcodeH5,
        this.barcodeH6
      };
      this.BarcodeX_TBOXS = new TextBox[6]
      {
        this.barcodeX1,
        this.barcodeX2,
        this.barcodeX3,
        this.barcodeX4,
        this.barcodeX5,
        this.barcodeX6
      };
      this.BarcodeY_TBOXS = new TextBox[6]
      {
        this.barcodeY1,
        this.barcodeY2,
        this.barcodeY3,
        this.barcodeY4,
        this.barcodeY5,
        this.barcodeY6
      };
      this.Horizontal_CB = new CheckBox[6]
      {
        this.cb_horizontal1,
        this.cb_horizontal2,
        this.cb_horizontal3,
        this.cb_horizontal4,
        this.cb_horizontal5,
        this.cb_horizontal6
      };
      this.Prefix_CB = new CheckBox[6]
      {
        this.checkBoxPrefix1,
        this.checkBoxPrefix2,
        this.checkBoxPrefix3,
        this.checkBoxPrefix4,
        this.checkBoxPrefix5,
        this.checkBoxPrefix6
      };
    }

    public BarcodePrintSettings(PageRouter _router)
    {
      Control.CheckForIllegalCrossThreadCalls = false;
      this.InitializeComponent();
      this.router = _router;
    }

    public void init(PageRouter _router, Datas data)
    {
      Console.WriteLine("BarcodePrintSettings.init");
      this.router = _router;
      this.configs = data;
      Animation.AnimationAdd((UserControl) this);
      this.servisCallLocations1.RunWorkerAsync();
      this.opFolders = new List<OutputFoldersStructure>[BarcodePrintSettings.COUNT];
      this.BarCodePressActives = new bool[6]
      {
        data.BarCodePressActive1,
        data.BarCodePressActive2,
        data.BarCodePressActive3,
        data.BarCodePressActive4,
        data.BarCodePressActive5,
        data.BarCodePressActive6
      };
      this.BarCodePress_Directories = new string[6]
      {
        data.BarCodePress_Directory1,
        data.BarCodePress_Directory2,
        data.BarCodePress_Directory3,
        data.BarCodePress_Directory4,
        data.BarCodePress_Directory5,
        data.BarCodePress_Directory6
      };
      this.PrePrintSuccess_Directories = new string[6]
      {
        data.PrePrintSuccess_Directory1,
        data.PrePrintSuccess_Directory2,
        data.PrePrintSuccess_Directory3,
        data.PrePrintSuccess_Directory4,
        data.PrePrintSuccess_Directory5,
        data.PrePrintSuccess_Directory6
      };
      this.PrePrintFailed_Directories = new string[6]
      {
        data.PrePrintFailed_Directory1,
        data.PrePrintFailed_Directory2,
        data.PrePrintFailed_Directory3,
        data.PrePrintFailed_Directory4,
        data.PrePrintFailed_Directory5,
        data.PrePrintFailed_Directory6
      };
      this.OutputFoldersJson_Directoreis = new string[6]
      {
        data.OutputFoldersJson1,
        data.OutputFoldersJson2,
        data.OutputFoldersJson3,
        data.OutputFoldersJson4,
        data.OutputFoldersJson5,
        data.OutputFoldersJson6
      };
      this.BarcodeWs = new int[6]
      {
        data.barcodeW1,
        data.barcodeW2,
        data.barcodeW3,
        data.barcodeW4,
        data.barcodeW5,
        data.barcodeW6
      };
      this.BarcodeHs = new int[6]
      {
        data.barcodeH1,
        data.barcodeH2,
        data.barcodeH3,
        data.barcodeH4,
        data.barcodeH5,
        data.barcodeH6
      };
      this.BarcodeXs = new int[6]
      {
        data.barcodeX1,
        data.barcodeX2,
        data.barcodeX3,
        data.barcodeX4,
        data.barcodeX5,
        data.barcodeX6
      };
      this.BarcodeYs = new int[6]
      {
        data.barcodeY1,
        data.barcodeY2,
        data.barcodeY3,
        data.barcodeY4,
        data.barcodeY5,
        data.barcodeY6
      };
      this.Horizontal = new bool[6]
      {
        data.horizontal1,
        data.horizontal2,
        data.horizontal3,
        data.horizontal4,
        data.horizontal5,
        data.horizontal6
      };
      this.Prefix = new bool[6]
      {
        data.prefix1,
        data.prefix2,
        data.prefix3,
        data.prefix4,
        data.prefix5,
        data.prefix6
      };
      JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
      for (int index = 0; index < BarcodePrintSettings.COUNT; ++index)
      {
        this.BarcodePressActive_CBOXS[index].Checked = this.BarCodePressActives[index];
        this.Horizontal_CB[index].Checked = this.Horizontal[index];
        this.Prefix_CB[index].Checked = this.Prefix[index];
        if (this.BarCodePress_Directories[index] != null)
          this.BarCodePressDirectory_BTNS[index].Text = this.BarCodePress_Directories[index];
        if (this.PrePrintSuccess_Directories[index] != null)
          this.PPrintSucces_BTNS[index].Text = this.PrePrintSuccess_Directories[index];
        if (this.PrePrintFailed_Directories[index] != null)
          this.PPrintFailed_BTNS[index].Text = this.PrePrintFailed_Directories[index];
        this.BarcodeX_TBOXS[index].Text = this.BarcodeXs[index].ToString();
        this.BarcodeY_TBOXS[index].Text = this.BarcodeYs[index].ToString();
        this.BarcodeW_TBOXS[index].Text = this.BarcodeWs[index].ToString();
        this.BarcodeH_TBOXS[index].Text = this.BarcodeHs[index].ToString();
        this.opFolders[index] = scriptSerializer.Deserialize<List<OutputFoldersStructure>>(this.OutputFoldersJson_Directoreis[index]);
      }
    }

    private void servisCallLocations1_DoWork(object sender, DoWorkEventArgs e)
    {
      RestClient restClient = new RestClient(Animation.Url + "locations", HttpVerb.GET, "{'someValueToPost': 'The Value being Posted'}");
      e.Result = (object) restClient.MakeRequest();
    }

    private void servisCallLocations1_RunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      Animation.AnimationRemove((UserControl) this);
      Locations locations = new JavaScriptSerializer().Deserialize<Locations>((string) e.Result);
      if (locations == null || !locations.status.Equals("SUCCESS"))
        return;
      this.comboBoxLocation.DataSource = (object) locations.items;
      this.comboBoxLocation.DisplayMember = "title";
      this.comboBoxLocation.ValueMember = "id";
      for (int index = 0; index < this.comboBoxLocation.Items.Count; ++index)
      {
        if (this.configs.Location == ((Item) this.comboBoxLocation.Items[index]).id)
        {
          this.comboBoxLocation.SelectedIndex = index;
          break;
        }
      }
    }

    private void buttonBarCodePressDirectory_Click(object sender, EventArgs e)
    {
      int index = Convert.ToInt32(((Control) sender).Tag) - 1;
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.BarCodePressDirectory_BTNS[index].Text = folderBrowserDialog.SelectedPath;
    }

    private void butonPPrintSuccess_Click(object sender, EventArgs e)
    {
      int index = Convert.ToInt32(((Control) sender).Tag) - 1;
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.PPrintSucces_BTNS[index].Text = folderBrowserDialog.SelectedPath;
    }

    private void butonPPrintFailed_Click(object sender, EventArgs e)
    {
      int index = Convert.ToInt32(((Control) sender).Tag) - 1;
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.PPrintFailed_BTNS[index].Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonBarcodePrintSave_Click(object sender, EventArgs e)
    {
      try
      {
        List<Datas> objectToWrite = new FileInfo(ReadWrite.dbPath).Exists ? ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath) : new List<Datas>();
        Datas data = objectToWrite.Count > 0 ? objectToWrite[0] : new Datas();
        for (int index = 0; index < 4; ++index)
        {
          if (this.BarcodePressActive_CBOXS[index].Checked && (this.BarCodePressDirectory_BTNS[index].Text.Length < 2 || this.PPrintSucces_BTNS[index].Text.Length < 2 || this.PPrintFailed_BTNS[index].Text.Length < 2))
          {
            int num = (int) MessageBox.Show("The desitation folder you selected is invalid.");
            return;
          }
        }
        Item selectedItem = (Item) this.comboBoxLocation.SelectedItem;
        if (selectedItem == null)
        {
          int num1 = (int) MessageBox.Show("Please Select Location");
        }
        else
        {
          data.Location = selectedItem.id;
          data.BarCodePressActive1 = this.checkBoxBarcodePressActive1.Checked;
          data.BarCodePress_Directory1 = this.buttonBarCodePressDirectory1.Text;
          data.PrePrintSuccess_Directory1 = this.butonPPrintSucces1.Text;
          data.PrePrintFailed_Directory1 = this.butonPPrintFailed1.Text;
          data.barcodeX1 = int.Parse(this.barcodeX1.Text);
          data.barcodeY1 = int.Parse(this.barcodeY1.Text);
          data.barcodeW1 = int.Parse(this.barcodeW1.Text);
          data.barcodeH1 = int.Parse(this.barcodeH1.Text);
          data.horizontal1 = this.cb_horizontal1.Checked;
          data.prefix1 = this.checkBoxPrefix1.Checked;
          data.OutputFoldersJson1 = JsonConvert.SerializeObject((object) this.opFolders[0]);
          data.BarCodePressActive2 = this.checkBoxBarcodePressActive2.Checked;
          data.BarCodePress_Directory2 = this.buttonBarCodePressDirectory2.Text;
          data.PrePrintSuccess_Directory2 = this.butonPPrintSucces2.Text;
          data.PrePrintFailed_Directory2 = this.butonPPrintFailed2.Text;
          data.barcodeX2 = int.Parse(this.barcodeX2.Text);
          data.barcodeY2 = int.Parse(this.barcodeY2.Text);
          data.barcodeW2 = int.Parse(this.barcodeW2.Text);
          data.barcodeH2 = int.Parse(this.barcodeH2.Text);
          data.horizontal2 = this.cb_horizontal2.Checked;
          data.prefix2 = this.checkBoxPrefix2.Checked;
          data.OutputFoldersJson2 = JsonConvert.SerializeObject((object) this.opFolders[1]);
          data.BarCodePressActive3 = this.checkBoxBarcodePressActive3.Checked;
          data.BarCodePress_Directory3 = this.buttonBarCodePressDirectory3.Text;
          data.PrePrintSuccess_Directory3 = this.butonPPrintSucces3.Text;
          data.PrePrintFailed_Directory3 = this.butonPPrintFailed3.Text;
          data.barcodeX3 = int.Parse(this.barcodeX3.Text);
          data.barcodeY3 = int.Parse(this.barcodeY3.Text);
          data.barcodeW3 = int.Parse(this.barcodeW3.Text);
          data.barcodeH3 = int.Parse(this.barcodeH3.Text);
          data.horizontal3 = this.cb_horizontal3.Checked;
          data.prefix3 = this.checkBoxPrefix3.Checked;
          data.OutputFoldersJson3 = JsonConvert.SerializeObject((object) this.opFolders[2]);
          data.BarCodePressActive4 = this.checkBoxBarcodePressActive4.Checked;
          data.BarCodePress_Directory4 = this.buttonBarCodePressDirectory4.Text;
          data.PrePrintSuccess_Directory4 = this.butonPPrintSucces4.Text;
          data.PrePrintFailed_Directory4 = this.butonPPrintFailed4.Text;
          data.barcodeX4 = int.Parse(this.barcodeX4.Text);
          data.barcodeY4 = int.Parse(this.barcodeY4.Text);
          data.barcodeW4 = int.Parse(this.barcodeW4.Text);
          data.barcodeH4 = int.Parse(this.barcodeH4.Text);
          data.horizontal4 = this.cb_horizontal4.Checked;
          data.prefix4 = this.checkBoxPrefix4.Checked;
          data.OutputFoldersJson4 = JsonConvert.SerializeObject((object) this.opFolders[3]);
          data.BarCodePressActive5 = this.checkBoxBarcodePressActive5.Checked;
          data.BarCodePress_Directory5 = this.buttonBarCodePressDirectory5.Text;
          data.PrePrintSuccess_Directory5 = this.butonPPrintSucces5.Text;
          data.PrePrintFailed_Directory5 = this.butonPPrintFailed5.Text;
          data.barcodeX5 = int.Parse(this.barcodeX5.Text);
          data.barcodeY5 = int.Parse(this.barcodeY5.Text);
          data.barcodeW5 = int.Parse(this.barcodeW5.Text);
          data.barcodeH5 = int.Parse(this.barcodeH5.Text);
          data.horizontal5 = this.cb_horizontal5.Checked;
          data.prefix5 = this.checkBoxPrefix5.Checked;
          data.OutputFoldersJson5 = JsonConvert.SerializeObject((object) this.opFolders[4]);
          data.BarCodePressActive6 = this.checkBoxBarcodePressActive6.Checked;
          data.BarCodePress_Directory6 = this.buttonBarCodePressDirectory6.Text;
          data.PrePrintSuccess_Directory6 = this.butonPPrintSucces6.Text;
          data.PrePrintFailed_Directory6 = this.butonPPrintFailed6.Text;
          data.barcodeX6 = int.Parse(this.barcodeX6.Text);
          data.barcodeY6 = int.Parse(this.barcodeY6.Text);
          data.barcodeW6 = int.Parse(this.barcodeW6.Text);
          data.barcodeH6 = int.Parse(this.barcodeH6.Text);
          data.horizontal6 = this.cb_horizontal6.Checked;
          data.prefix6 = this.checkBoxPrefix6.Checked;
          data.OutputFoldersJson6 = JsonConvert.SerializeObject((object) this.opFolders[5]);
          objectToWrite.Clear();
          objectToWrite.Add(data);
          ReadWrite.WriteToXmlFile<List<Datas>>(ReadWrite.dbPath, objectToWrite);
          this.router.goBarcodePrint(this.router, data);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void buttonOutputFolders_Click(object sender, EventArgs e)
    {
      int _index = Convert.ToInt32(((Control) sender).Tag) - 1;
      outputFolders outputFolders = new outputFolders();
      outputFolders.init(this, _index);
      if (this.opFolders[_index] == null)
      {
        this.opFolders[_index] = new List<OutputFoldersStructure>();
        for (int index = 0; index < 10; ++index)
          this.opFolders[_index].Add(new OutputFoldersStructure()
          {
            outputFolder = (string) null,
            prefixValue = (string) null,
            cycle = false,
            amount = 0,
            active = false,
            queue = false,
            loop = 0
          });
      }
      outputFolders.fillOutputFolders(this.opFolders[_index], this.Prefix_CB[_index].Checked);
      int num = (int) outputFolders.ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label6 = new Label();
      this.comboBoxLocation = new ComboBox();
      this.SettingPhotoTaken = new Label();
      this.buttonBarcodePrintSave = new Button();
      this.label2 = new Label();
      this.servisCallLocations1 = new BackgroundWorker();
      this.tabControlBarcodeSettings = new TabControl();
      this.tabSize1 = new TabPage();
      this.checkBoxPrefix1 = new CheckBox();
      this.cb_horizontal1 = new CheckBox();
      this.checkBoxBarcodePressActive1 = new CheckBox();
      this.label11 = new Label();
      this.buttonOutputFolders1 = new Button();
      this.label10 = new Label();
      this.label4 = new Label();
      this.label9 = new Label();
      this.label3 = new Label();
      this.barcodeH1 = new TextBox();
      this.barcodeW1 = new TextBox();
      this.barcodeY1 = new TextBox();
      this.barcodeX1 = new TextBox();
      this.label7 = new Label();
      this.label1 = new Label();
      this.label5 = new Label();
      this.label8 = new Label();
      this.butonPPrintFailed1 = new Button();
      this.butonPPrintSucces1 = new Button();
      this.buttonBarCodePressDirectory1 = new Button();
      this.tabSize2 = new TabPage();
      this.checkBoxPrefix2 = new CheckBox();
      this.cb_horizontal2 = new CheckBox();
      this.checkBoxBarcodePressActive2 = new CheckBox();
      this.label12 = new Label();
      this.buttonOutputFolders2 = new Button();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.barcodeH2 = new TextBox();
      this.barcodeW2 = new TextBox();
      this.barcodeY2 = new TextBox();
      this.barcodeX2 = new TextBox();
      this.label17 = new Label();
      this.label18 = new Label();
      this.label19 = new Label();
      this.label20 = new Label();
      this.butonPPrintFailed2 = new Button();
      this.butonPPrintSucces2 = new Button();
      this.buttonBarCodePressDirectory2 = new Button();
      this.tabSize3 = new TabPage();
      this.checkBoxPrefix3 = new CheckBox();
      this.cb_horizontal3 = new CheckBox();
      this.checkBoxBarcodePressActive3 = new CheckBox();
      this.label21 = new Label();
      this.buttonOutputFolders3 = new Button();
      this.label22 = new Label();
      this.label23 = new Label();
      this.label24 = new Label();
      this.label25 = new Label();
      this.barcodeH3 = new TextBox();
      this.barcodeW3 = new TextBox();
      this.barcodeY3 = new TextBox();
      this.barcodeX3 = new TextBox();
      this.label26 = new Label();
      this.label27 = new Label();
      this.label28 = new Label();
      this.label29 = new Label();
      this.butonPPrintFailed3 = new Button();
      this.butonPPrintSucces3 = new Button();
      this.buttonBarCodePressDirectory3 = new Button();
      this.tabSize4 = new TabPage();
      this.checkBoxPrefix4 = new CheckBox();
      this.cb_horizontal4 = new CheckBox();
      this.checkBoxBarcodePressActive4 = new CheckBox();
      this.label30 = new Label();
      this.buttonOutputFolders4 = new Button();
      this.label31 = new Label();
      this.label32 = new Label();
      this.label33 = new Label();
      this.label34 = new Label();
      this.barcodeH4 = new TextBox();
      this.barcodeW4 = new TextBox();
      this.barcodeY4 = new TextBox();
      this.barcodeX4 = new TextBox();
      this.label35 = new Label();
      this.label36 = new Label();
      this.label37 = new Label();
      this.label38 = new Label();
      this.butonPPrintFailed4 = new Button();
      this.butonPPrintSucces4 = new Button();
      this.buttonBarCodePressDirectory4 = new Button();
      this.tabSize5 = new TabPage();
      this.checkBoxPrefix5 = new CheckBox();
      this.cb_horizontal5 = new CheckBox();
      this.checkBoxBarcodePressActive5 = new CheckBox();
      this.label39 = new Label();
      this.buttonOutputFolders5 = new Button();
      this.label40 = new Label();
      this.label41 = new Label();
      this.label42 = new Label();
      this.label43 = new Label();
      this.barcodeH5 = new TextBox();
      this.barcodeW5 = new TextBox();
      this.barcodeY5 = new TextBox();
      this.barcodeX5 = new TextBox();
      this.label44 = new Label();
      this.label45 = new Label();
      this.label46 = new Label();
      this.label47 = new Label();
      this.butonPPrintFailed5 = new Button();
      this.butonPPrintSucces5 = new Button();
      this.buttonBarCodePressDirectory5 = new Button();
      this.tabSize6 = new TabPage();
      this.checkBoxPrefix6 = new CheckBox();
      this.cb_horizontal6 = new CheckBox();
      this.checkBoxBarcodePressActive6 = new CheckBox();
      this.label48 = new Label();
      this.buttonOutputFolders6 = new Button();
      this.label49 = new Label();
      this.label50 = new Label();
      this.label51 = new Label();
      this.label52 = new Label();
      this.barcodeH6 = new TextBox();
      this.barcodeW6 = new TextBox();
      this.barcodeY6 = new TextBox();
      this.barcodeX6 = new TextBox();
      this.label53 = new Label();
      this.label54 = new Label();
      this.label55 = new Label();
      this.label56 = new Label();
      this.butonPPrintFailed6 = new Button();
      this.butonPPrintSucces6 = new Button();
      this.buttonBarCodePressDirectory6 = new Button();
      this.tabControlBarcodeSettings.SuspendLayout();
      this.tabSize1.SuspendLayout();
      this.tabSize2.SuspendLayout();
      this.tabSize3.SuspendLayout();
      this.tabSize4.SuspendLayout();
      this.tabSize5.SuspendLayout();
      this.tabSize6.SuspendLayout();
      this.SuspendLayout();
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label6.ForeColor = SystemColors.ButtonFace;
      this.label6.Location = new Point(113, 78);
      this.label6.Name = "label6";
      this.label6.Size = new Size(160, 25);
      this.label6.TabIndex = 39;
      this.label6.Text = "Select Location";
      this.comboBoxLocation.Cursor = Cursors.Hand;
      this.comboBoxLocation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxLocation.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.comboBoxLocation.FormattingEnabled = true;
      this.comboBoxLocation.Location = new Point(276, 71);
      this.comboBoxLocation.Name = "comboBoxLocation";
      this.comboBoxLocation.Size = new Size(662, 32);
      this.comboBoxLocation.TabIndex = 38;
      this.SettingPhotoTaken.BackColor = Color.Transparent;
      this.SettingPhotoTaken.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.SettingPhotoTaken.ForeColor = SystemColors.ButtonFace;
      this.SettingPhotoTaken.Location = new Point(0, 16);
      this.SettingPhotoTaken.Name = "SettingPhotoTaken";
      this.SettingPhotoTaken.Size = new Size(1000, 36);
      this.SettingPhotoTaken.TabIndex = 31;
      this.SettingPhotoTaken.Text = "BARCODE && PRINT SETTINGS";
      this.SettingPhotoTaken.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonBarcodePrintSave.BackColor = Color.RoyalBlue;
      this.buttonBarcodePrintSave.Cursor = Cursors.Hand;
      this.buttonBarcodePrintSave.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarcodePrintSave.ForeColor = Color.White;
      this.buttonBarcodePrintSave.Location = new Point(738, 531);
      this.buttonBarcodePrintSave.Name = "buttonBarcodePrintSave";
      this.buttonBarcodePrintSave.Size = new Size(200, 57);
      this.buttonBarcodePrintSave.TabIndex = 29;
      this.buttonBarcodePrintSave.Text = "Next";
      this.buttonBarcodePrintSave.UseVisualStyleBackColor = false;
      this.buttonBarcodePrintSave.Click += new EventHandler(this.buttonBarcodePrintSave_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = Color.White;
      this.label2.Location = new Point(0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(154, 13);
      this.label2.TabIndex = 27;
      this.label2.Text = "Mode: Barcode && Print Settings";
      this.label2.Visible = false;
      this.servisCallLocations1.WorkerSupportsCancellation = true;
      this.servisCallLocations1.DoWork += new DoWorkEventHandler(this.servisCallLocations1_DoWork);
      this.servisCallLocations1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.servisCallLocations1_RunWorkerCompleted);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize1);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize2);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize3);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize4);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize5);
      this.tabControlBarcodeSettings.Controls.Add((Control) this.tabSize6);
      this.tabControlBarcodeSettings.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabControlBarcodeSettings.Location = new Point(19, 129);
      this.tabControlBarcodeSettings.Name = "tabControlBarcodeSettings";
      this.tabControlBarcodeSettings.SelectedIndex = 0;
      this.tabControlBarcodeSettings.Size = new Size(981, 386);
      this.tabControlBarcodeSettings.SizeMode = TabSizeMode.Fixed;
      this.tabControlBarcodeSettings.TabIndex = 51;
      this.tabSize1.BackColor = Color.White;
      this.tabSize1.BackgroundImage = (Image) Resources._6671;
      this.tabSize1.Controls.Add((Control) this.checkBoxPrefix1);
      this.tabSize1.Controls.Add((Control) this.cb_horizontal1);
      this.tabSize1.Controls.Add((Control) this.checkBoxBarcodePressActive1);
      this.tabSize1.Controls.Add((Control) this.label11);
      this.tabSize1.Controls.Add((Control) this.buttonOutputFolders1);
      this.tabSize1.Controls.Add((Control) this.label10);
      this.tabSize1.Controls.Add((Control) this.label4);
      this.tabSize1.Controls.Add((Control) this.label9);
      this.tabSize1.Controls.Add((Control) this.label3);
      this.tabSize1.Controls.Add((Control) this.barcodeH1);
      this.tabSize1.Controls.Add((Control) this.barcodeW1);
      this.tabSize1.Controls.Add((Control) this.barcodeY1);
      this.tabSize1.Controls.Add((Control) this.barcodeX1);
      this.tabSize1.Controls.Add((Control) this.label7);
      this.tabSize1.Controls.Add((Control) this.label1);
      this.tabSize1.Controls.Add((Control) this.label5);
      this.tabSize1.Controls.Add((Control) this.label8);
      this.tabSize1.Controls.Add((Control) this.butonPPrintFailed1);
      this.tabSize1.Controls.Add((Control) this.butonPPrintSucces1);
      this.tabSize1.Controls.Add((Control) this.buttonBarCodePressDirectory1);
      this.tabSize1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabSize1.Location = new Point(4, 29);
      this.tabSize1.Name = "tabSize1";
      this.tabSize1.Padding = new Padding(3);
      this.tabSize1.Size = new Size(973, 353);
      this.tabSize1.TabIndex = 0;
      this.tabSize1.Text = "Size 1";
      this.checkBoxPrefix1.AutoSize = true;
      this.checkBoxPrefix1.BackColor = Color.Transparent;
      this.checkBoxPrefix1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix1.ForeColor = SystemColors.Control;
      this.checkBoxPrefix1.Location = new Point(265, 313);
      this.checkBoxPrefix1.Name = "checkBoxPrefix1";
      this.checkBoxPrefix1.Size = new Size(73, 24);
      this.checkBoxPrefix1.TabIndex = 70;
      this.checkBoxPrefix1.Text = "Prefix";
      this.checkBoxPrefix1.UseVisualStyleBackColor = false;
      this.cb_horizontal1.AutoSize = true;
      this.cb_horizontal1.BackColor = Color.Transparent;
      this.cb_horizontal1.ForeColor = SystemColors.Control;
      this.cb_horizontal1.Location = new Point(447, 254);
      this.cb_horizontal1.Name = "cb_horizontal1";
      this.cb_horizontal1.Size = new Size(98, 17);
      this.cb_horizontal1.TabIndex = 69;
      this.cb_horizontal1.Text = "Rotate ▬ -> ▮";
      this.cb_horizontal1.UseMnemonic = false;
      this.cb_horizontal1.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive1.AutoSize = true;
      this.checkBoxBarcodePressActive1.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive1.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive1.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive1.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive1.Name = "checkBoxBarcodePressActive1";
      this.checkBoxBarcodePressActive1.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive1.TabIndex = 68;
      this.checkBoxBarcodePressActive1.Tag = (object) "1";
      this.checkBoxBarcodePressActive1.Text = "Yes";
      this.checkBoxBarcodePressActive1.UseVisualStyleBackColor = false;
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label11.ForeColor = SystemColors.ButtonFace;
      this.label11.Location = new Point(159, 23);
      this.label11.Name = "label11";
      this.label11.Size = new Size(91, 25);
      this.label11.TabIndex = 67;
      this.label11.Text = "Enabled";
      this.label11.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders1.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders1.Cursor = Cursors.Hand;
      this.buttonOutputFolders1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders1.ForeColor = Color.White;
      this.buttonOutputFolders1.Location = new Point(794, 118);
      this.buttonOutputFolders1.Name = "buttonOutputFolders1";
      this.buttonOutputFolders1.Size = new Size(118, 57);
      this.buttonOutputFolders1.TabIndex = 66;
      this.buttonOutputFolders1.Tag = (object) "1";
      this.buttonOutputFolders1.Text = "Output Folders";
      this.buttonOutputFolders1.UseVisualStyleBackColor = false;
      this.buttonOutputFolders1.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.ForeColor = Color.White;
      this.label10.Location = new Point(344, 281);
      this.label10.Name = "label10";
      this.label10.Size = new Size(41, 13);
      this.label10.TabIndex = 62;
      this.label10.Text = "Height:";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.ForeColor = Color.White;
      this.label4.Location = new Point(367, (int) byte.MaxValue);
      this.label4.Name = "label4";
      this.label4.Size = new Size(17, 13);
      this.label4.TabIndex = 63;
      this.label4.Text = "Y:";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.ForeColor = Color.White;
      this.label9.Location = new Point(254, 281);
      this.label9.Name = "label9";
      this.label9.Size = new Size(38, 13);
      this.label9.TabIndex = 64;
      this.label9.Text = "Width:";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.ForeColor = Color.White;
      this.label3.Location = new Point(274, (int) byte.MaxValue);
      this.label3.Name = "label3";
      this.label3.Size = new Size(17, 13);
      this.label3.TabIndex = 65;
      this.label3.Text = "X:";
      this.barcodeH1.Location = new Point(385, 278);
      this.barcodeH1.Name = "barcodeH1";
      this.barcodeH1.Size = new Size(46, 20);
      this.barcodeH1.TabIndex = 58;
      this.barcodeH1.Tag = (object) "1";
      this.barcodeH1.Text = "0";
      this.barcodeW1.Location = new Point(292, 278);
      this.barcodeW1.Name = "barcodeW1";
      this.barcodeW1.Size = new Size(46, 20);
      this.barcodeW1.TabIndex = 59;
      this.barcodeW1.Tag = (object) "1";
      this.barcodeW1.Text = "0";
      this.barcodeY1.Location = new Point(385, 252);
      this.barcodeY1.Name = "barcodeY1";
      this.barcodeY1.Size = new Size(46, 20);
      this.barcodeY1.TabIndex = 60;
      this.barcodeY1.Tag = (object) "1";
      this.barcodeY1.Text = "0";
      this.barcodeX1.Location = new Point(292, 252);
      this.barcodeX1.Name = "barcodeX1";
      this.barcodeX1.Size = new Size(46, 20);
      this.barcodeX1.TabIndex = 61;
      this.barcodeX1.Tag = (object) "1";
      this.barcodeX1.Text = "0";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label7.ForeColor = SystemColors.ButtonFace;
      this.label7.Location = new Point(74, 248);
      this.label7.Name = "label7";
      this.label7.Size = new Size(173, 25);
      this.label7.TabIndex = 54;
      this.label7.Text = "Bar Code Layout";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(86, 195);
      this.label1.Name = "label1";
      this.label1.Size = new Size(161, 25);
      this.label1.TabIndex = 55;
      this.label1.Text = "Pre-Print Failed";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label5.ForeColor = SystemColors.ButtonFace;
      this.label5.Location = new Point(65, 133);
      this.label5.Name = "label5";
      this.label5.Size = new Size(184, 25);
      this.label5.TabIndex = 56;
      this.label5.Text = "Pre-Print Success";
      this.label5.TextAlign = ContentAlignment.MiddleCenter;
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label8.ForeColor = SystemColors.ButtonFace;
      this.label8.Location = new Point(-1, 72);
      this.label8.Name = "label8";
      this.label8.Size = new Size(249, 25);
      this.label8.TabIndex = 57;
      this.label8.Text = "BarCode Press Directory";
      this.label8.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed1.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed1.Cursor = Cursors.Hand;
      this.butonPPrintFailed1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed1.ForeColor = Color.White;
      this.butonPPrintFailed1.Location = new Point(250, 180);
      this.butonPPrintFailed1.Name = "butonPPrintFailed1";
      this.butonPPrintFailed1.Size = new Size(662, 57);
      this.butonPPrintFailed1.TabIndex = 51;
      this.butonPPrintFailed1.Tag = (object) "1";
      this.butonPPrintFailed1.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed1.UseVisualStyleBackColor = false;
      this.butonPPrintFailed1.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces1.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces1.Cursor = Cursors.Hand;
      this.butonPPrintSucces1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces1.ForeColor = Color.White;
      this.butonPPrintSucces1.Location = new Point(250, 118);
      this.butonPPrintSucces1.Name = "butonPPrintSucces1";
      this.butonPPrintSucces1.Size = new Size(538, 57);
      this.butonPPrintSucces1.TabIndex = 52;
      this.butonPPrintSucces1.Tag = (object) "1";
      this.butonPPrintSucces1.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces1.UseVisualStyleBackColor = false;
      this.butonPPrintSucces1.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory1.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory1.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory1.ForeColor = Color.White;
      this.buttonBarCodePressDirectory1.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory1.Name = "buttonBarCodePressDirectory1";
      this.buttonBarCodePressDirectory1.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory1.TabIndex = 53;
      this.buttonBarCodePressDirectory1.Tag = (object) "1";
      this.buttonBarCodePressDirectory1.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory1.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory1.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.tabSize2.BackColor = Color.White;
      this.tabSize2.BackgroundImage = (Image) Resources._6671;
      this.tabSize2.Controls.Add((Control) this.checkBoxPrefix2);
      this.tabSize2.Controls.Add((Control) this.cb_horizontal2);
      this.tabSize2.Controls.Add((Control) this.checkBoxBarcodePressActive2);
      this.tabSize2.Controls.Add((Control) this.label12);
      this.tabSize2.Controls.Add((Control) this.buttonOutputFolders2);
      this.tabSize2.Controls.Add((Control) this.label13);
      this.tabSize2.Controls.Add((Control) this.label14);
      this.tabSize2.Controls.Add((Control) this.label15);
      this.tabSize2.Controls.Add((Control) this.label16);
      this.tabSize2.Controls.Add((Control) this.barcodeH2);
      this.tabSize2.Controls.Add((Control) this.barcodeW2);
      this.tabSize2.Controls.Add((Control) this.barcodeY2);
      this.tabSize2.Controls.Add((Control) this.barcodeX2);
      this.tabSize2.Controls.Add((Control) this.label17);
      this.tabSize2.Controls.Add((Control) this.label18);
      this.tabSize2.Controls.Add((Control) this.label19);
      this.tabSize2.Controls.Add((Control) this.label20);
      this.tabSize2.Controls.Add((Control) this.butonPPrintFailed2);
      this.tabSize2.Controls.Add((Control) this.butonPPrintSucces2);
      this.tabSize2.Controls.Add((Control) this.buttonBarCodePressDirectory2);
      this.tabSize2.Location = new Point(4, 29);
      this.tabSize2.Name = "tabSize2";
      this.tabSize2.Padding = new Padding(3);
      this.tabSize2.Size = new Size(973, 353);
      this.tabSize2.TabIndex = 1;
      this.tabSize2.Text = "Size 2";
      this.checkBoxPrefix2.AutoSize = true;
      this.checkBoxPrefix2.BackColor = Color.Transparent;
      this.checkBoxPrefix2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix2.ForeColor = SystemColors.Control;
      this.checkBoxPrefix2.Location = new Point(265, 313);
      this.checkBoxPrefix2.Name = "checkBoxPrefix2";
      this.checkBoxPrefix2.Size = new Size(73, 24);
      this.checkBoxPrefix2.TabIndex = 88;
      this.checkBoxPrefix2.Text = "Prefix";
      this.checkBoxPrefix2.UseVisualStyleBackColor = false;
      this.cb_horizontal2.AutoSize = true;
      this.cb_horizontal2.BackColor = Color.Transparent;
      this.cb_horizontal2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.cb_horizontal2.ForeColor = SystemColors.Control;
      this.cb_horizontal2.Location = new Point(447, 254);
      this.cb_horizontal2.Name = "cb_horizontal2";
      this.cb_horizontal2.Size = new Size(73, 17);
      this.cb_horizontal2.TabIndex = 87;
      this.cb_horizontal2.Text = "Horizontal";
      this.cb_horizontal2.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive2.AutoSize = true;
      this.checkBoxBarcodePressActive2.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive2.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive2.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive2.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive2.Name = "checkBoxBarcodePressActive2";
      this.checkBoxBarcodePressActive2.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive2.TabIndex = 86;
      this.checkBoxBarcodePressActive2.Tag = (object) "2";
      this.checkBoxBarcodePressActive2.Text = "Yes";
      this.checkBoxBarcodePressActive2.UseVisualStyleBackColor = false;
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label12.ForeColor = SystemColors.ButtonFace;
      this.label12.Location = new Point(159, 23);
      this.label12.Name = "label12";
      this.label12.Size = new Size(91, 25);
      this.label12.TabIndex = 85;
      this.label12.Text = "Enabled";
      this.label12.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders2.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders2.Cursor = Cursors.Hand;
      this.buttonOutputFolders2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders2.ForeColor = Color.White;
      this.buttonOutputFolders2.Location = new Point(794, 118);
      this.buttonOutputFolders2.Name = "buttonOutputFolders2";
      this.buttonOutputFolders2.Size = new Size(118, 57);
      this.buttonOutputFolders2.TabIndex = 84;
      this.buttonOutputFolders2.Tag = (object) "2";
      this.buttonOutputFolders2.Text = "Output Folders";
      this.buttonOutputFolders2.UseVisualStyleBackColor = false;
      this.buttonOutputFolders2.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label13.AutoSize = true;
      this.label13.BackColor = Color.Transparent;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label13.ForeColor = Color.White;
      this.label13.Location = new Point(344, 281);
      this.label13.Name = "label13";
      this.label13.Size = new Size(41, 13);
      this.label13.TabIndex = 80;
      this.label13.Text = "Height:";
      this.label14.AutoSize = true;
      this.label14.BackColor = Color.Transparent;
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label14.ForeColor = Color.White;
      this.label14.Location = new Point(367, (int) byte.MaxValue);
      this.label14.Name = "label14";
      this.label14.Size = new Size(17, 13);
      this.label14.TabIndex = 81;
      this.label14.Text = "Y:";
      this.label15.AutoSize = true;
      this.label15.BackColor = Color.Transparent;
      this.label15.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label15.ForeColor = Color.White;
      this.label15.Location = new Point(254, 281);
      this.label15.Name = "label15";
      this.label15.Size = new Size(38, 13);
      this.label15.TabIndex = 82;
      this.label15.Text = "Width:";
      this.label16.AutoSize = true;
      this.label16.BackColor = Color.Transparent;
      this.label16.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label16.ForeColor = Color.White;
      this.label16.Location = new Point(274, (int) byte.MaxValue);
      this.label16.Name = "label16";
      this.label16.Size = new Size(17, 13);
      this.label16.TabIndex = 83;
      this.label16.Text = "X:";
      this.barcodeH2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeH2.Location = new Point(385, 278);
      this.barcodeH2.Name = "barcodeH2";
      this.barcodeH2.Size = new Size(46, 20);
      this.barcodeH2.TabIndex = 76;
      this.barcodeH2.Tag = (object) "2";
      this.barcodeH2.Text = "0";
      this.barcodeW2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeW2.Location = new Point(292, 278);
      this.barcodeW2.Name = "barcodeW2";
      this.barcodeW2.Size = new Size(46, 20);
      this.barcodeW2.TabIndex = 77;
      this.barcodeW2.Tag = (object) "2";
      this.barcodeW2.Text = "0";
      this.barcodeY2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeY2.Location = new Point(385, 252);
      this.barcodeY2.Name = "barcodeY2";
      this.barcodeY2.Size = new Size(46, 20);
      this.barcodeY2.TabIndex = 78;
      this.barcodeY2.Tag = (object) "2";
      this.barcodeY2.Text = "0";
      this.barcodeX2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeX2.Location = new Point(292, 252);
      this.barcodeX2.Name = "barcodeX2";
      this.barcodeX2.Size = new Size(46, 20);
      this.barcodeX2.TabIndex = 79;
      this.barcodeX2.Tag = (object) "2";
      this.barcodeX2.Text = "0";
      this.label17.AutoSize = true;
      this.label17.BackColor = Color.Transparent;
      this.label17.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label17.ForeColor = SystemColors.ButtonFace;
      this.label17.Location = new Point(74, 248);
      this.label17.Name = "label17";
      this.label17.Size = new Size(173, 25);
      this.label17.TabIndex = 72;
      this.label17.Text = "Bar Code Layout";
      this.label17.TextAlign = ContentAlignment.MiddleCenter;
      this.label18.AutoSize = true;
      this.label18.BackColor = Color.Transparent;
      this.label18.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label18.ForeColor = SystemColors.ButtonFace;
      this.label18.Location = new Point(86, 195);
      this.label18.Name = "label18";
      this.label18.Size = new Size(161, 25);
      this.label18.TabIndex = 73;
      this.label18.Text = "Pre-Print Failed";
      this.label18.TextAlign = ContentAlignment.MiddleCenter;
      this.label19.AutoSize = true;
      this.label19.BackColor = Color.Transparent;
      this.label19.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label19.ForeColor = SystemColors.ButtonFace;
      this.label19.Location = new Point(65, 133);
      this.label19.Name = "label19";
      this.label19.Size = new Size(184, 25);
      this.label19.TabIndex = 74;
      this.label19.Text = "Pre-Print Success";
      this.label19.TextAlign = ContentAlignment.MiddleCenter;
      this.label20.AutoSize = true;
      this.label20.BackColor = Color.Transparent;
      this.label20.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label20.ForeColor = SystemColors.ButtonFace;
      this.label20.Location = new Point(-1, 72);
      this.label20.Name = "label20";
      this.label20.Size = new Size(249, 25);
      this.label20.TabIndex = 75;
      this.label20.Text = "BarCode Press Directory";
      this.label20.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed2.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed2.Cursor = Cursors.Hand;
      this.butonPPrintFailed2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed2.ForeColor = Color.White;
      this.butonPPrintFailed2.Location = new Point(250, 180);
      this.butonPPrintFailed2.Name = "butonPPrintFailed2";
      this.butonPPrintFailed2.Size = new Size(662, 57);
      this.butonPPrintFailed2.TabIndex = 69;
      this.butonPPrintFailed2.Tag = (object) "2";
      this.butonPPrintFailed2.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed2.UseVisualStyleBackColor = false;
      this.butonPPrintFailed2.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces2.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces2.Cursor = Cursors.Hand;
      this.butonPPrintSucces2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces2.ForeColor = Color.White;
      this.butonPPrintSucces2.Location = new Point(250, 118);
      this.butonPPrintSucces2.Name = "butonPPrintSucces2";
      this.butonPPrintSucces2.Size = new Size(538, 57);
      this.butonPPrintSucces2.TabIndex = 70;
      this.butonPPrintSucces2.Tag = (object) "2";
      this.butonPPrintSucces2.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces2.UseVisualStyleBackColor = false;
      this.butonPPrintSucces2.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory2.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory2.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory2.ForeColor = Color.White;
      this.buttonBarCodePressDirectory2.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory2.Name = "buttonBarCodePressDirectory2";
      this.buttonBarCodePressDirectory2.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory2.TabIndex = 71;
      this.buttonBarCodePressDirectory2.Tag = (object) "2";
      this.buttonBarCodePressDirectory2.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory2.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory2.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.tabSize3.BackColor = Color.White;
      this.tabSize3.BackgroundImage = (Image) Resources._6671;
      this.tabSize3.Controls.Add((Control) this.checkBoxPrefix3);
      this.tabSize3.Controls.Add((Control) this.cb_horizontal3);
      this.tabSize3.Controls.Add((Control) this.checkBoxBarcodePressActive3);
      this.tabSize3.Controls.Add((Control) this.label21);
      this.tabSize3.Controls.Add((Control) this.buttonOutputFolders3);
      this.tabSize3.Controls.Add((Control) this.label22);
      this.tabSize3.Controls.Add((Control) this.label23);
      this.tabSize3.Controls.Add((Control) this.label24);
      this.tabSize3.Controls.Add((Control) this.label25);
      this.tabSize3.Controls.Add((Control) this.barcodeH3);
      this.tabSize3.Controls.Add((Control) this.barcodeW3);
      this.tabSize3.Controls.Add((Control) this.barcodeY3);
      this.tabSize3.Controls.Add((Control) this.barcodeX3);
      this.tabSize3.Controls.Add((Control) this.label26);
      this.tabSize3.Controls.Add((Control) this.label27);
      this.tabSize3.Controls.Add((Control) this.label28);
      this.tabSize3.Controls.Add((Control) this.label29);
      this.tabSize3.Controls.Add((Control) this.butonPPrintFailed3);
      this.tabSize3.Controls.Add((Control) this.butonPPrintSucces3);
      this.tabSize3.Controls.Add((Control) this.buttonBarCodePressDirectory3);
      this.tabSize3.Location = new Point(4, 29);
      this.tabSize3.Name = "tabSize3";
      this.tabSize3.Size = new Size(973, 353);
      this.tabSize3.TabIndex = 2;
      this.tabSize3.Text = "Size 3";
      this.checkBoxPrefix3.AutoSize = true;
      this.checkBoxPrefix3.BackColor = Color.Transparent;
      this.checkBoxPrefix3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix3.ForeColor = SystemColors.Control;
      this.checkBoxPrefix3.Location = new Point(265, 313);
      this.checkBoxPrefix3.Name = "checkBoxPrefix3";
      this.checkBoxPrefix3.Size = new Size(73, 24);
      this.checkBoxPrefix3.TabIndex = 106;
      this.checkBoxPrefix3.Text = "Prefix";
      this.checkBoxPrefix3.UseVisualStyleBackColor = false;
      this.cb_horizontal3.AutoSize = true;
      this.cb_horizontal3.BackColor = Color.Transparent;
      this.cb_horizontal3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.cb_horizontal3.ForeColor = SystemColors.Control;
      this.cb_horizontal3.Location = new Point(447, 254);
      this.cb_horizontal3.Name = "cb_horizontal3";
      this.cb_horizontal3.Size = new Size(73, 17);
      this.cb_horizontal3.TabIndex = 105;
      this.cb_horizontal3.Text = "Horizontal";
      this.cb_horizontal3.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive3.AutoSize = true;
      this.checkBoxBarcodePressActive3.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive3.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive3.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive3.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive3.Name = "checkBoxBarcodePressActive3";
      this.checkBoxBarcodePressActive3.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive3.TabIndex = 104;
      this.checkBoxBarcodePressActive3.Tag = (object) "3";
      this.checkBoxBarcodePressActive3.Text = "Yes";
      this.checkBoxBarcodePressActive3.UseVisualStyleBackColor = false;
      this.label21.AutoSize = true;
      this.label21.BackColor = Color.Transparent;
      this.label21.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label21.ForeColor = SystemColors.ButtonFace;
      this.label21.Location = new Point(159, 23);
      this.label21.Name = "label21";
      this.label21.Size = new Size(91, 25);
      this.label21.TabIndex = 103;
      this.label21.Text = "Enabled";
      this.label21.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders3.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders3.Cursor = Cursors.Hand;
      this.buttonOutputFolders3.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders3.ForeColor = Color.White;
      this.buttonOutputFolders3.Location = new Point(794, 118);
      this.buttonOutputFolders3.Name = "buttonOutputFolders3";
      this.buttonOutputFolders3.Size = new Size(118, 57);
      this.buttonOutputFolders3.TabIndex = 102;
      this.buttonOutputFolders3.Tag = (object) "3";
      this.buttonOutputFolders3.Text = "Output Folders";
      this.buttonOutputFolders3.UseVisualStyleBackColor = false;
      this.buttonOutputFolders3.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label22.AutoSize = true;
      this.label22.BackColor = Color.Transparent;
      this.label22.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label22.ForeColor = Color.White;
      this.label22.Location = new Point(344, 281);
      this.label22.Name = "label22";
      this.label22.Size = new Size(41, 13);
      this.label22.TabIndex = 98;
      this.label22.Text = "Height:";
      this.label23.AutoSize = true;
      this.label23.BackColor = Color.Transparent;
      this.label23.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label23.ForeColor = Color.White;
      this.label23.Location = new Point(367, (int) byte.MaxValue);
      this.label23.Name = "label23";
      this.label23.Size = new Size(17, 13);
      this.label23.TabIndex = 99;
      this.label23.Text = "Y:";
      this.label24.AutoSize = true;
      this.label24.BackColor = Color.Transparent;
      this.label24.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label24.ForeColor = Color.White;
      this.label24.Location = new Point(254, 281);
      this.label24.Name = "label24";
      this.label24.Size = new Size(38, 13);
      this.label24.TabIndex = 100;
      this.label24.Text = "Width:";
      this.label25.AutoSize = true;
      this.label25.BackColor = Color.Transparent;
      this.label25.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label25.ForeColor = Color.White;
      this.label25.Location = new Point(274, (int) byte.MaxValue);
      this.label25.Name = "label25";
      this.label25.Size = new Size(17, 13);
      this.label25.TabIndex = 101;
      this.label25.Text = "X:";
      this.barcodeH3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeH3.Location = new Point(385, 278);
      this.barcodeH3.Name = "barcodeH3";
      this.barcodeH3.Size = new Size(46, 20);
      this.barcodeH3.TabIndex = 94;
      this.barcodeH3.Tag = (object) "3";
      this.barcodeH3.Text = "0";
      this.barcodeW3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeW3.Location = new Point(292, 278);
      this.barcodeW3.Name = "barcodeW3";
      this.barcodeW3.Size = new Size(46, 20);
      this.barcodeW3.TabIndex = 95;
      this.barcodeW3.Tag = (object) "3";
      this.barcodeW3.Text = "0";
      this.barcodeY3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeY3.Location = new Point(385, 252);
      this.barcodeY3.Name = "barcodeY3";
      this.barcodeY3.Size = new Size(46, 20);
      this.barcodeY3.TabIndex = 96;
      this.barcodeY3.Tag = (object) "3";
      this.barcodeY3.Text = "0";
      this.barcodeX3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeX3.Location = new Point(292, 252);
      this.barcodeX3.Name = "barcodeX3";
      this.barcodeX3.Size = new Size(46, 20);
      this.barcodeX3.TabIndex = 97;
      this.barcodeX3.Tag = (object) "3";
      this.barcodeX3.Text = "0";
      this.label26.AutoSize = true;
      this.label26.BackColor = Color.Transparent;
      this.label26.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label26.ForeColor = SystemColors.ButtonFace;
      this.label26.Location = new Point(74, 248);
      this.label26.Name = "label26";
      this.label26.Size = new Size(173, 25);
      this.label26.TabIndex = 90;
      this.label26.Text = "Bar Code Layout";
      this.label26.TextAlign = ContentAlignment.MiddleCenter;
      this.label27.AutoSize = true;
      this.label27.BackColor = Color.Transparent;
      this.label27.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label27.ForeColor = SystemColors.ButtonFace;
      this.label27.Location = new Point(86, 195);
      this.label27.Name = "label27";
      this.label27.Size = new Size(161, 25);
      this.label27.TabIndex = 91;
      this.label27.Text = "Pre-Print Failed";
      this.label27.TextAlign = ContentAlignment.MiddleCenter;
      this.label28.AutoSize = true;
      this.label28.BackColor = Color.Transparent;
      this.label28.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label28.ForeColor = SystemColors.ButtonFace;
      this.label28.Location = new Point(65, 133);
      this.label28.Name = "label28";
      this.label28.Size = new Size(184, 25);
      this.label28.TabIndex = 92;
      this.label28.Text = "Pre-Print Success";
      this.label28.TextAlign = ContentAlignment.MiddleCenter;
      this.label29.AutoSize = true;
      this.label29.BackColor = Color.Transparent;
      this.label29.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label29.ForeColor = SystemColors.ButtonFace;
      this.label29.Location = new Point(-1, 72);
      this.label29.Name = "label29";
      this.label29.Size = new Size(249, 25);
      this.label29.TabIndex = 93;
      this.label29.Text = "BarCode Press Directory";
      this.label29.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed3.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed3.Cursor = Cursors.Hand;
      this.butonPPrintFailed3.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed3.ForeColor = Color.White;
      this.butonPPrintFailed3.Location = new Point(250, 180);
      this.butonPPrintFailed3.Name = "butonPPrintFailed3";
      this.butonPPrintFailed3.Size = new Size(662, 57);
      this.butonPPrintFailed3.TabIndex = 87;
      this.butonPPrintFailed3.Tag = (object) "3";
      this.butonPPrintFailed3.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed3.UseVisualStyleBackColor = false;
      this.butonPPrintFailed3.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces3.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces3.Cursor = Cursors.Hand;
      this.butonPPrintSucces3.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces3.ForeColor = Color.White;
      this.butonPPrintSucces3.Location = new Point(250, 118);
      this.butonPPrintSucces3.Name = "butonPPrintSucces3";
      this.butonPPrintSucces3.Size = new Size(538, 57);
      this.butonPPrintSucces3.TabIndex = 88;
      this.butonPPrintSucces3.Tag = (object) "3";
      this.butonPPrintSucces3.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces3.UseVisualStyleBackColor = false;
      this.butonPPrintSucces3.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory3.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory3.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory3.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory3.ForeColor = Color.White;
      this.buttonBarCodePressDirectory3.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory3.Name = "buttonBarCodePressDirectory3";
      this.buttonBarCodePressDirectory3.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory3.TabIndex = 89;
      this.buttonBarCodePressDirectory3.Tag = (object) "3";
      this.buttonBarCodePressDirectory3.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory3.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory3.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.tabSize4.BackColor = Color.White;
      this.tabSize4.BackgroundImage = (Image) Resources._6671;
      this.tabSize4.Controls.Add((Control) this.checkBoxPrefix4);
      this.tabSize4.Controls.Add((Control) this.cb_horizontal4);
      this.tabSize4.Controls.Add((Control) this.checkBoxBarcodePressActive4);
      this.tabSize4.Controls.Add((Control) this.label30);
      this.tabSize4.Controls.Add((Control) this.buttonOutputFolders4);
      this.tabSize4.Controls.Add((Control) this.label31);
      this.tabSize4.Controls.Add((Control) this.label32);
      this.tabSize4.Controls.Add((Control) this.label33);
      this.tabSize4.Controls.Add((Control) this.label34);
      this.tabSize4.Controls.Add((Control) this.barcodeH4);
      this.tabSize4.Controls.Add((Control) this.barcodeW4);
      this.tabSize4.Controls.Add((Control) this.barcodeY4);
      this.tabSize4.Controls.Add((Control) this.barcodeX4);
      this.tabSize4.Controls.Add((Control) this.label35);
      this.tabSize4.Controls.Add((Control) this.label36);
      this.tabSize4.Controls.Add((Control) this.label37);
      this.tabSize4.Controls.Add((Control) this.label38);
      this.tabSize4.Controls.Add((Control) this.butonPPrintFailed4);
      this.tabSize4.Controls.Add((Control) this.butonPPrintSucces4);
      this.tabSize4.Controls.Add((Control) this.buttonBarCodePressDirectory4);
      this.tabSize4.Location = new Point(4, 29);
      this.tabSize4.Name = "tabSize4";
      this.tabSize4.Size = new Size(973, 353);
      this.tabSize4.TabIndex = 3;
      this.tabSize4.Text = "Size 4";
      this.checkBoxPrefix4.AutoSize = true;
      this.checkBoxPrefix4.BackColor = Color.Transparent;
      this.checkBoxPrefix4.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix4.ForeColor = SystemColors.Control;
      this.checkBoxPrefix4.Location = new Point(265, 313);
      this.checkBoxPrefix4.Name = "checkBoxPrefix4";
      this.checkBoxPrefix4.Size = new Size(73, 24);
      this.checkBoxPrefix4.TabIndex = 124;
      this.checkBoxPrefix4.Text = "Prefix";
      this.checkBoxPrefix4.UseVisualStyleBackColor = false;
      this.cb_horizontal4.AutoSize = true;
      this.cb_horizontal4.BackColor = Color.Transparent;
      this.cb_horizontal4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.cb_horizontal4.ForeColor = SystemColors.Control;
      this.cb_horizontal4.Location = new Point(447, 254);
      this.cb_horizontal4.Name = "cb_horizontal4";
      this.cb_horizontal4.Size = new Size(73, 17);
      this.cb_horizontal4.TabIndex = 123;
      this.cb_horizontal4.Text = "Horizontal";
      this.cb_horizontal4.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive4.AutoSize = true;
      this.checkBoxBarcodePressActive4.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive4.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive4.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive4.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive4.Name = "checkBoxBarcodePressActive4";
      this.checkBoxBarcodePressActive4.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive4.TabIndex = 122;
      this.checkBoxBarcodePressActive4.Tag = (object) "3";
      this.checkBoxBarcodePressActive4.Text = "Yes";
      this.checkBoxBarcodePressActive4.UseVisualStyleBackColor = false;
      this.label30.AutoSize = true;
      this.label30.BackColor = Color.Transparent;
      this.label30.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label30.ForeColor = SystemColors.ButtonFace;
      this.label30.Location = new Point(159, 23);
      this.label30.Name = "label30";
      this.label30.Size = new Size(91, 25);
      this.label30.TabIndex = 121;
      this.label30.Text = "Enabled";
      this.label30.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders4.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders4.Cursor = Cursors.Hand;
      this.buttonOutputFolders4.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders4.ForeColor = Color.White;
      this.buttonOutputFolders4.Location = new Point(794, 118);
      this.buttonOutputFolders4.Name = "buttonOutputFolders4";
      this.buttonOutputFolders4.Size = new Size(118, 57);
      this.buttonOutputFolders4.TabIndex = 120;
      this.buttonOutputFolders4.Tag = (object) "4";
      this.buttonOutputFolders4.Text = "Output Folders";
      this.buttonOutputFolders4.UseVisualStyleBackColor = false;
      this.buttonOutputFolders4.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label31.AutoSize = true;
      this.label31.BackColor = Color.Transparent;
      this.label31.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label31.ForeColor = Color.White;
      this.label31.Location = new Point(344, 281);
      this.label31.Name = "label31";
      this.label31.Size = new Size(41, 13);
      this.label31.TabIndex = 116;
      this.label31.Text = "Height:";
      this.label32.AutoSize = true;
      this.label32.BackColor = Color.Transparent;
      this.label32.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label32.ForeColor = Color.White;
      this.label32.Location = new Point(367, (int) byte.MaxValue);
      this.label32.Name = "label32";
      this.label32.Size = new Size(17, 13);
      this.label32.TabIndex = 117;
      this.label32.Text = "Y:";
      this.label33.AutoSize = true;
      this.label33.BackColor = Color.Transparent;
      this.label33.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label33.ForeColor = Color.White;
      this.label33.Location = new Point(254, 281);
      this.label33.Name = "label33";
      this.label33.Size = new Size(38, 13);
      this.label33.TabIndex = 118;
      this.label33.Text = "Width:";
      this.label34.AutoSize = true;
      this.label34.BackColor = Color.Transparent;
      this.label34.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label34.ForeColor = Color.White;
      this.label34.Location = new Point(274, (int) byte.MaxValue);
      this.label34.Name = "label34";
      this.label34.Size = new Size(17, 13);
      this.label34.TabIndex = 119;
      this.label34.Text = "X:";
      this.barcodeH4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeH4.Location = new Point(385, 278);
      this.barcodeH4.Name = "barcodeH4";
      this.barcodeH4.Size = new Size(46, 20);
      this.barcodeH4.TabIndex = 112;
      this.barcodeH4.Tag = (object) "4";
      this.barcodeH4.Text = "0";
      this.barcodeW4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeW4.Location = new Point(292, 278);
      this.barcodeW4.Name = "barcodeW4";
      this.barcodeW4.Size = new Size(46, 20);
      this.barcodeW4.TabIndex = 113;
      this.barcodeW4.Tag = (object) "4";
      this.barcodeW4.Text = "0";
      this.barcodeY4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeY4.Location = new Point(385, 252);
      this.barcodeY4.Name = "barcodeY4";
      this.barcodeY4.Size = new Size(46, 20);
      this.barcodeY4.TabIndex = 114;
      this.barcodeY4.Tag = (object) "4";
      this.barcodeY4.Text = "0";
      this.barcodeX4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeX4.Location = new Point(292, 252);
      this.barcodeX4.Name = "barcodeX4";
      this.barcodeX4.Size = new Size(46, 20);
      this.barcodeX4.TabIndex = 115;
      this.barcodeX4.Tag = (object) "4";
      this.barcodeX4.Text = "0";
      this.label35.AutoSize = true;
      this.label35.BackColor = Color.Transparent;
      this.label35.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label35.ForeColor = SystemColors.ButtonFace;
      this.label35.Location = new Point(74, 248);
      this.label35.Name = "label35";
      this.label35.Size = new Size(173, 25);
      this.label35.TabIndex = 108;
      this.label35.Text = "Bar Code Layout";
      this.label35.TextAlign = ContentAlignment.MiddleCenter;
      this.label36.AutoSize = true;
      this.label36.BackColor = Color.Transparent;
      this.label36.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label36.ForeColor = SystemColors.ButtonFace;
      this.label36.Location = new Point(86, 195);
      this.label36.Name = "label36";
      this.label36.Size = new Size(161, 25);
      this.label36.TabIndex = 109;
      this.label36.Text = "Pre-Print Failed";
      this.label36.TextAlign = ContentAlignment.MiddleCenter;
      this.label37.AutoSize = true;
      this.label37.BackColor = Color.Transparent;
      this.label37.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label37.ForeColor = SystemColors.ButtonFace;
      this.label37.Location = new Point(65, 133);
      this.label37.Name = "label37";
      this.label37.Size = new Size(184, 25);
      this.label37.TabIndex = 110;
      this.label37.Text = "Pre-Print Success";
      this.label37.TextAlign = ContentAlignment.MiddleCenter;
      this.label38.AutoSize = true;
      this.label38.BackColor = Color.Transparent;
      this.label38.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label38.ForeColor = SystemColors.ButtonFace;
      this.label38.Location = new Point(-1, 72);
      this.label38.Name = "label38";
      this.label38.Size = new Size(249, 25);
      this.label38.TabIndex = 111;
      this.label38.Text = "BarCode Press Directory";
      this.label38.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed4.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed4.Cursor = Cursors.Hand;
      this.butonPPrintFailed4.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed4.ForeColor = Color.White;
      this.butonPPrintFailed4.Location = new Point(250, 180);
      this.butonPPrintFailed4.Name = "butonPPrintFailed4";
      this.butonPPrintFailed4.Size = new Size(662, 57);
      this.butonPPrintFailed4.TabIndex = 105;
      this.butonPPrintFailed4.Tag = (object) "4";
      this.butonPPrintFailed4.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed4.UseVisualStyleBackColor = false;
      this.butonPPrintFailed4.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces4.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces4.Cursor = Cursors.Hand;
      this.butonPPrintSucces4.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces4.ForeColor = Color.White;
      this.butonPPrintSucces4.Location = new Point(250, 118);
      this.butonPPrintSucces4.Name = "butonPPrintSucces4";
      this.butonPPrintSucces4.Size = new Size(538, 57);
      this.butonPPrintSucces4.TabIndex = 106;
      this.butonPPrintSucces4.Tag = (object) "4";
      this.butonPPrintSucces4.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces4.UseVisualStyleBackColor = false;
      this.butonPPrintSucces4.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory4.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory4.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory4.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory4.ForeColor = Color.White;
      this.buttonBarCodePressDirectory4.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory4.Name = "buttonBarCodePressDirectory4";
      this.buttonBarCodePressDirectory4.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory4.TabIndex = 107;
      this.buttonBarCodePressDirectory4.Tag = (object) "4";
      this.buttonBarCodePressDirectory4.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory4.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory4.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.tabSize5.BackColor = Color.White;
      this.tabSize5.BackgroundImage = (Image) Resources._6671;
      this.tabSize5.Controls.Add((Control) this.checkBoxPrefix5);
      this.tabSize5.Controls.Add((Control) this.cb_horizontal5);
      this.tabSize5.Controls.Add((Control) this.checkBoxBarcodePressActive5);
      this.tabSize5.Controls.Add((Control) this.label39);
      this.tabSize5.Controls.Add((Control) this.buttonOutputFolders5);
      this.tabSize5.Controls.Add((Control) this.label40);
      this.tabSize5.Controls.Add((Control) this.label41);
      this.tabSize5.Controls.Add((Control) this.label42);
      this.tabSize5.Controls.Add((Control) this.label43);
      this.tabSize5.Controls.Add((Control) this.barcodeH5);
      this.tabSize5.Controls.Add((Control) this.barcodeW5);
      this.tabSize5.Controls.Add((Control) this.barcodeY5);
      this.tabSize5.Controls.Add((Control) this.barcodeX5);
      this.tabSize5.Controls.Add((Control) this.label44);
      this.tabSize5.Controls.Add((Control) this.label45);
      this.tabSize5.Controls.Add((Control) this.label46);
      this.tabSize5.Controls.Add((Control) this.label47);
      this.tabSize5.Controls.Add((Control) this.butonPPrintFailed5);
      this.tabSize5.Controls.Add((Control) this.butonPPrintSucces5);
      this.tabSize5.Controls.Add((Control) this.buttonBarCodePressDirectory5);
      this.tabSize5.Location = new Point(4, 29);
      this.tabSize5.Name = "tabSize5";
      this.tabSize5.Size = new Size(973, 353);
      this.tabSize5.TabIndex = 4;
      this.tabSize5.Text = "Size 5";
      this.checkBoxPrefix5.AutoSize = true;
      this.checkBoxPrefix5.BackColor = Color.Transparent;
      this.checkBoxPrefix5.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix5.ForeColor = SystemColors.Control;
      this.checkBoxPrefix5.Location = new Point(265, 313);
      this.checkBoxPrefix5.Name = "checkBoxPrefix5";
      this.checkBoxPrefix5.Size = new Size(73, 24);
      this.checkBoxPrefix5.TabIndex = 142;
      this.checkBoxPrefix5.Text = "Prefix";
      this.checkBoxPrefix5.UseVisualStyleBackColor = false;
      this.cb_horizontal5.AutoSize = true;
      this.cb_horizontal5.BackColor = Color.Transparent;
      this.cb_horizontal5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.cb_horizontal5.ForeColor = SystemColors.Control;
      this.cb_horizontal5.Location = new Point(447, 254);
      this.cb_horizontal5.Name = "cb_horizontal5";
      this.cb_horizontal5.Size = new Size(73, 17);
      this.cb_horizontal5.TabIndex = 141;
      this.cb_horizontal5.Text = "Horizontal";
      this.cb_horizontal5.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive5.AutoSize = true;
      this.checkBoxBarcodePressActive5.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive5.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive5.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive5.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive5.Name = "checkBoxBarcodePressActive5";
      this.checkBoxBarcodePressActive5.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive5.TabIndex = 140;
      this.checkBoxBarcodePressActive5.Tag = (object) "5";
      this.checkBoxBarcodePressActive5.Text = "Yes";
      this.checkBoxBarcodePressActive5.UseVisualStyleBackColor = false;
      this.label39.AutoSize = true;
      this.label39.BackColor = Color.Transparent;
      this.label39.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label39.ForeColor = SystemColors.ButtonFace;
      this.label39.Location = new Point(159, 23);
      this.label39.Name = "label39";
      this.label39.Size = new Size(91, 25);
      this.label39.TabIndex = 139;
      this.label39.Text = "Enabled";
      this.label39.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders5.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders5.Cursor = Cursors.Hand;
      this.buttonOutputFolders5.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders5.ForeColor = Color.White;
      this.buttonOutputFolders5.Location = new Point(794, 118);
      this.buttonOutputFolders5.Name = "buttonOutputFolders5";
      this.buttonOutputFolders5.Size = new Size(118, 57);
      this.buttonOutputFolders5.TabIndex = 138;
      this.buttonOutputFolders5.Tag = (object) "5";
      this.buttonOutputFolders5.Text = "Output Folders";
      this.buttonOutputFolders5.UseVisualStyleBackColor = false;
      this.buttonOutputFolders5.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label40.AutoSize = true;
      this.label40.BackColor = Color.Transparent;
      this.label40.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label40.ForeColor = Color.White;
      this.label40.Location = new Point(344, 281);
      this.label40.Name = "label40";
      this.label40.Size = new Size(41, 13);
      this.label40.TabIndex = 134;
      this.label40.Text = "Height:";
      this.label41.AutoSize = true;
      this.label41.BackColor = Color.Transparent;
      this.label41.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label41.ForeColor = Color.White;
      this.label41.Location = new Point(367, (int) byte.MaxValue);
      this.label41.Name = "label41";
      this.label41.Size = new Size(17, 13);
      this.label41.TabIndex = 135;
      this.label41.Text = "Y:";
      this.label42.AutoSize = true;
      this.label42.BackColor = Color.Transparent;
      this.label42.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label42.ForeColor = Color.White;
      this.label42.Location = new Point(254, 281);
      this.label42.Name = "label42";
      this.label42.Size = new Size(38, 13);
      this.label42.TabIndex = 136;
      this.label42.Text = "Width:";
      this.label43.AutoSize = true;
      this.label43.BackColor = Color.Transparent;
      this.label43.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label43.ForeColor = Color.White;
      this.label43.Location = new Point(274, (int) byte.MaxValue);
      this.label43.Name = "label43";
      this.label43.Size = new Size(17, 13);
      this.label43.TabIndex = 137;
      this.label43.Text = "X:";
      this.barcodeH5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeH5.Location = new Point(385, 278);
      this.barcodeH5.Name = "barcodeH5";
      this.barcodeH5.Size = new Size(46, 20);
      this.barcodeH5.TabIndex = 130;
      this.barcodeH5.Tag = (object) "5";
      this.barcodeH5.Text = "0";
      this.barcodeW5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeW5.Location = new Point(292, 278);
      this.barcodeW5.Name = "barcodeW5";
      this.barcodeW5.Size = new Size(46, 20);
      this.barcodeW5.TabIndex = 131;
      this.barcodeW5.Tag = (object) "5";
      this.barcodeW5.Text = "0";
      this.barcodeY5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeY5.Location = new Point(385, 252);
      this.barcodeY5.Name = "barcodeY5";
      this.barcodeY5.Size = new Size(46, 20);
      this.barcodeY5.TabIndex = 132;
      this.barcodeY5.Tag = (object) "5";
      this.barcodeY5.Text = "0";
      this.barcodeX5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeX5.Location = new Point(292, 252);
      this.barcodeX5.Name = "barcodeX5";
      this.barcodeX5.Size = new Size(46, 20);
      this.barcodeX5.TabIndex = 133;
      this.barcodeX5.Tag = (object) "5";
      this.barcodeX5.Text = "0";
      this.label44.AutoSize = true;
      this.label44.BackColor = Color.Transparent;
      this.label44.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label44.ForeColor = SystemColors.ButtonFace;
      this.label44.Location = new Point(74, 248);
      this.label44.Name = "label44";
      this.label44.Size = new Size(173, 25);
      this.label44.TabIndex = 126;
      this.label44.Text = "Bar Code Layout";
      this.label44.TextAlign = ContentAlignment.MiddleCenter;
      this.label45.AutoSize = true;
      this.label45.BackColor = Color.Transparent;
      this.label45.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label45.ForeColor = SystemColors.ButtonFace;
      this.label45.Location = new Point(86, 195);
      this.label45.Name = "label45";
      this.label45.Size = new Size(161, 25);
      this.label45.TabIndex = (int) sbyte.MaxValue;
      this.label45.Text = "Pre-Print Failed";
      this.label45.TextAlign = ContentAlignment.MiddleCenter;
      this.label46.AutoSize = true;
      this.label46.BackColor = Color.Transparent;
      this.label46.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label46.ForeColor = SystemColors.ButtonFace;
      this.label46.Location = new Point(65, 133);
      this.label46.Name = "label46";
      this.label46.Size = new Size(184, 25);
      this.label46.TabIndex = 128;
      this.label46.Text = "Pre-Print Success";
      this.label46.TextAlign = ContentAlignment.MiddleCenter;
      this.label47.AutoSize = true;
      this.label47.BackColor = Color.Transparent;
      this.label47.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label47.ForeColor = SystemColors.ButtonFace;
      this.label47.Location = new Point(-1, 72);
      this.label47.Name = "label47";
      this.label47.Size = new Size(249, 25);
      this.label47.TabIndex = 129;
      this.label47.Text = "BarCode Press Directory";
      this.label47.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed5.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed5.Cursor = Cursors.Hand;
      this.butonPPrintFailed5.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed5.ForeColor = Color.White;
      this.butonPPrintFailed5.Location = new Point(250, 180);
      this.butonPPrintFailed5.Name = "butonPPrintFailed5";
      this.butonPPrintFailed5.Size = new Size(662, 57);
      this.butonPPrintFailed5.TabIndex = 123;
      this.butonPPrintFailed5.Tag = (object) "5";
      this.butonPPrintFailed5.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed5.UseVisualStyleBackColor = false;
      this.butonPPrintFailed5.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces5.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces5.Cursor = Cursors.Hand;
      this.butonPPrintSucces5.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces5.ForeColor = Color.White;
      this.butonPPrintSucces5.Location = new Point(250, 118);
      this.butonPPrintSucces5.Name = "butonPPrintSucces5";
      this.butonPPrintSucces5.Size = new Size(538, 57);
      this.butonPPrintSucces5.TabIndex = 124;
      this.butonPPrintSucces5.Tag = (object) "5";
      this.butonPPrintSucces5.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces5.UseVisualStyleBackColor = false;
      this.butonPPrintSucces5.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory5.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory5.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory5.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory5.ForeColor = Color.White;
      this.buttonBarCodePressDirectory5.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory5.Name = "buttonBarCodePressDirectory5";
      this.buttonBarCodePressDirectory5.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory5.TabIndex = 125;
      this.buttonBarCodePressDirectory5.Tag = (object) "5";
      this.buttonBarCodePressDirectory5.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory5.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory5.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.tabSize6.BackColor = Color.White;
      this.tabSize6.BackgroundImage = (Image) Resources._6671;
      this.tabSize6.Controls.Add((Control) this.checkBoxPrefix6);
      this.tabSize6.Controls.Add((Control) this.cb_horizontal6);
      this.tabSize6.Controls.Add((Control) this.checkBoxBarcodePressActive6);
      this.tabSize6.Controls.Add((Control) this.label48);
      this.tabSize6.Controls.Add((Control) this.buttonOutputFolders6);
      this.tabSize6.Controls.Add((Control) this.label49);
      this.tabSize6.Controls.Add((Control) this.label50);
      this.tabSize6.Controls.Add((Control) this.label51);
      this.tabSize6.Controls.Add((Control) this.label52);
      this.tabSize6.Controls.Add((Control) this.barcodeH6);
      this.tabSize6.Controls.Add((Control) this.barcodeW6);
      this.tabSize6.Controls.Add((Control) this.barcodeY6);
      this.tabSize6.Controls.Add((Control) this.barcodeX6);
      this.tabSize6.Controls.Add((Control) this.label53);
      this.tabSize6.Controls.Add((Control) this.label54);
      this.tabSize6.Controls.Add((Control) this.label55);
      this.tabSize6.Controls.Add((Control) this.label56);
      this.tabSize6.Controls.Add((Control) this.butonPPrintFailed6);
      this.tabSize6.Controls.Add((Control) this.butonPPrintSucces6);
      this.tabSize6.Controls.Add((Control) this.buttonBarCodePressDirectory6);
      this.tabSize6.Location = new Point(4, 29);
      this.tabSize6.Name = "tabSize6";
      this.tabSize6.Size = new Size(973, 353);
      this.tabSize6.TabIndex = 5;
      this.tabSize6.Text = "Size 6";
      this.checkBoxPrefix6.AutoSize = true;
      this.checkBoxPrefix6.BackColor = Color.Transparent;
      this.checkBoxPrefix6.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxPrefix6.ForeColor = SystemColors.Control;
      this.checkBoxPrefix6.Location = new Point(265, 313);
      this.checkBoxPrefix6.Name = "checkBoxPrefix6";
      this.checkBoxPrefix6.Size = new Size(73, 24);
      this.checkBoxPrefix6.TabIndex = 160;
      this.checkBoxPrefix6.Text = "Prefix";
      this.checkBoxPrefix6.UseVisualStyleBackColor = false;
      this.cb_horizontal6.AutoSize = true;
      this.cb_horizontal6.BackColor = Color.Transparent;
      this.cb_horizontal6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.cb_horizontal6.ForeColor = SystemColors.Control;
      this.cb_horizontal6.Location = new Point(447, 254);
      this.cb_horizontal6.Name = "cb_horizontal6";
      this.cb_horizontal6.Size = new Size(73, 17);
      this.cb_horizontal6.TabIndex = 159;
      this.cb_horizontal6.Text = "Horizontal";
      this.cb_horizontal6.UseVisualStyleBackColor = false;
      this.checkBoxBarcodePressActive6.AutoSize = true;
      this.checkBoxBarcodePressActive6.BackColor = Color.Transparent;
      this.checkBoxBarcodePressActive6.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.checkBoxBarcodePressActive6.ForeColor = SystemColors.ButtonFace;
      this.checkBoxBarcodePressActive6.Location = new Point(250, 22);
      this.checkBoxBarcodePressActive6.Name = "checkBoxBarcodePressActive6";
      this.checkBoxBarcodePressActive6.Size = new Size(69, 29);
      this.checkBoxBarcodePressActive6.TabIndex = 158;
      this.checkBoxBarcodePressActive6.Tag = (object) "6";
      this.checkBoxBarcodePressActive6.Text = "Yes";
      this.checkBoxBarcodePressActive6.UseVisualStyleBackColor = false;
      this.label48.AutoSize = true;
      this.label48.BackColor = Color.Transparent;
      this.label48.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label48.ForeColor = SystemColors.ButtonFace;
      this.label48.Location = new Point(159, 23);
      this.label48.Name = "label48";
      this.label48.Size = new Size(91, 25);
      this.label48.TabIndex = 157;
      this.label48.Text = "Enabled";
      this.label48.TextAlign = ContentAlignment.MiddleCenter;
      this.buttonOutputFolders6.BackColor = Color.DodgerBlue;
      this.buttonOutputFolders6.Cursor = Cursors.Hand;
      this.buttonOutputFolders6.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonOutputFolders6.ForeColor = Color.White;
      this.buttonOutputFolders6.Location = new Point(794, 118);
      this.buttonOutputFolders6.Name = "buttonOutputFolders6";
      this.buttonOutputFolders6.Size = new Size(118, 57);
      this.buttonOutputFolders6.TabIndex = 156;
      this.buttonOutputFolders6.Tag = (object) "6";
      this.buttonOutputFolders6.Text = "Output Folders";
      this.buttonOutputFolders6.UseVisualStyleBackColor = false;
      this.buttonOutputFolders6.Click += new EventHandler(this.buttonOutputFolders_Click);
      this.label49.AutoSize = true;
      this.label49.BackColor = Color.Transparent;
      this.label49.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label49.ForeColor = Color.White;
      this.label49.Location = new Point(344, 281);
      this.label49.Name = "label49";
      this.label49.Size = new Size(41, 13);
      this.label49.TabIndex = 152;
      this.label49.Text = "Height:";
      this.label50.AutoSize = true;
      this.label50.BackColor = Color.Transparent;
      this.label50.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label50.ForeColor = Color.White;
      this.label50.Location = new Point(367, (int) byte.MaxValue);
      this.label50.Name = "label50";
      this.label50.Size = new Size(17, 13);
      this.label50.TabIndex = 153;
      this.label50.Text = "Y:";
      this.label51.AutoSize = true;
      this.label51.BackColor = Color.Transparent;
      this.label51.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label51.ForeColor = Color.White;
      this.label51.Location = new Point(254, 281);
      this.label51.Name = "label51";
      this.label51.Size = new Size(38, 13);
      this.label51.TabIndex = 154;
      this.label51.Text = "Width:";
      this.label52.AutoSize = true;
      this.label52.BackColor = Color.Transparent;
      this.label52.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label52.ForeColor = Color.White;
      this.label52.Location = new Point(274, (int) byte.MaxValue);
      this.label52.Name = "label52";
      this.label52.Size = new Size(17, 13);
      this.label52.TabIndex = 155;
      this.label52.Text = "X:";
      this.barcodeH6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeH6.Location = new Point(385, 278);
      this.barcodeH6.Name = "barcodeH6";
      this.barcodeH6.Size = new Size(46, 20);
      this.barcodeH6.TabIndex = 148;
      this.barcodeH6.Tag = (object) "6";
      this.barcodeH6.Text = "0";
      this.barcodeW6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeW6.Location = new Point(292, 278);
      this.barcodeW6.Name = "barcodeW6";
      this.barcodeW6.Size = new Size(46, 20);
      this.barcodeW6.TabIndex = 149;
      this.barcodeW6.Tag = (object) "6";
      this.barcodeW6.Text = "0";
      this.barcodeY6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeY6.Location = new Point(385, 252);
      this.barcodeY6.Name = "barcodeY6";
      this.barcodeY6.Size = new Size(46, 20);
      this.barcodeY6.TabIndex = 150;
      this.barcodeY6.Tag = (object) "6";
      this.barcodeY6.Text = "0";
      this.barcodeX6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.barcodeX6.Location = new Point(292, 252);
      this.barcodeX6.Name = "barcodeX6";
      this.barcodeX6.Size = new Size(46, 20);
      this.barcodeX6.TabIndex = 151;
      this.barcodeX6.Tag = (object) "6";
      this.barcodeX6.Text = "0";
      this.label53.AutoSize = true;
      this.label53.BackColor = Color.Transparent;
      this.label53.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label53.ForeColor = SystemColors.ButtonFace;
      this.label53.Location = new Point(74, 248);
      this.label53.Name = "label53";
      this.label53.Size = new Size(173, 25);
      this.label53.TabIndex = 144;
      this.label53.Text = "Bar Code Layout";
      this.label53.TextAlign = ContentAlignment.MiddleCenter;
      this.label54.AutoSize = true;
      this.label54.BackColor = Color.Transparent;
      this.label54.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label54.ForeColor = SystemColors.ButtonFace;
      this.label54.Location = new Point(86, 195);
      this.label54.Name = "label54";
      this.label54.Size = new Size(161, 25);
      this.label54.TabIndex = 145;
      this.label54.Text = "Pre-Print Failed";
      this.label54.TextAlign = ContentAlignment.MiddleCenter;
      this.label55.AutoSize = true;
      this.label55.BackColor = Color.Transparent;
      this.label55.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label55.ForeColor = SystemColors.ButtonFace;
      this.label55.Location = new Point(65, 133);
      this.label55.Name = "label55";
      this.label55.Size = new Size(184, 25);
      this.label55.TabIndex = 146;
      this.label55.Text = "Pre-Print Success";
      this.label55.TextAlign = ContentAlignment.MiddleCenter;
      this.label56.AutoSize = true;
      this.label56.BackColor = Color.Transparent;
      this.label56.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label56.ForeColor = SystemColors.ButtonFace;
      this.label56.Location = new Point(-1, 72);
      this.label56.Name = "label56";
      this.label56.Size = new Size(249, 25);
      this.label56.TabIndex = 147;
      this.label56.Text = "BarCode Press Directory";
      this.label56.TextAlign = ContentAlignment.MiddleCenter;
      this.butonPPrintFailed6.BackColor = Color.DodgerBlue;
      this.butonPPrintFailed6.Cursor = Cursors.Hand;
      this.butonPPrintFailed6.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintFailed6.ForeColor = Color.White;
      this.butonPPrintFailed6.Location = new Point(250, 180);
      this.butonPPrintFailed6.Name = "butonPPrintFailed6";
      this.butonPPrintFailed6.Size = new Size(662, 57);
      this.butonPPrintFailed6.TabIndex = 141;
      this.butonPPrintFailed6.Tag = (object) "6";
      this.butonPPrintFailed6.Text = "Pre-Print Failed Directory";
      this.butonPPrintFailed6.UseVisualStyleBackColor = false;
      this.butonPPrintFailed6.Click += new EventHandler(this.butonPPrintFailed_Click);
      this.butonPPrintSucces6.BackColor = Color.DodgerBlue;
      this.butonPPrintSucces6.Cursor = Cursors.Hand;
      this.butonPPrintSucces6.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.butonPPrintSucces6.ForeColor = Color.White;
      this.butonPPrintSucces6.Location = new Point(250, 118);
      this.butonPPrintSucces6.Name = "butonPPrintSucces6";
      this.butonPPrintSucces6.Size = new Size(538, 57);
      this.butonPPrintSucces6.TabIndex = 142;
      this.butonPPrintSucces6.Tag = (object) "6";
      this.butonPPrintSucces6.Text = "Pre-Print Success Directory";
      this.butonPPrintSucces6.UseVisualStyleBackColor = false;
      this.butonPPrintSucces6.Click += new EventHandler(this.butonPPrintSuccess_Click);
      this.buttonBarCodePressDirectory6.BackColor = Color.DodgerBlue;
      this.buttonBarCodePressDirectory6.Cursor = Cursors.Hand;
      this.buttonBarCodePressDirectory6.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.buttonBarCodePressDirectory6.ForeColor = Color.White;
      this.buttonBarCodePressDirectory6.Location = new Point(250, 57);
      this.buttonBarCodePressDirectory6.Name = "buttonBarCodePressDirectory6";
      this.buttonBarCodePressDirectory6.Size = new Size(662, 57);
      this.buttonBarCodePressDirectory6.TabIndex = 143;
      this.buttonBarCodePressDirectory6.Tag = (object) "6";
      this.buttonBarCodePressDirectory6.Text = "BarCode Press Directory";
      this.buttonBarCodePressDirectory6.UseVisualStyleBackColor = false;
      this.buttonBarCodePressDirectory6.Click += new EventHandler(this.buttonBarCodePressDirectory_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources._6671;
      this.Controls.Add((Control) this.tabControlBarcodeSettings);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.comboBoxLocation);
      this.Controls.Add((Control) this.SettingPhotoTaken);
      this.Controls.Add((Control) this.buttonBarcodePrintSave);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (BarcodePrintSettings);
      this.Size = new Size(1024, 656);
      this.tabControlBarcodeSettings.ResumeLayout(false);
      this.tabSize1.ResumeLayout(false);
      this.tabSize1.PerformLayout();
      this.tabSize2.ResumeLayout(false);
      this.tabSize2.PerformLayout();
      this.tabSize3.ResumeLayout(false);
      this.tabSize3.PerformLayout();
      this.tabSize4.ResumeLayout(false);
      this.tabSize4.PerformLayout();
      this.tabSize5.ResumeLayout(false);
      this.tabSize5.PerformLayout();
      this.tabSize6.ResumeLayout(false);
      this.tabSize6.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
