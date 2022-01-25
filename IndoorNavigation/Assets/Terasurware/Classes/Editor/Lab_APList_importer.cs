using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Lab_APList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Terasurware/ExcelData/Lab_APList.xls";
	private static readonly string exportPath = "Assets/Terasurware/ExcelData/Lab_APList.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_AP data = (Entity_AP)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_AP));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_AP> ();
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

					Entity_AP.Sheet s = new Entity_AP.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_AP.Param p = new Entity_AP.Param ();
						
						cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
						cell = row.GetCell(1); p.SSID = (cell == null ? "" : cell.StringCellValue);
						cell = row.GetCell(2); p.IP = (cell == null ? "" : cell.StringCellValue);
						cell = row.GetCell(3); p.X = (float)(cell == null ? 0 : cell.NumericCellValue);
						cell = row.GetCell(4); p.Y = (float)(cell == null ? 0 : cell.NumericCellValue);
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
