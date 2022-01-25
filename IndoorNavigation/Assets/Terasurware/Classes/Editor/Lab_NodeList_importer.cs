using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Lab_NodeList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Terasurware/ExcelData/Lab_NodeList.xls";
	private static readonly string exportPath = "Assets/Terasurware/ExcelData/Lab_NodeList.asset";
	private static readonly string[] sheetNames = { "LabNode1F","LabNode2F","LabNode3F", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_LabNode data = (Entity_LabNode)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_LabNode));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_LabNode> ();
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

					Entity_LabNode.Sheet s = new Entity_LabNode.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_LabNode.Param p = new Entity_LabNode.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.X = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Y = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.connectID = (cell == null ? "" : cell.StringCellValue);
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
