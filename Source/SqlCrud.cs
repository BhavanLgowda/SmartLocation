using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SmartLocationApp.Source
{
  public class SqlCrud
  {
    private SQLiteConnection connection;
    private string SQLInsert = "INSERT INTO SmartLocationLog(imageName,UploadMessage,UploadType,SaleFolder) VALUES(?, ?, ?,?)";
    private string SQLSelectByDate = "SELECT DISTINCT imageName,UploadMessage,UploadType,SaleFolder,date FROM SmartLocationLog where SaleFolder = @SaleFolder";

    public SqlCrud() => this.checkDb();

    private void checkDb()
    {
      if (!File.Exists(ReadWrite.LogPath))
      {
        SQLiteConnection.CreateFile(ReadWrite.LogPath);
        using (this.connection = new SQLiteConnection("Data Source=" + ReadWrite.LogPath + ";Version=3;", true))
        {
          this.connection.Open();
          new SQLiteCommand("\r\n                    CREATE table SmartLocationLog (\r\n                        id INTEGER PRIMARY KEY AUTOINCREMENT,\r\n                        imageName varchar(50),\r\n                        UploadMessage varchar(100),\r\n                        UploadType varchar(20),\r\n                        date TIMESTAMP DEFAULT (DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')),\r\n                        SaleFolder TEXT\r\n                    );\r\n\r\n                    CREATE table failed_photos (\r\n                        id INTEGER PRIMARY KEY AUTOINCREMENT,\r\n                        location_id INTEGER NOT NULL DEFAULT 0,\r\n                        name varchar(50),\r\n                        path varchar(255),\r\n                        message text,\r\n                        created TIMESTAMP DEFAULT (DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')),\r\n                        deleted TIMESTAMP\r\n                    );\r\n\r\n                    CREATE table failed_podcams (\r\n                        id INTEGER PRIMARY KEY AUTOINCREMENT,\r\n                        location_id INTEGER NOT NULL DEFAULT 0,\r\n                        ticket varchar(50),\r\n                        path varchar(255),\r\n                        type varchar(15),\r\n                        message text,\r\n                        created TIMESTAMP DEFAULT (DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')),\r\n                        deleted TIMESTAMP\r\n                    );\r\n\r\n                    ", this.connection).ExecuteNonQuery();
        }
      }
      this.connection = new SQLiteConnection("Data Source=" + ReadWrite.LogPath + ";Version=3;", true);
    }

    public SQLiteConnection GetNewConnection()
    {
      if (!File.Exists(ReadWrite.LogPath))
        this.checkDb();
      return new SQLiteConnection("Data Source=" + ReadWrite.LogPath + ";Version=3;", true);
    }

    public void AddLog(
      string imageName,
      string UploadMessage,
      string UploadType,
      string SaleFolder)
    {
      if (this.connection.State != ConnectionState.Open)
        this.connection.Open();
      SQLiteCommand command = this.connection.CreateCommand();
      command.CommandText = this.SQLInsert;
      command.Parameters.AddWithValue(nameof (imageName), (object) imageName);
      command.Parameters.AddWithValue(nameof (UploadMessage), (object) UploadMessage);
      command.Parameters.AddWithValue(nameof (UploadType), (object) UploadType);
      command.Parameters.AddWithValue(nameof (SaleFolder), (object) SaleFolder);
      command.ExecuteNonQuery();
      this.connection.Close();
    }

    public DataTable GetDataByDayAndBySaleFolder(string SaleFolderPath)
    {
      if (this.connection.State != ConnectionState.Open)
        this.connection.Open();
      SQLiteCommand command = this.connection.CreateCommand();
      command.CommandText = this.SQLSelectByDate;
      command.Parameters.AddWithValue("@SaleFolder", (object) SaleFolderPath);
      DataTable dataTable = new DataTable();
      new SQLiteDataAdapter(command).Fill(dataTable);
      this.connection.Close();
      return dataTable;
    }

    public object ExecuteScalar(string sql)
    {
      SQLiteConnection newConnection = this.GetNewConnection();
      newConnection.Open();
      object obj = new SQLiteCommand(sql, newConnection).ExecuteScalar();
      newConnection.Close();
      return obj;
    }

    public int ExecuteNoneQuery(string sql)
    {
      SQLiteConnection newConnection = this.GetNewConnection();
      newConnection.Open();
      int num = new SQLiteCommand(sql, newConnection).ExecuteNonQuery();
      newConnection.Close();
      return num;
    }

    public int insertAndGetNewId(string sql)
    {
      SQLiteConnection newConnection = this.GetNewConnection();
      newConnection.Open();
      int newId = new SQLiteCommand(sql, newConnection).ExecuteNonQuery();
      if (newId > 0)
        newId = (int) (long) new SQLiteCommand("select last_insert_rowid()", newConnection).ExecuteScalar();
      newConnection.Close();
      return newId;
    }

    public DataTable ExecuteDataTable(string sql)
    {
      DataTable dataTable = new DataTable();
      SQLiteConnection newConnection = this.GetNewConnection();
      newConnection.Open();
      new SQLiteDataAdapter(sql, newConnection).Fill(dataTable);
      newConnection.Close();
      return dataTable;
    }
  }
}
