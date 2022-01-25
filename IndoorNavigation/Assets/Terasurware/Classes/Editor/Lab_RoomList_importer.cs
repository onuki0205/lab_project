using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Lab_RoomList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Terasurware/ExcelData/Lab_RoomList.xls";
	private static readonly string exportPath = "Assets/Terasurware/ExcelData/Lab_RoomList.asset";
	private static readonly string[] sheetNames = { "LabRoom1F","LabRoom2F","LabRoom3F", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_LabRoom data = (Entity_LabRoom)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_LabRoom));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_LabRoom> ();
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

					Entity_LabRoom.Sheet s = new Entity_LabRoom.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_LabRoom.Param p = new Entity_LabRoom.Param ();
						
					cell = row.GetCell(0); p.ID = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.X = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Y = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.ClosestNode = (int)(cell == null ? 0 : cell.NumericCellValue);
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
