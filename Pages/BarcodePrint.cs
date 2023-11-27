using BarcodeLib;
using SmartLocationApp.Pages.Classes;
using SmartLocationApp.Properties;
using SmartLocationApp.Router;
using SmartLocationApp.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SmartLocationApp.Pages
{
  public class BarcodePrint : UserControl
  {
    private static int COUNT = 6;
    private Barcode b = new Barcode();
    private MyFileSystemWatcher[] PreBarcodeWatchers;
    private MyFileSystemWatcher[] PrePrintWatchers;
    private List<OutputFoldersStructure>[] opFolders;
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
    private Datas data;
    private PageRouter router;
    private IContainer components;
    private Label label1;
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;

    public BarcodePrint() => this.InitializeComponent();

    public void init(PageRouter _router)
    {
      this.router = _router;
      this.data = ReadWrite.ReadFromXmlFile<List<Datas>>(ReadWrite.dbPath)[0];
      this.PreBarcodeWatchers = new MyFileSystemWatcher[BarcodePrint.COUNT];
      this.PrePrintWatchers = new MyFileSystemWatcher[BarcodePrint.COUNT];
      this.opFolders = new List<OutputFoldersStructure>[BarcodePrint.COUNT];
      this.BarCodePressActives = new bool[6]
      {
        this.data.BarCodePressActive1,
        this.data.BarCodePressActive2,
        this.data.BarCodePressActive3,
        this.data.BarCodePressActive4,
        this.data.BarCodePressActive5,
        this.data.BarCodePressActive6
      };
      this.BarCodePress_Directories = new string[6]
      {
        this.data.BarCodePress_Directory1,
        this.data.BarCodePress_Directory2,
        this.data.BarCodePress_Directory3,
        this.data.BarCodePress_Directory4,
        this.data.BarCodePress_Directory5,
        this.data.BarCodePress_Directory6
      };
      this.PrePrintSuccess_Directories = new string[6]
      {
        this.data.PrePrintSuccess_Directory1,
        this.data.PrePrintSuccess_Directory2,
        this.data.PrePrintSuccess_Directory3,
        this.data.PrePrintSuccess_Directory4,
        this.data.PrePrintSuccess_Directory5,
        this.data.PrePrintSuccess_Directory6
      };
      this.PrePrintFailed_Directories = new string[6]
      {
        this.data.PrePrintFailed_Directory1,
        this.data.PrePrintFailed_Directory2,
        this.data.PrePrintFailed_Directory3,
        this.data.PrePrintFailed_Directory4,
        this.data.PrePrintFailed_Directory5,
        this.data.PrePrintFailed_Directory6
      };
      this.OutputFoldersJson_Directoreis = new string[6]
      {
        this.data.OutputFoldersJson1,
        this.data.OutputFoldersJson2,
        this.data.OutputFoldersJson3,
        this.data.OutputFoldersJson4,
        this.data.OutputFoldersJson5,
        this.data.OutputFoldersJson6
      };
      this.BarcodeWs = new int[6]
      {
        this.data.barcodeW1,
        this.data.barcodeW2,
        this.data.barcodeW3,
        this.data.barcodeW4,
        this.data.barcodeW5,
        this.data.barcodeW6
      };
      this.BarcodeHs = new int[6]
      {
        this.data.barcodeH1,
        this.data.barcodeH2,
        this.data.barcodeH3,
        this.data.barcodeH4,
        this.data.barcodeH5,
        this.data.barcodeH6
      };
      this.BarcodeXs = new int[6]
      {
        this.data.barcodeX1,
        this.data.barcodeX2,
        this.data.barcodeX3,
        this.data.barcodeX4,
        this.data.barcodeX5,
        this.data.barcodeX6
      };
      this.BarcodeYs = new int[6]
      {
        this.data.barcodeY1,
        this.data.barcodeY2,
        this.data.barcodeY3,
        this.data.barcodeY4,
        this.data.barcodeY5,
        this.data.barcodeY6
      };
      this.Horizontal = new bool[6]
      {
        this.data.horizontal1,
        this.data.horizontal2,
        this.data.horizontal3,
        this.data.horizontal4,
        this.data.horizontal5,
        this.data.horizontal6
      };
      this.Prefix = new bool[6]
      {
        this.data.prefix1,
        this.data.prefix2,
        this.data.prefix3,
        this.data.prefix4,
        this.data.prefix5,
        this.data.prefix6
      };
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.BarCodePressActives[index])
          this.opFolders[index] = this.handleOPFolders(this.OutputFoldersJson_Directoreis[index]);
      }
      this.disposeFileWatchers();
      this.handlePreBarcodeWatcher();
      this.handlePrePrintWatcher();
    }

    public void enableFileWatchers()
    {
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.BarCodePressActives[index])
        {
          if (this.PrePrintWatchers[index] != null)
            this.PrePrintWatchers[index].EnableRaisingEvents = true;
          if (this.PreBarcodeWatchers[index] != null)
            this.PreBarcodeWatchers[index].EnableRaisingEvents = true;
        }
      }
    }

    public void disableFileWatchers()
    {
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.PrePrintWatchers != null && this.PrePrintWatchers[index] != null)
          this.PrePrintWatchers[index].EnableRaisingEvents = false;
        if (this.PrePrintWatchers != null && this.PreBarcodeWatchers[index] != null)
          this.PreBarcodeWatchers[index].EnableRaisingEvents = false;
      }
    }

    public void disposeFileWatchers()
    {
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.PrePrintWatchers != null && this.PrePrintWatchers[index] != null)
          this.PrePrintWatchers[index].Dispose();
        if (this.PreBarcodeWatchers != null && this.PreBarcodeWatchers[index] != null)
          this.PreBarcodeWatchers[index].Dispose();
      }
    }

    private void handlePreBarcodeWatcher()
    {
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.BarCodePressActives[index])
        {
          this.PreBarcodeWatchers[index] = new MyFileSystemWatcher();
          this.PreBarcodeWatchers[index].Path = this.BarCodePress_Directories[index];
          this.PreBarcodeWatchers[index].Filter = "*.*";
          this.PreBarcodeWatchers[index].Created += new FileSystemEventHandler(this.OnCreateNewTicketedImage);
          this.PreBarcodeWatchers[index].SynchronizingObject = (ISynchronizeInvoke) this;
          this.PreBarcodeWatchers[index].Index = index;
        }
      }
    }

    private void handlePrePrintWatcher()
    {
      for (int index = 0; index < BarcodePrint.COUNT; ++index)
      {
        if (this.BarCodePressActives[index])
        {
          this.PrePrintWatchers[index] = new MyFileSystemWatcher();
          this.PrePrintWatchers[index].Path = this.PrePrintSuccess_Directories[index];
          this.PrePrintWatchers[index].Filter = "*.*";
          this.PrePrintWatchers[index].Created += new FileSystemEventHandler(this.OnCreateNewPreSuccessImage);
          this.PrePrintWatchers[index].SynchronizingObject = (ISynchronizeInvoke) this;
          this.PrePrintWatchers[index].Index = index;
        }
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void OnCreateNewPreSuccessImage(object source, FileSystemEventArgs e)
    {
      Console.Out.WriteLine("BarcodePrint.OnCreateNewPreSuccessImage");
      try
      {
        int index = ((MyFileSystemWatcher) source).Index;
        string str1 = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt", (IFormatProvider) new CultureInfo("en-US"));
        FileInfo file = new FileInfo(e.FullPath);
        List<OutputFoldersStructure> foldersStructureList1 = new List<OutputFoldersStructure>();
        bool flag1 = false;
        bool flag2 = false;
        while (File.Exists(file.FullName) && this.IsFileLocked(file))
        {
          Thread.Sleep(100);
          Console.Out.WriteLine("BarcodePrint.OnCreateNewPreSuccessImage --> IsFileLocked:" + file.Name);
        }
        if (!Function.IsImageFileValid(file.FullName))
          return;
        foreach (OutputFoldersStructure foldersStructure in this.opFolders[index])
        {
          if (this.Prefix[index])
          {
            string str2 = file.Name.Substring(0, foldersStructure.prefixValue.Length);
            if (foldersStructure.prefixValue == str2)
            {
              this.folderNotExistsCreate(foldersStructure.outputFolder);
              File.Move(file.FullName, foldersStructure.outputFolder + "\\" + file.Name);
              ++foldersStructure.loop;
              this.listView1.Items.Add(new ListViewItem(new string[5]
              {
                string.Format("Size {0}", (object) (index + 1)),
                str1,
                string.Format("{0}/{1}", (object) foldersStructure.loop, (object) foldersStructure.amount),
                file.Name,
                foldersStructure.outputFolder
              }));
            }
          }
          else
          {
            if (flag1 && !flag2)
            {
              foldersStructure.queue = true;
              flag2 = true;
            }
            if (foldersStructure.queue)
            {
              this.folderNotExistsCreate(foldersStructure.outputFolder);
              File.Move(file.FullName, foldersStructure.outputFolder + "\\" + file.Name);
              ++foldersStructure.loop;
              this.listView1.Items.Add(new ListViewItem(new string[5]
              {
                string.Format("Size {0}", (object) (index + 1)),
                str1,
                string.Format("{0}/{1}", (object) foldersStructure.loop, (object) foldersStructure.amount),
                file.Name,
                foldersStructure.outputFolder
              }));
              if (foldersStructure.loop >= foldersStructure.amount)
              {
                foldersStructure.queue = false;
                flag1 = true;
                if (foldersStructure.cycle)
                  foldersStructure.loop = 0;
                else
                  continue;
              }
            }
          }
          foldersStructureList1.Add(foldersStructure);
        }
        this.opFolders[index] = foldersStructureList1;
        List<OutputFoldersStructure> foldersStructureList2 = new List<OutputFoldersStructure>();
        if (!flag1 || flag2)
          return;
        int num = 0;
        foreach (OutputFoldersStructure foldersStructure in this.opFolders[index])
        {
          if (num == 0)
            foldersStructure.queue = true;
          ++num;
          foldersStructureList2.Add(foldersStructure);
        }
        this.opFolders[index] = foldersStructureList2;
      }
      catch (Exception ex)
      {
        Console.WriteLine("BarcodePrint.OnCreateNewPreSuccessImage --> " + ex.Message);
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void OnCreateNewTicketedImage(object source, FileSystemEventArgs e)
    {
      string json = "";
      Console.Out.WriteLine("BarcodePrint.OnCreateNewTicketedImage");
      try
      {
        int index = ((MyFileSystemWatcher) source).Index;
        this.folderNotExistsCreate(this.BarCodePress_Directories[index]);
        FileInfo file = new FileInfo(e.FullPath);
        while (File.Exists(file.FullName) && this.IsFileLocked(file))
        {
          Thread.Sleep(100);
          Console.Out.WriteLine("BarcodePrint.OnCreateNewTicketedImage --> IsFileLocked:" + file.Name);
        }
        if (!Function.IsImageFileValid(file.FullName))
          return;
        List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>()
        {
          new KeyValuePair<string, string>("location_id", this.data.Location),
          new KeyValuePair<string, string>("image_name", file.Name)
        };
        MultipartFormDataContent content = new MultipartFormDataContent();
        foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
          content.Add((HttpContent) new StringContent(keyValuePair.Value), keyValuePair.Key);
        HttpResponseMessage result = new HttpClient()
        {
          BaseAddress = new Uri(Animation.Url)
        }.PostAsync("photos/photoBarcode", (HttpContent) content).Result;
        if (!result.IsSuccessStatusCode)
          return;
        json = result.Content.ReadAsStringAsync().Result;
        Console.Out.WriteLine("BarcodePrint.OnCreateNewTicketedImage --> " + json);
        this.handlePhotoBarCode(json, file.FullName, file.Name, index);
      }
      catch (Exception ex)
      {
        Logger.Error("BarcodePrint.OnCreateNewTicketedImage.catch --> " + ex.Message + " Response: " + json);
      }
    }

    private void handlePhotoBarCode(string json, string source, string finalPhotoName, int index)
    {
      PhotoBarcode photoBarcode = new JavaScriptSerializer().Deserialize<PhotoBarcode>(json);
      if (!(photoBarcode.status == "SUCCESS"))
        return;
      DirectoryInfo directoryInfo;
      if (photoBarcode.items.is_printable == "yes")
      {
        if (string.IsNullOrWhiteSpace(this.PrePrintSuccess_Directories[index]))
          return;
        directoryInfo = new DirectoryInfo(this.PrePrintSuccess_Directories[index]);
        if (!directoryInfo.Exists)
          directoryInfo.Create();
      }
      else
      {
        if (string.IsNullOrWhiteSpace(this.PrePrintFailed_Directories[index]))
          return;
        directoryInfo = new DirectoryInfo(this.PrePrintFailed_Directories[index]);
        if (!directoryInfo.Exists)
          directoryInfo.Create();
      }
      this.b.Alignment = AlignmentPositions.CENTER;
      this.b.Width = this.BarcodeWs[index];
      this.b.Height = this.BarcodeHs[index];
      this.b.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      this.b.IncludeLabel = true;
      if (this.Horizontal[index])
        this.b.RotateFlipType = RotateFlipType.Rotate270FlipNone;
      this.b.EncodedType = TYPE.CODE39Extended;
      Image image = this.b.Encode(TYPE.CODE39Extended, "*" + photoBarcode.items.barcode + "*", Color.Black, Color.White, this.b.Width, this.b.Height);
      string filename = Path.Combine(directoryInfo.FullName, finalPhotoName);
      using (Image original = Image.FromFile(source))
      {
        using (Bitmap bitmap = new Bitmap(original))
        {
          bitmap.SetResolution(original.HorizontalResolution, original.VerticalResolution);
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            if (this.Horizontal[index])
              graphics.DrawImage(image, this.BarcodeXs[index], this.BarcodeYs[index], this.BarcodeHs[index], this.BarcodeWs[index]);
            else
              graphics.DrawImage(image, this.BarcodeXs[index], this.BarcodeYs[index], this.BarcodeWs[index], this.BarcodeHs[index]);
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            bitmap.Save(filename, imageEncoders[1], encoderParams);
            image.Dispose();
            original.Dispose();
            bitmap.Dispose();
            graphics.Dispose();
          }
        }
      }
      File.Delete(source);
    }

    private List<OutputFoldersStructure> handleOPFolders(string OutputFoldersJson)
    {
      List<OutputFoldersStructure> foldersStructureList1 = new List<OutputFoldersStructure>();
      try
      {
        List<OutputFoldersStructure> foldersStructureList2 = new JavaScriptSerializer().Deserialize<List<OutputFoldersStructure>>(OutputFoldersJson);
        int num = 0;
        foreach (OutputFoldersStructure foldersStructure in foldersStructureList2)
        {
          if (foldersStructure.active)
          {
            foldersStructureList1.Add(new OutputFoldersStructure()
            {
              outputFolder = foldersStructure.outputFolder,
              cycle = foldersStructure.cycle,
              amount = foldersStructure.amount,
              active = foldersStructure.active,
              queue = num == 0,
              prefixValue = foldersStructure.prefixValue,
              loop = 0
            });
            ++num;
          }
        }
        return foldersStructureList1;
      }
      catch
      {
        this.router.goBarcodePrintSettings(this.router);
        return foldersStructureList1;
      }
    }

    protected virtual bool IsFileLocked(FileInfo file)
    {
      FileStream fileStream = (FileStream) null;
      try
      {
        fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
      }
      catch (IOException ex)
      {
        return true;
      }
      finally
      {
        fileStream?.Close();
      }
      return false;
    }

    public void folderNotExistsCreate(string folder)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(folder);
      if (directoryInfo.Exists)
        return;
      directoryInfo.Create();
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
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.SuspendLayout();
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = SystemColors.ButtonHighlight;
      this.label1.Location = new Point(-17, 75);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1025, 32);
      this.label1.TabIndex = 5;
      this.label1.Text = "BarCode && Print";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.listView1.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(39, 124);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(940, 454);
      this.listView1.TabIndex = 7;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.columnHeader1.Text = "Date";
      this.columnHeader1.Width = 120;
      this.columnHeader2.Text = "Size";
      this.columnHeader2.Width = 120;
      this.columnHeader3.Text = "Loop";
      this.columnHeader3.Width = 120;
      this.columnHeader4.Text = "File";
      this.columnHeader4.Width = 220;
      this.columnHeader5.Text = "Output";
      this.columnHeader5.Width = 220;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources._667;
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (BarcodePrint);
      this.Size = new Size(1084, 616);
      this.ResumeLayout(false);
    }
  }
}
