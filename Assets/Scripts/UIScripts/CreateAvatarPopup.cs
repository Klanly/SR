//using UnityEngine;
//using System.Collections;
//
//public class CreateAvatarPopup : MonoBehaviour 
//{
//	public dfButton _maleButton;
//	public dfButton _femaleButton;
//	public dfButton _confirmButton;
//	public dfTextbox _avatarNameTextBox;
//	public dfLabel _errorLabel;
//
//	private string _avatarName;
//	private string _gender;
//
//	private UI_Raid.CharacterSelectionHandler _listenerInterface;
//		
//	void Start()
//	{
//		this.PerformDFLayout();
//	}
//	
//	public void SetListenerInterface(UI_Raid.CharacterSelectionHandler listenerInterface)
//	{
//		_listenerInterface = listenerInterface;
//	}
//	
//	public void PickMale()
//	{
//		_gender = "Male";
//		_maleButton.BackgroundSprite = "MaleButtonPressed";
//		_femaleButton.BackgroundSprite = "FemaleButton";
//		
//		if(_listenerInterface != null)
//			_listenerInterface.OnMaleCharacterSelected();
//	}
//
//	public void PickFemale()
//	{
//		_gender = "Female";
//		_femaleButton.BackgroundSprite = "FemaleButtonPressed";
//		_maleButton.BackgroundSprite = "MaleButton";
//		
//		if(_listenerInterface != null)
//			_listenerInterface.OnFemaleCharacterSelected();
//	}
//
//	public void Confirm()
//	{
//		_avatarName = _avatarNameTextBox.Text;
//		Debug.Log("Confirmed");
//		Debug.Log("Avatar Name : " + _avatarName);
//		Debug.Log("Gender : " + _gender);
//		
//		if(_listenerInterface != null)
//			_listenerInterface.OnContinueClick(_avatarName, _gender.ToLower() == "male");
//	}
//	
//	public void ShowInvalidUsername(string message)
//	{
//		Debug.Log("Invalid username! > " + message);
//	}
//	
//	public void ClearTextBox()
//	{
//		_avatarNameTextBox.Text = "";
//	}
//}
