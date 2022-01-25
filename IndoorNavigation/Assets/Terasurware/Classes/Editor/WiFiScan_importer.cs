using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class WiFiScan_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Terasurware/ExcelData/WiFiScan.xls";
	private static readonly string exportPath = "Assets/Terasurware/ExcelData/WiFiScan.asset";
	private static readonly string[] sheetNames = { "sheet1","sheet2","sheet3","sheet4","sheet5", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_WiFi data = (Entity_WiFi)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_WiFi));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_WiFi> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_WiFi.Sheet s = new Entity_WiFi.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_WiFi.Param p = new Entity_WiFi.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.SSID = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.MAC = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.RSSI = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.DIS = (float)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
