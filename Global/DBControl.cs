using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DataControl
/// </summary>
public class DBControl
{
    private OleDbConnection conn;
    /// <summary>
    /// 获得OleDbConnection
    /// *只读 一般不用
    /// </summary>
    public OleDbConnection Connection
    {
        get
        {
            return conn;
        }
    }

    private OleDbCommand cmd;
    /// <summary>
    /// 获得OleDbCommand实例
    /// *只读（不能改变引用）
    /// *引用（可读写其属性）
    /// </summary>
    public OleDbCommand Command
    {
        get
        {
            return cmd;
        }
    }
    /// <summary>
    /// 更改OleDbCommand的CommandText
    /// *用SQL语句赋值
    /// </summary>
    public string CommandText
    {
        get
        {
            return cmd.CommandText;
        }
        set
        {
            cmd.CommandText = value;
        }
    }

    private OleDbDataReader reader;
    /// <summary>
    /// 获得OleDbDataReader实例
    /// *只读（不能改变引用）
    /// *引用（可读写其属性）
    /// </summary>
    public OleDbDataReader Reader
    {
        get
        {
            return reader;
        }
    }
    /// <summary>
    /// 判断OleDbDataReader是否有内容
    /// *必须先调用ExecuteReader()
    /// </summary>
    public bool HasRow
    {
        get
        {
            if (reader == null)
            {
                return false;
            }
            return reader.HasRows;
        }
    }

    //TODO:DataAdapter DataSet

    /// <summary>
    /// 添加SqlCommand的参数
    /// *CommandText为带@XXX参数的SQL语句时使用
    /// </summary>
    /// <param name="parameterName">参数名称（*不带@）</param>
    /// <param name="value">参数值（*object类型）</param>
    public void AddParameter(string parameterName, object value)
    {
        cmd.Parameters.AddWithValue(parameterName, value);
    }

    /// <summary>
    /// 删除SqlCommand参数
    /// *CommandText为带@XXX参数的SQL语句时使用
    /// </summary>
    /// <param name="parameterName">参数名称（*不带@）</param>
    public void DelParameter(string parameterName)
    {
        cmd.Parameters.RemoveAt(parameterName);
    }

    /// <summary>
    /// 执行查询语句并将结果返回到Reader中
    /// *执行后通过Reader属性获取结果
    /// *必须先给CommandText赋值
    /// </summary>
    public void ExecuteReader()
    {
        if (cmd.CommandText == "")
        {
            return;
        }
        reader = cmd.ExecuteReader();
    }

    /// <summary>
    /// 执行非查询语句
    /// *必须先给CommandText赋值
    /// </summary>
    /// <returns>影响的行数</returns>
    public int ExecuteNonQuery()
    {
        if (cmd.CommandText == "")
        {
            return -1;
        }
        return cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// 关闭Reader
    /// *在使用Reader后如果要执行新的CommandText，必须先关闭Reader
    /// </summary>
    public void CloseReader()
    {
        if (reader != null)
        {
            reader.Close();
        }

    }

    /// <summary>
    /// 关闭数据库连接，释放资源
    /// *数据操作结束后必须调用！
    /// </summary>
    public void CloseConntion()
    {
        if (reader != null)
        {
            reader.Close();
            reader.Dispose();
        }
        cmd.Dispose();
        conn.Close();
        conn.Dispose();
    }

    /// <summary>
    /// 构造方法 生成数据操作类实例
    /// *切记操作结束后调用CloseConntion()关闭连接
    /// </summary>
    public DBControl()
    {
        conn = new OleDbConnection();
        conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Resources\DB\db.mdb";
        conn.Open();
        cmd = new OleDbCommand();
        cmd.Connection = conn;
    }
    /*
    ~DBControl()
    {
        CloseConntion();
    }
     * */
}
