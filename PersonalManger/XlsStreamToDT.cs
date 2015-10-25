using NPOI.HSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

/// <summary>
/// XlsStreamToDT 的摘要说明
/// 
/// MaxColumNum属性表示xls文件的列数；
/// XlsStream属性表示xls文件流；
/// Xls2DT方法返回DataTable；
/// </summary>
public class XlsStreamToDT
{
    private int maxColumNum;
    private Stream stream;
    private int sheetNum;
    public int MaxColumNum
    {
        get
        {
            return maxColumNum;
        }
        set
        {
            maxColumNum = value;
        }
    }
    public Stream XlsStream
    {
        get { return this.stream; }
        set { this.stream = value; }
    }
    public int SheetNum
    {
        get
        {
            return sheetNum;
        }
        set
        {
            sheetNum = value;
        }
    }
    public XlsStreamToDT()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public XlsStreamToDT(Stream stream, int maxColumNum)
    {
        this.stream = stream;
        this.maxColumNum = maxColumNum;
    }
    private DataTable XlsToDataTable(HSSFWorkbook hw)
    {
        int hasNull = 0;
        try
        {
            DataTable dt = new DataTable();
            HSSFSheet sheet = (HSSFSheet)hw.GetSheetAt(SheetNum);
            IEnumerator rows = sheet.GetRowEnumerator();
            for (int i = 0; i < MaxColumNum; i++)
            {
                dt.Columns.Add(GetColumnName(i + 1).Trim());
            }
            while (rows.MoveNext())
            {
                hasNull = 0;
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                for (int i = 0; i < MaxColumNum; i++)
                {
                    HSSFCell cell = (HSSFCell)row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString().Trim();
                    }
                }
                foreach (object obj in dr.ItemArray)
                {
                    if (string.IsNullOrEmpty(obj.ToString().Trim()))
                    {
                        hasNull++;
                    }
                }
                if (hasNull < MaxColumNum)
                    dt.Rows.Add(dr);


            }
            if (dt.Rows.Count < 1)
                return null;
            else
                return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable Xls2DT()
    {
        try
        {
            HSSFWorkbook hw = new HSSFWorkbook(stream);
            return XlsToDataTable(hw);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private string GetColumnName(int num)
    {
        List<string> list = new List<string>();
        string result = "";
        int yushu = 0;
        for (; num > 0; )
        {
            yushu = num % 26;
            num = (int)(num / 26);
            list.Add(yushu.ToString());
        }
        int jiewei = 0;
        for (int i = 0; i < list.Count; i++)
        {
            string s = list[i];
            char c = (char)Convert.ToInt32(s);
            char n_c = (char)(c + jiewei);
            switch (n_c)
            {
                case (char)0: result += (c = ((char)('A' + 25))).ToString(); break;
                default: result += (c = (char)('A' + n_c - 1)).ToString(); break;
            }
            if (c == 'Z')
                jiewei = -1;
            else
                jiewei = 0;
        }
        result = result[result.Length - 1] == 'Z' ? result.Remove(result.Length - 1) : result;
        result = Fanzhuan(result);
        return result;
    }

    private string Fanzhuan(string s)
    {
        string r = "";
        for (int i = s.Length - 1; i >= 0; i--)
        {
            r += s[i].ToString();
        }
        return r;
    }
}


public class Product
{
    private int age;
    private string name;
    public int Age
    {
        set { age=value;}
        get { return age; }
    }

    public string Name
    {
        set { name = value; }
        get { return name; }
    }

    public Product(int age, string name)
    {
        this.Age= age;
        this.Name = name;
    }

    public void ShowInfo()
    {
        Console.WriteLine(this.Name+this.Age);
    }
}
