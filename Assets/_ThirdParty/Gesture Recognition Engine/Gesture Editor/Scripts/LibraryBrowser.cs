//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Class that browses libraries of gestures, allowing deletion of libraries and gestures, creation of libraries and gestures,
/// and selection of libraries to test gestures against.
/// </summary>
public class LibraryBrowser : MonoBehaviour
{
	/// <summary>
	/// the directory where the library files are stored 
	/// </summary>
	private const string _libraryPath = "Libraries";
	
	/// <summary>
	/// the directory where the library files are stored if this project is embedded in an asset store project 
	/// </summary>
	private const string _alternateLibraryPath = "Gesture Recognition Engine/Gesture Editor/Libraries";
	
	/// <summary>
	/// the file extension for the library should be a text file 
	/// </summary>
	private const string _fileExtension = ".txt";
	
	/// <summary>
	/// mode of the browser, determining what elements are drawn in OnGUI 
	/// </summary>
	private enum MODE
	{
		BrowseLibraries,
		CreateLibrary,
		RenameLibrary,
		BrowseGestures,
		CreateGesture,
		ConfirmDeleteLibrary,
		ConfirmDeleteGesture
	}
	
	/// <summary>
	/// move direction used by the gesture browser to reorganize gestures 
	/// </summary>
	private enum MOVE_DIR
	{
		Up = -1,
		Down = 1
	}
	
	/// <summary>
	/// mode of the browser, see enum MODE 
	/// </summary>
	private MODE _mode;
	
	/// <summary>
	/// GUI Style used for centering label text 
	/// </summary>
	private GUIStyle _centeredLabelStyle;
	
	/// <summary>
	/// GUI Style used for centering text input boxes 
	/// </summary>
	private GUIStyle _centeredTextAreaStyle;
	
	/// <summary>
	/// the path of the library file 
	/// </summary>
	private string _filePath = "";
	
	/// <summary>
	/// window for the library browser 
	/// </summary>
	private Rect _libraryWindowRect = new Rect(10, 40, 620, 430);
	
	/// <summary>
	/// window for the new library popup 
	/// </summary>
	private Rect _newLibraryWindowRect = new Rect(100, 100, 440, 100);
	
	/// <summary>
	/// window for the gesture browser 
	/// </summary>
	private Rect _gestureWindowRect = new Rect(10, 40, 620, 430);
	
	/// <summary>
	/// popup window for confirming a user's actions 
	/// </summary>
	private Rect _popupQuestionRect = new Rect(100, 100, 440, 100);
	
	/// <summary>
	/// scroll for the file browser 
	/// </summary>
	private Vector2 _scrollFilesVector = Vector2.zero;
	
	/// <summary>
	/// index of the currently selected library file 
	/// </summary>
	private int _fileSelectionIndex;
	
	/// <summary>
	/// index of the currently selected library file for renaming 
	/// </summary>
	private int _fileRenameIndex;
	
	/// <summary>
	/// index of the currently selected library file for deletion 
	/// </summary>
	private int _fileDeletionIndex;
	
	/// <summary>
	/// list of library file names 
	/// </summary>
	private string [] _fileNames;
	
	/// <summary>
	/// list of Rename buttons to go beside the file names 
	/// </summary>
	private string [] _renameFileNames;
	
	/// <summary>
	/// list of X's to delete the library files 
	/// </summary>
	private string [] _deleteFileNames;
	
	/// <summary>
	/// list of file paths used when selecting a library file 
	/// </summary>
	private string [] _filePaths;

	/// <summary>
	/// scroll for the gesture browser 
	/// </summary>
	private Vector2 _scrollGesturesVector = Vector2.zero;
	
	/// <summary>
	/// index of the selected gesture 
	/// </summary>
	private int _gestureSelectionIndex;
	
	/// <summary>
	/// index of the selected gesture for deletion 
	/// </summary>
	private int _gestureDeletionIndex;
	
	/// <summary>
	/// index of the selected gesture to move up 
	/// </summary>
	private int _gestureMoveUpIndex;
	
	/// <summary>
	/// index of the selected gesture to move down
	/// </summary>
	private int _gestureMoveDownIndex;
	
	/// <summary>
	/// list of the gesture names in a library 
	/// </summary>
	private string [] _gestureNames;
	
	/// <summary>
	/// list of the up arrows to move gestures in the library
	/// </summary>
	private string [] _gestureMoveUpNames;
	
	/// <summary>
	/// list of the down arrows to move gestures in the library
	/// </summary>
	private string [] _gestureMoveDownNames;
	
	/// <summary>
	/// list of X's to delete the gestures from the library 
	/// </summary>
	private string [] _deleteGestureNames;
	
	/// <summary>
	/// the new library name used when creating a library 
	/// </summary>
	private string _newLibraryName = "";
		
	/// <summary>
	/// the private accessor used to set the layout when the mode changes 
	/// </summary>
	private MODE Mode
	{
		set
		{
			_mode = value;
			
			//Debug.Log("Mode = " + _mode.ToString() + " " + Time.realtimeSinceStartup);
			
			switch (_mode)
			{
			case MODE.BrowseLibraries:
				// load the list of libraries
				LoadDirectoriesAndFiles();
				break;
				
			case MODE.BrowseGestures:
				// load the list of gestures
				LoadGestures();
				break;
			}
		}
	}
	
	/// <summary>
	/// Initializes the browser 
	/// </summary>
	void Start()
	{
		// initialize the lists
		_gestureNames = new string[0];
		_deleteGestureNames = new string[0];
		
		// get the file path based on the application path plus the user specified sub-directory
		_filePath = Application.dataPath + @"/" + _libraryPath + @"/";
		
		DirectoryInfo di = new DirectoryInfo(_filePath);
		if (!di.Exists)
		{
			_filePath = Application.dataPath +@"/" + _alternateLibraryPath + @"/";
			di = new DirectoryInfo(_filePath);
		}
		
		// initialize the selection idices
		_fileSelectionIndex = -1;
		_fileRenameIndex = -1;
		_fileDeletionIndex = -1;
		_gestureSelectionIndex = -1;
		_gestureDeletionIndex = -1;
		
		// set the initial mode to browsing the libraries
		Mode = MODE.BrowseLibraries;
		
		// if we have been browsing a library's gesture, set the mode to that library
		if (PlayerPrefs.GetString("LibraryPath", "") != "")
		{
			// look for the index of this library
			_fileSelectionIndex = -1;
			for (int i=0; i<_filePaths.Length; i++)
			{
				if (_filePaths[i] == PlayerPrefs.GetString("LibraryPath", ""))
				{
					_fileSelectionIndex = i;
					break;
				}
			}
			
			if (_fileSelectionIndex != -1)
			{
				// we found the index, so set the mode to browse the gestures
				Mode = MODE.BrowseGestures;
			}
		}
	}
	
	/// <summary>
	/// Sets the layout of the browser 
	/// </summary>
	void OnGUI()
	{
		// set the centered label style
		_centeredLabelStyle = GUI.skin.GetStyle("Label");
		_centeredLabelStyle.alignment = TextAnchor.UpperCenter;
		
		// set the centered text input box style
		_centeredTextAreaStyle = GUI.skin.GetStyle("TextArea");
		_centeredTextAreaStyle.alignment = TextAnchor.UpperCenter;
		
		// change how the browser looks depending on the mode
		switch (_mode)
		{
		case MODE.BrowseLibraries:
			// browsing the libraries
			
			// button: create a new library
			if (GUI.Button(new Rect(10, 5, 100, 30), "New Library"))
			{
				_fileSelectionIndex = -1;
				Mode = MODE.CreateLibrary;
			}
			
			// open up the new library window
			_libraryWindowRect = GUI.Window(0, _libraryWindowRect, LibraryBrowserWindow, "Libraries");	
			break;

		case MODE.CreateLibrary:
			// create a new library
			_newLibraryWindowRect = GUI.Window(0, _newLibraryWindowRect, NewLibraryWindow, "New Library");	
			break;
			
		case MODE.RenameLibrary:
			// rename a library
			_newLibraryWindowRect = GUI.Window(0, _newLibraryWindowRect, RenameLibraryWindow, "Rename Library");	
			break;

		case MODE.BrowseGestures:
			// browsing the gestures of a library
			
			// button: go back to the list of libraries
			if (GUI.Button(new Rect(10, 5, 100, 30), "Libraries"))
			{
				_fileSelectionIndex = -1;
				PlayerPrefs.SetString("LibraryPath", "");
				Mode = MODE.BrowseLibraries;
			}
			
			// if there are any gestures in this library
			if (_gestureNames.Length > 0)
			{
				// button: test the gestures of this library
				if (GUI.Button(new Rect(115, 5, 100, 30), "Test Gestures"))
				{
					if (_fileSelectionIndex != -1)
					{
						PlayerPrefs.SetString("LibraryPath", _filePaths[_fileSelectionIndex].Trim());
						Application.LoadLevel("TestGestures");
					}
				}
			}
			
			// button: create a new gesture
			if (GUI.Button(new Rect(220, 5, 100, 30), "New Gesture"))
			{
				if (_fileSelectionIndex != -1)
				{
					PlayerPrefs.SetString("LibraryPath", _filePaths[_fileSelectionIndex].Trim());
					PlayerPrefs.SetString("GestureName", "New Gesture");
					PlayerPrefs.SetInt("GestureIndex", -1);
				
					Application.LoadLevel("GestureEditor");
				}
			}
			
			// if a library file is specified, then show the gestures browser
			if (_fileSelectionIndex != -1)
				_gestureWindowRect = GUI.Window(0, _gestureWindowRect, GestureBrowserWindow, _fileNames[_fileSelectionIndex] + "'s Gestures");
			break;
			
		case MODE.CreateGesture:
			break;
			
		case MODE.ConfirmDeleteLibrary:
		case MODE.ConfirmDeleteGesture:
			// popup window confirming deletion of the library or gesture
			_popupQuestionRect = GUI.Window(0, _popupQuestionRect, PopupQuestionWindow, "Confirm Delete");
			break;			
		}
	}
	
	/// <summary>
	/// Window showing the library browser 
	/// </summary>
	private void LibraryBrowserWindow(int windowID)
	{
		// reset the selection indices
		_fileSelectionIndex = -1;
		_fileRenameIndex = -1;
		_fileDeletionIndex = -1;
		
		// scrollview: list of library names and their deletion buttons
		_scrollFilesVector = GUI.BeginScrollView(new Rect (10,25,_libraryWindowRect.width-20,_gestureWindowRect.height-65), _scrollFilesVector, new Rect(0, 0, _libraryWindowRect.width-40, 1000));
		_fileSelectionIndex = GUI.SelectionGrid (new Rect (0, 0, _libraryWindowRect.width-180, _fileNames.Length*30), _fileSelectionIndex, _fileNames, 1);
		_fileRenameIndex = GUI.SelectionGrid (new Rect (_libraryWindowRect.width-175, 0, 95, _renameFileNames.Length*30), _fileRenameIndex, _renameFileNames, 1);
		_fileDeletionIndex = GUI.SelectionGrid (new Rect (_libraryWindowRect.width-75, 0, 35, _deleteFileNames.Length*30), _fileDeletionIndex, _deleteFileNames, 1);
		GUI.EndScrollView();	
		
		// if a library was selected, switch to browsing its gestures
		if (_fileSelectionIndex > -1)
		{
			Mode = MODE.BrowseGestures;
		}
		
		// if a library is selected for renaming, switch to the rename screen
		if (_fileRenameIndex > -1)
		{
			_newLibraryName = _fileNames[_fileRenameIndex].Replace(_fileExtension, "");
			Mode = MODE.RenameLibrary;
		}
		
		// if a library was deleted, switch to confirming the deletion		
		if (_fileDeletionIndex > -1)
		{
			Mode = MODE.ConfirmDeleteLibrary;
		}
	}	
	
	/// <summary>
	/// Window allowing user to change the name of a library 
	/// </summary>
	private void NewLibraryWindow(int windowID)
	{
		// textfield: enter the new library name
		_newLibraryName = GUI.TextField(new Rect(15, 30, 415, 20), _newLibraryName, _centeredTextAreaStyle);
		
		// button: create the new library
		if (GUI.Button(new Rect(_newLibraryWindowRect.width-180,_newLibraryWindowRect.height-30,80,20), "Create"))
		{
			// strip out file extensions
			if (_newLibraryName.Contains("."))
			{
				int pos = _newLibraryName.IndexOf(".");
				_newLibraryName = _newLibraryName.Substring(0, pos);
			}
			
			// trim
			_newLibraryName = _newLibraryName.Trim();
			
			if (_newLibraryName != "")
			{
				// create the file
				System.IO.File.Create(_filePath + _newLibraryName + _fileExtension);
				_newLibraryName = "";
			}
				
			// switch back to browsing the libraries
			Mode = MODE.BrowseLibraries;
		}
		
		// button: cancel creation
		if (GUI.Button(new Rect(_newLibraryWindowRect.width-90,_newLibraryWindowRect.height-30,80,20), "Cancel"))
		{
			_newLibraryName = "";
			Mode = MODE.BrowseLibraries;
		}
	}	
	
	/// <summary>
	/// Window allowing user to enter the name of the new library 
	/// </summary>
	private void RenameLibraryWindow(int windowID)
	{
		// textfield: enter the new library name
		_newLibraryName = GUI.TextField(new Rect(15, 30, 415, 20), _newLibraryName, _centeredTextAreaStyle);
		
		// button: rename the library
		if (GUI.Button(new Rect(_newLibraryWindowRect.width-180,_newLibraryWindowRect.height-30,80,20), "Rename"))
		{
			// strip out file extensions
			if (_newLibraryName.Contains("."))
			{
				int pos = _newLibraryName.IndexOf(".");
				_newLibraryName = _newLibraryName.Substring(0, pos);
			}
			
			// trim
			_newLibraryName = _newLibraryName.Trim();
			
			if (_newLibraryName != "")
			{
				// rename the file by moving it to the new name
				System.IO.File.Move(_filePath + _fileNames[_fileRenameIndex] + _fileExtension, _filePath + _newLibraryName + _fileExtension);
			}
			
			// switch back to browsing the libraries
			Mode = MODE.BrowseLibraries;
		}
		
		// button: cancel creation
		if (GUI.Button(new Rect(_newLibraryWindowRect.width-90,_newLibraryWindowRect.height-30,80,20), "Cancel"))
		{
			_newLibraryName = "";
			Mode = MODE.BrowseLibraries;
		}
	}	
	
	/// <summary>
	/// Window allowing user to browse the gestures in a library 
	/// </summary>
	private void GestureBrowserWindow(int windowID)
	{
		// reset the selection indices
		_gestureSelectionIndex = -1;
		_gestureDeletionIndex = -1;
		_gestureMoveUpIndex = -1;
		_gestureMoveDownIndex = -1;
		
		// if there are gestures in this library
		if (_gestureNames.Length > 0)
		{
			// scrollview: gesture names, movement, and deletion buttons
			_scrollGesturesVector = GUI.BeginScrollView(new Rect (10,25,_gestureWindowRect.width-20,_gestureWindowRect.height-65), _scrollGesturesVector, new Rect(0, 0, _gestureWindowRect.width-40, 3000));
			_gestureMoveUpIndex = GUI.SelectionGrid (new Rect (0, 31, 35, _gestureMoveUpNames.Length*30), _gestureMoveUpIndex, _gestureMoveUpNames, 1);
			_gestureMoveDownIndex = GUI.SelectionGrid (new Rect (40, 0, 35, _gestureMoveDownNames.Length*30), _gestureMoveDownIndex, _gestureMoveDownNames, 1);
			_gestureSelectionIndex = GUI.SelectionGrid (new Rect (80, 0, _gestureWindowRect.width-160, _gestureNames.Length*30), _gestureSelectionIndex, _gestureNames, 1);
			_gestureDeletionIndex = GUI.SelectionGrid (new Rect (_gestureWindowRect.width-75, 0, 35, _gestureNames.Length*30), _gestureDeletionIndex, _deleteGestureNames, 1);
			GUI.EndScrollView();	
			
			// if gesture was selected, open the gesture editor
			if (_gestureSelectionIndex > -1)
			{
				PlayerPrefs.SetString("LibraryPath", _filePaths[_fileSelectionIndex].Trim());
				PlayerPrefs.SetString("GestureName", _gestureNames[_gestureSelectionIndex].Trim());
				PlayerPrefs.SetInt("GestureIndex", _gestureSelectionIndex);
				
				Application.LoadLevel("GestureEditor");
			}
			
			// if gesture was deleted, confirm deletion
			if (_gestureDeletionIndex > -1)
			{
				Mode = MODE.ConfirmDeleteGesture;
			}		
			
			// if the gesture was selected to move up, then move it and resave
			if (_gestureMoveUpIndex > -1)
			{
				// we add one to the index, since the buttons are offset by one
				MoveGesture(_gestureMoveUpIndex+1, MOVE_DIR.Up);
				Mode = MODE.BrowseGestures;
			}
			
			// if the gesture was selected to move down, then move it and resave
			if (_gestureMoveDownIndex > -1)
			{
				MoveGesture(_gestureMoveDownIndex, MOVE_DIR.Down);
				Mode = MODE.BrowseGestures;
			}
		}		
	}		
	
	/// <summary>
	/// Popup window used to confirm user actions 
	/// </summary>
	private void PopupQuestionWindow(int windowID)
	{	
		string message = "";
		
		// change the message depending on the user's action
		switch (_mode)
		{
		case MODE.ConfirmDeleteLibrary:
			message = "Are you sure you want to delete library [ " + _fileNames[_fileDeletionIndex] + " ]?";
			break;
			
		case MODE.ConfirmDeleteGesture:
			message = "Are you sure you want to delete gesture [ " + _gestureNames[_gestureDeletionIndex] + " ]?";
			break;
		}

		// label: show the message
		GUI.Label(new Rect(10, 20, _popupQuestionRect.width - 20, _popupQuestionRect.height - 35), message, _centeredLabelStyle);
		
		// button: confirm the user's action
		if (GUI.Button(new Rect(_popupQuestionRect.width-180,_popupQuestionRect.height-30,80,20), "Yes"))
		{
			switch (_mode)
			{
			case MODE.ConfirmDeleteLibrary:
				// delete the library file
				
				System.IO.File.Delete(_filePath + @"/" + _fileNames[_fileDeletionIndex] + _fileExtension);
				
				if (PlayerPrefs.GetString("LibraryPath", "") == _filePath + @"/" + _fileNames[_fileDeletionIndex] + _fileExtension)
				{
					PlayerPrefs.SetString("LibraryPath", "");
				}
				
				// switch back to browsing libraries
				Mode = MODE.BrowseLibraries;
				break;
				
			case MODE.ConfirmDeleteGesture:
				// delete the gesture from the library
				
				DeleteGesture(_gestureDeletionIndex);
				
				// switch back to browsing gestures
				Mode = MODE.BrowseGestures;
				break;
			}
		}
		
		// button: cancel confirm
		if (GUI.Button(new Rect(_popupQuestionRect.width-90,_popupQuestionRect.height-30,80,20), "No"))
		{
			switch (_mode)
			{
			case MODE.ConfirmDeleteLibrary:
				Mode = MODE.BrowseLibraries;
				break;
				
			case MODE.ConfirmDeleteGesture:
				Mode = MODE.BrowseGestures;
				break;
			}
		}
	}		
	
	/// <summary>
	/// Loads the list of libraries from the library path 
	/// </summary>
	private void LoadDirectoriesAndFiles()
	{
		int i;
		DirectoryInfo directoryInfo = null;
		FileInfo [] fileInfos;

		directoryInfo = new DirectoryInfo(_filePath);
		
		// get all the files in the directory with the proper extension
		fileInfos = directoryInfo.GetFiles("*" + _fileExtension);
		
		// initialize the arrays
		_fileNames = new string[fileInfos.Length];
		_renameFileNames = new string[fileInfos.Length];
		_deleteFileNames = new string[fileInfos.Length];
		_filePaths = new string[fileInfos.Length];
		
		// set the arrays based on the files
		for (i=0; i<fileInfos.Length; i++)
		{
			_fileNames[i] = fileInfos[i].Name.Replace(_fileExtension, "");
			_filePaths[i] = fileInfos[i].FullName;
			
			_renameFileNames[i] = "Rename";
			_deleteFileNames[i] = "X";
		}
	}
	
	/// <summary>
	/// Load the gestures from a library file 
	/// </summary>
	private void LoadGestures()
	{
		StreamReader sr = new StreamReader(_filePaths[_fileSelectionIndex]);
		List<string> lines = new List<string>();
		string inputLine;
		int i;
		
		// add all the lines in the file (we do this first so that we can initialize the arrays with the right element count)
		while (!sr.EndOfStream)
		{
			inputLine = sr.ReadLine().Trim();
			
			// if not a blank line
			if (inputLine != "")
			{
				// if not a comment
				if (inputLine.Substring(0, 1) != "'")
				{
					lines.Add(inputLine);
				}
			}
		}
		
		sr.Close();
		sr.Dispose();
		
		// initialize the arrays
		_gestureNames = new string[lines.Count];
		_deleteGestureNames = new string[lines.Count];
		
		if (lines.Count > 0)
		{
			_gestureMoveUpNames = new string[lines.Count-1];
			_gestureMoveDownNames = new string[lines.Count-1];
		}
		
		// set the gestures names
		for (i=0; i<lines.Count; i++)
		{
			string [] field = lines[i].Split(":"[0]);
			
			_gestureNames[i] = field[0];
			_deleteGestureNames[i] = "X";
			
			if (i < (lines.Count-1))
			{
				_gestureMoveUpNames[i] = @"/\";
				_gestureMoveDownNames[i] = @"\/";
			}
		}
		
		lines.Clear();
	}
	
	/// <summary>
	/// Deletes a gesture from a library file 
	/// </summary>
	/// <param name="gestureName">
	/// Name of the gesture to remove
	/// </param>
	private void DeleteGesture(int index)
	{	
		// load in the lines from the file
		StreamReader sr = new StreamReader(_filePaths[_fileSelectionIndex]);
		List<string> lines = new List<string>();
		string inputLine;
		int i;
		
		while (!sr.EndOfStream)
		{
			inputLine = sr.ReadLine().Trim();
			
			// if not a blank line
			if (inputLine != "")
			{
				// if not a comment
				if (inputLine.Substring(0, 1) != "'")
				{
					lines.Add(inputLine);
				}
			}
		}
		
		sr.Close();
		sr.Dispose();
		
		// open a writer to write back the data
		StreamWriter sw = new StreamWriter(_filePaths[_fileSelectionIndex], false, System.Text.Encoding.ASCII);
		
		// loop through each line.
		// if the line contains the gesture to be removed, don't write it back to the new file
		for (i=0; i<lines.Count; i++)
		{
			if (i != index)
				sw.WriteLine(lines[i]);
		}
		
		sw.Flush();
		sw.Close();
		sw.Dispose();
		
		lines.Clear();		
	}
	
	/// <summary>
	/// Moves the gesture selected up or down 
	/// </summary>
	/// <param name="gestureIndex">
	/// The index of the gesture to move
	/// </param>
	/// <param name="moveDirection">
	/// The direction to move the gesture
	/// </param>
	private void MoveGesture(int gestureIndex, MOVE_DIR moveDirection)
	{
		// load in the lines from the file
		StreamReader sr = new StreamReader(_filePaths[_fileSelectionIndex]);
		List<string> lines = new List<string>();
		
		while (!sr.EndOfStream)
		{
			lines.Add(sr.ReadLine());
		}
		
		sr.Close();
		sr.Dispose();
		
		// swap the lines
		string tempLine = "";
		tempLine = lines[gestureIndex];
		lines[gestureIndex] = lines[gestureIndex + (int)moveDirection];
		lines[gestureIndex + (int)moveDirection] = tempLine;
		
		// open a writer to write back the data
		StreamWriter sw = new StreamWriter(_filePaths[_fileSelectionIndex], false, System.Text.Encoding.ASCII);
		
		// loop through each line, writing them back to the file
		foreach (string line in lines)
		{
			sw.WriteLine(line);
		}
		
		sw.Flush();
		sw.Close();
		sw.Dispose();
		
		lines.Clear();			
	}
}