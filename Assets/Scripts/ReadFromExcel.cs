using UnityEngine;
using System.Collections;
using System;
using System.Data;
using System.Data.Odbc;
//read the excel you can try another method like:Interop.Excel.dll and refer to the links: http://bbs.csdn.net/topics/340082590

public class ReadFromExcel : MonoBehaviour {
	static DataTable dtData = new DataTable("YottaData");

	// Use this for initialization
	void Awake()
	{

	}
	void Start () {
		//dtData.Clear();
		//readXLS(Application.dataPath + "/Book1.xls");
		//name = "" + dtData.Rows[2][dtData.Columns[2].ColumnName].ToString();

	} 
	
	// Update is called once per frame
	void Update () {
	
	}
	static public void readXLS( string filetoread)
	{
		// Must be saved as excel 2003 workbook, not 2007, mono issue really

		string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+filetoread+";";
		//Debug.Log(con);
		string yourQuery = "SELECT * FROM [Sheet1$]"; 
		// our odbc connector 
		OdbcConnection oCon = new OdbcConnection(con); 
		// our command object 
		OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
		// table to hold the data 
		
		// open the connection 
		oCon.Open(); 
		// lets use a datareader to fill that table! 
		OdbcDataReader rData = oCmd.ExecuteReader(); 
		// now lets blast that into the table by sheer man power! 
		dtData.Load(rData); 
		// close that reader! 
		rData.Close(); 
		// close your connection to the spreadsheet! 
		oCon.Close(); 
		// wow look at us go now! we are on a roll!!!!! 
		// lets now see if our table has the spreadsheet data in it, shall we? 
		//Debug.Log(dtData.Rows.Count);

	} 

	static public void readXLS(string filename,string sheetname)
	{
		dtData.Clear();
		dtData.Reset();
		string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+filename+";";
		//Debug.Log(sheetname);
		string yourQuery = "SELECT * FROM ["+sheetname+"$]"; 
		// our odbc connector 
		OdbcConnection oCon = new OdbcConnection(con); 
		// our command object 
		OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
		// table to hold the data 
		
		// open the connection 
		oCon.Open(); 
		// lets use a datareader to fill that table! 
		OdbcDataReader rData = oCmd.ExecuteReader(); 
		// now lets blast that into the table by sheer man power! 
		dtData.Load(rData); 
		// close that reader! 
		rData.Close(); 
		// close your connection to the spreadsheet! 
		oCon.Close(); 

	}
	static public string getStringFromRowColm(int row, int col)
	{
		return	dtData.Rows[row-2][col-1].ToString();
	}
	static public int getIntFromRowColm(int row, int col)
	{
		return Convert.ToInt32(dtData.Rows[row-2][col-1]);
	}
	static public float getFloatFromRowColm(int row, int col)
	{
		return Convert.ToSingle(dtData.Rows[row-2][col-1]);
	}

	static public int ExtractIntContinue(string str,int index)
	{
		string tempStr = str.Substring(index);
		return Convert.ToInt32(tempStr);
	}
	static public int ExtractIntContinue(string str,int startIndex,int length)
	{
		string tempStr = str.Substring(startIndex,length);
		return Convert.ToInt32(tempStr);
	}
	static public int ExtractInt(string str)
	{
		char[] strArray = new char[str.Length];
		Int16 i,j=0;
		for(i=0; i<str.Length; ++i)
		{
			if(str[i]>='0' && str[i]<='9')
				strArray[j++] = str[i];
		}
		string tempStr = new string(strArray);
		return Convert.ToInt32(tempStr);
	}



}
