using UnityEngine;
using System.Collections;

public class NGCreateAvatarPopup : MonoBehaviour
{
    public UISprite _maleButtonSprite;
    public UISprite _femaleButtonSprite;

    public UILabel _avatarNameTextBox;
    public UILabel _errorLabel;
	
    private string _avatarName;
    private string _gender = "Male";
	private bool _genderSelected = false;
	
    private UI_Raid.CharacterSelectionHandler _listenerInterface;
	
    public void SetListenerInterface(UI_Raid.CharacterSelectionHandler listenerInterface)
    {
        _listenerInterface = listenerInterface;
    }
	
    public void PickMale()
    {
        _gender = "Male";
        _maleButtonSprite.spriteName = "MaleButtonPressed";
        _femaleButtonSprite.spriteName = "FemaleButton";
		_genderSelected = true;

        if (_listenerInterface != null)
            _listenerInterface.OnMaleCharacterSelected();
    }
	
    public void PickFemale()
    {
        _gender = "Female";
        _femaleButtonSprite.spriteName = "FemaleButtonPressed";
        _maleButtonSprite.spriteName = "MaleButton";
		_genderSelected = true;
		
        if (_listenerInterface != null)
            _listenerInterface.OnFemaleCharacterSelected();
    }

    public void OnInputTextChanged()
    {
        ShowInvalidUsername("");
    }
	
    public void Confirm()
    {
        _avatarName = _avatarNameTextBox.text;
        Debug.Log("Confirmed");
        Debug.Log("Avatar Name : " + _avatarName);
        Debug.Log("Gender : " + _gender);


		if(!_genderSelected)
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Select Gender", "", () => {});

		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to create Avatar", () => {});
			return;
		}

        if (_listenerInterface != null && _genderSelected)
            _listenerInterface.OnContinueClick(_avatarName, _gender.ToLower() == "male");
    }
	
    public void ShowInvalidUsername(string message)
    {
        Debug.Log("Invalid username! > " + message);
        _errorLabel.text = message;
    }
	
    public void ClearTextBox()
    {
        _avatarNameTextBox.text = "";
    }
}
