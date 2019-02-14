﻿using UnityEngine;
using System.Collections;

public interface GameUIInterface
{
	void OnRegisterSWFCallback(NGGameUI obj);
	void MenuButtonClick();
	void resumeGame();
	void onRingsButton();
	void onStaffButton();
	void onOptionButton();
	void onMarketButton();
	void onUpgradeButton(PurchaseManager.GeneralPopupType popupType);
	void onSpiritsButton();
	void onTransmutationButton();
	void OnRegisterSWFChildCallback(MonoBehaviour loadedSwf);
	void playHeartBeat();
	void onVictorySound();
	void onRatingSound();
	void onVictoryContinue();
	void onDefeatSound();
	void onDefeatReturn();
	void onDefeatRestart();
	void lootPopupClosed();
	void onTreasureSound();
	void onSpiralPopupSound();
	void onUnlockGame();
	void loginToFacebook();
	void onSkullLevelTextFocus();
	void resetGame();
	void onGetSkullLevelText(string skullLevel);
	void fpsSet(bool fps);
	void ShowUserID(bool userID);
	void setSkullLevel();
	void bossDefeatPopupClosed();
	void pauseGame();
	void showPausePopup();
	void showLoadingPopup();
	void onOpenChest();
	bool onKeyBuyButton();
	void onPotionBuyButton();
	void onTickSound();
	void CollectShrineReward();
	void ShrineInviteFriend();
	void ShrineConnect();
	void ChargeShrine();
	void onUseHealthPotion();
	void onCloseHealthPopup();
	void ConfermButtonYes(string msgCopy);
	void reTryConnection();
	void GeneralPopupButton1Pressed();
	void GeneralPopupButton2Pressed();
	void GeneralPopup3Button1Pressed();
	void GeneralPopup3Button2Pressed();
	void onClosePopup();
	void onCloseUiGeneralPopup();
	void closeLoadedUI();
	void onHealthIcon();
	void onShrineBoost();
	void supportFaceBook();
	void supportMailUs();
	void RaidsVictoryPopup();
	void RaidsDefeatPopup();
	void onRaidsVictoryPopupClose();
	void onRaidsDefeatPopupClose();
	void NextButton();
	void skipDilog();
}
